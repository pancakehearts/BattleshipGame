using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module8_GroupBlue
{
    internal class BluePlayer : IPlayer
    {
        private List<Position> HitPositions = new List<Position>();   // stores all ‘hit’ guesses
        private List<Position> MissPositions = new List<Position>();   // stores all ‘miss’ guesses
        private List<Position> SankPositions = new List<Position>();   // stores all ‘sank’ guesses
        private int _index; // player's index in turn order
        private int _gridSize; // size of grid
        private Ships _ships; // size of grid
        private static readonly Random Random = new Random(); // used to randomize choices
        private char[] directions = { 'N', 'E', 'S', 'W' }; //represents north, east, south, west


        // Constructor:
        public BluePlayer(string name)
        {
            Name = name;
        }

        // Property that returns player's name:
        public string Name { get; }

        // Propery that returns player's index in turn order.
        public int Index => _index;


        // Logic to start new game
        
        public void StartNewGame(int playerIndex, int gridSize, Ships ships)
        {
            _gridSize = gridSize;
            _index = playerIndex;


            
            // Currently: this borrows from RandomPlayer, which just puts the ships in the grid in Random columns
            

            var availableColumns = new List<int>();
            for (int i = 0; i < gridSize; i++)
            {
                availableColumns.Add(i);
            }

            _ships = ships;
            foreach (var ship in ships._ships)
            {
                // Pick an open X from the remaining columns
                var x = availableColumns[Random.Next(availableColumns.Count)];
                availableColumns.Remove(x); //Make sure we can't pick it again

                // Pick a Y that fits
                var y = Random.Next(gridSize - ship.Length);
                ship.Place(new Position(x, y), Direction.Vertical);
            }
        }


        // Method to intelligently find best spot to attack.
        public Position GetAttackPosition()
        {
            Position guess = null;

            // -   Look at the spaces to the north, east, south, or west of each hit (reference the HitPositions array here).
            // -    If the it finds a spot on the grid that doesn’t contain the AI’s own ships, it will shoot at it.
            foreach (Position position in HitPositions)
            {
                foreach (char direction in directions)
                {
                    guess = GetAdjacent(position, direction);
                    if (guess != null)
                        break;
                }
                if (guess != null)
                    break;
            }

            // If guess is null by now, that means nothing has been found.
            
            if (guess == null)
                guess = new Position(0, 0); // ( This is a placeholder that just guesses 0, 0. )

            return guess;

        }

        // Method to find adjacent spot to a given position, if provided the direction.
        // Returns null if the spot is somehow invalid (off the grid or has already been shot at)
        internal Position GetAdjacent(Position p, char direction)
        {
            // initialize x & y
            int x = p.X;
            int y = p.Y;

            // shift in the desired adjacent direction
            if (direction == 'N')
                y++;
            else if (direction == 'E')
                x++;
            else if (direction == 'S')
                y--;
            else if (direction == 'W')
                x--;
            else
                return null;

            // save result
            Position result = new Position(x, y);

            // return result if valid
            if (IsValid(result))
                return result;

            // return null otherwise
            else
                return null;

        }

        // This method, given a position, checks if it is a valid spot at which to fire.
        // Valid spots do not contain the player's own ships, have not already been shot at, and
        // are on the grid.
        internal bool IsValid(Position p)
        {
            // Check to see if spot contains the AI's ship.
            foreach (Ship s in _ships._ships)
            {
                foreach (Position ShipPosition in s.Positions)
                {
                    if (ShipPosition.X == p.X && ShipPosition.Y == p.Y)
                    {
                        return false;
                    }
                }
            }

            // Check to see if spot has already been shot at
            foreach (List<Position> LoggedPositions in new[] { HitPositions, MissPositions, SankPositions })
            {
                foreach (Position LoggedPosition in LoggedPositions)
                {
                    if (LoggedPosition.X == p.X && LoggedPosition.Y == p.Y)
                    {
                        return false;
                    }
                }
            }

            // Check to see if spot is on the grid
            if (p.X < 0 || p.X >= _gridSize || p.Y < 0 || p.Y >= _gridSize)
            {
                return false;
            }

            // If all the checks have passed, this spot is valid.
            return true;

        }

        // Method to log results throughout the game.
        // BluePlayer will separately keep track of each guess that results in a hit or a miss.
        // It does not track misses, as those require no follow up.
        public void SetAttackResults(List<AttackResult> results)
        {
            foreach (AttackResult r in results)
            {
                if (r.ResultType == AttackResultType.Miss)
                {
                    if (MissPositions.Contains(r.Position) == false)
                        MissPositions.Add(r.Position);
                }
                else if (r.ResultType == AttackResultType.Hit)
                {
                    if (HitPositions.Contains(r.Position) == false)
                        HitPositions.Add(r.Position);
                }
                else if (r.ResultType == AttackResultType.Sank)
                {
                    if (SankPositions.Contains(r.Position) == false)
                        SankPositions.Add(r.Position);
                }
            }
        }
    }
}
