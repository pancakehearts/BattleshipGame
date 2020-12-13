/************************************************************************************
 *                                                                                  *
 *  Author:        Blue Group: Valerie Stephens, Stephen Peacock, Dustin Poulisse   *
 *  Course:        CS3110 C# Programming                                            *
 *  Assignment:    Module 8 Group Game Assignment                                   *
 *  File:          BluePlayer.cs                                                    *
 *  Description:   Multiplayer Battleship Game with AI                              *
 *  Input:         None                                                             *
 *  Output:        Game board with guesses                                          *
 *  Created:       12/11/2020                                                       *
 *                                                                                  *
 ***********************************************************************************/

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
        private Ships _ships; // Player's ships
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

        // Property that stores the positions of the player's fleet.
        private List<Position> ShipPositions
        {
            get
            {
                ShipPositions = new List<Position>();
                foreach (Ship ship in _ships._ships)
                {
                    foreach (Position shipPosition in ship.Positions)
                    {
                        ShipPositions.Add(shipPosition);
                    }
                }
                return ShipPositions;
            }

            set
            {
                ShipPositions = value;
            }
        }

        /// <summary>
        /// Logic to start new game
        /// </summary>
        /// <param name="playerIndex">Player's index in turn order.</param>
        /// <param name="gridSize">Size of the grid.</param>
        /// <param name="ships">Ships to be placed on grid.</param>
        public void StartNewGame(int playerIndex, int gridSize, Ships ships)
        {
            //TODO: Intelligently place Ships.
            //TODO: Reset ships between games.


            _gridSize = gridSize;
            _index = playerIndex;


            
            // Currently: this borrows from RandomPlayer, which just puts the ships in the grid in Random columns
            

            var availableRows = new List<int>();
            for (int i = 0; i < gridSize; i++)
            {
                availableRows.Add(i);
            }

            _ships = ships;
            foreach (var ship in ships._ships)
            {
                // Pick an open y from the remaining rows
                var y = availableRows[Random.Next(availableRows.Count)];
                availableRows.Remove(y); //Make sure we can't pick it again

                // Pick a Y that fits
                var x = Random.Next(gridSize - ship.Length);
                ship.Place(new Position(x, y), Direction.Horizontal);
            }
        }

        /// <summary>
        /// Method to intelligently find best spot to attack.
        /// </summary>
        /// <returns>Position to be attacked.</returns>
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
                //    guess = new Position(0, 0); // ( This is a placeholder that just guesses 0, 0. )
                guess = GetRandomAttackPosition();


            return guess;

        }

        /// <summary>
        /// Chooses a random open square to attack on the grid.
        /// </summary>
        /// <returns>Random position to attack.</returns>
        private Position GetRandomAttackPosition()
        {
            Position position = new Position(0, 0);
            do
            {
                Random random = new Random();
                position.X = random.Next(0, _gridSize);
                position.Y = random.Next(0, _gridSize);
            } while (!IsValid(position));

            return position;
        }


        /// <summary>
        /// Method to find adjacent spot to a given position, if provided the direction.
        /// </summary>
        /// <param name="p">Initial position being checked.</param>
        /// <param name="direction">Direction of adjacent square in relation to
        /// initial position.</param>
        /// <returns>Returns null if the spot is somehow invalid
        /// (off the grid or has already been shot at).</returns>
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

        /// <summary>
        /// This method, given a position, checks if it is a valid spot at which to fire.
        /// Valid spots do not contain the player's own ships, have not already been shot at, and
        /// are on the grid.
        /// </summary>
        /// <param name="p">Position to be checked.</param>
        /// <returns>True if position is valid and open to attack. Otherwise, false.</returns>
        internal bool IsValid(Position p)
        {
            // Check to see if spot is on the grid
            if (p.X < 0 || p.X >= _gridSize || p.Y < 0 || p.Y >= _gridSize)
            {
                return false;
            }

            // Checks to see if position has already been attacked.
            if (HasBeenHit(p))
            {
                return false;
            }

            // Checks to see if position contains player ship
            if (ContainsMyShip(p))
            {
                if (OpenPositionsRemain())
                {
                    return false;
                }
                
            }

            // If all the checks have passed, this spot is valid.
            return true;

        }

        /// <summary>
        /// Check to see if spot contains the AI's ship.
        /// </summary>
        /// <param name="p">Position on board to be checked.</param>
        /// <returns>True if the position overlaps with one of the AI's
        /// ships. Otherwise, false.</returns>
        internal bool ContainsMyShip(Position position)
        {
            bool isMyShip = false;
            foreach (Ship ship in _ships._ships)
            {
                foreach (Position ShipPosition in ship.Positions)
                {
                    if (ShipPosition.X == position.X && ShipPosition.Y == position.Y)
                    {
                        isMyShip = true;
                    }
                }
            }

            return isMyShip;
        }

        /// <summary>
        /// Check to see if spot has already been attacked.
        /// </summary>
        /// <param name="position">Position to be checked.</param>
        /// <returns>True if position has been attacked already. Otherwise, false.</returns>
        internal bool HasBeenHit(Position position)
        {
            bool isPositionHit = false;
            foreach (List<Position> LoggedPositions in new[] { HitPositions, MissPositions, SankPositions })
            {
                foreach (Position LoggedPosition in LoggedPositions)
                {
                    if (LoggedPosition.X == position.X && LoggedPosition.Y == position.Y)
                    {
                        isPositionHit = true;
                    }
                }
            }

            return isPositionHit;
        }

        /// <summary>
        /// Checks board if spaces remain that have not been hit and do not
        /// contain our ships.
        /// </summary>
        /// <returns>True if open spaces remain that have not been attacked
        /// and do not contain the AI's ships. Otherwise, false.</returns>
        internal bool OpenPositionsRemain()
        {
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    Position position = new Position(x, y);
                    if (!HasBeenHit(position) && !ContainsMyShip(position))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Method to log results throughout the game.
        /// BluePlayer will separately keep track of each guess that results in a hit or a miss.
        /// It does not track misses, as those require no follow up.
        /// </summary>
        /// <param name="results">Results of the attack on all player boards
        /// from the previous round.</param>
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
