/************************************************************************************
 *                                                                                  *
 *  Author:        Blue Group: Valerie Stephens, Stephen Peacock, Dustin Poulisse   *
 *  Course:        CS3110 C# Programming                                            *
 *  Assignment:    Module 8 Group Game Assignment                                   *
 *  File:          Program.cs                                                       *
 *  Description:   Multiplayer Battleship Game with AI                              *
 *  Input:         None                                                             *
 *  Output:        Game board with guesses                                          *
 *  Created:       12/11/2020                                                       *
 *                                                                                  *
 ***********************************************************************************/

// Multiplayer Battleship Game with AI - Partial Solution

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module8_GroupBlue
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IPlayer> players = new List<IPlayer>();
            players.Add(new DumbPlayer("Dumb 1"));
            players.Add(new DumbPlayer("Dumb 2"));
            players.Add(new DumbPlayer("Dumb 3"));
            players.Add(new RandomPlayer("Random 1"));
            players.Add(new RandomPlayer("Random 2"));
            players.Add(new RandomPlayer("Random 3"));
            players.Add(new RandomPlayer("Random 4"));
            players.Add(new RandomPlayer("Random 5"));
            players.Add(new BluePlayer("Blue 1"));
            players.Add(new BluePlayer("Blue 2"));
            players.Add(new BluePlayer("Blue 3"));

            

            MultiPlayerBattleShip game = new MultiPlayerBattleShip(players);
            game.Play(PlayMode.Pause);
        }
    }
}
