﻿/************************************************************************************
 *                                                                                  *
 *  Author:        Blue Group: Valerie Stephens, Stephen Peacock, Dustin Poulisse   *
 *  Course:        CS3110 C# Programming                                            *
 *  Assignment:    Module 8 Group Game Assignment                                   *
 *  File:          Destroyer.cs                                                     *
 *  Description:   Multiplayer Battleship Game with AI                              *
 *  Input:         None                                                             *
 *  Output:        Game board with guesses                                          *
 *  Created:       12/11/2020                                                       *
 *                                                                                  *
 ***********************************************************************************/

using System;

namespace Module8_GroupBlue
{
    class Destroyer : Ship
    {
        public Destroyer() : base(3, ConsoleColor.DarkRed, ShipTypes.Destroyer)
        {
        }
    }
}
