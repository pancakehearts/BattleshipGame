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

namespace Module8_GroupBlue
{
    public class Position
    {
        public int X;
        public int Y;
        public bool Hit;
        public Position(int x, int y)
        {
            X = x;
            Y = y;
            Hit = false;
        }
    }
}
