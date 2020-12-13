/************************************************************************************
 *                                                                                  *
 *  Author:        Blue Group: Valerie Stephens, Stephen Peacock, Dustin Poulisse   *
 *  Course:        CS3110 C# Programming                                            *
 *  Assignment:    Module 8 Group Game Assignment                                   *
 *  File:          AttackResult.cs                                                  *
 *  Description:   Multiplayer Battleship Game with AI                              *
 *  Input:         None                                                             *
 *  Output:        Game board with guesses                                          *
 *  Created:       12/11/2020                                                       *
 *                                                                                  *
 ***********************************************************************************/

using System;

namespace Module8_GroupBlue
{
    public struct AttackResult
    {
        public int PlayerIndex;
        public Position Position;
        public AttackResultType ResultType;
        public ShipTypes SunkShip; //Filled in if ResultType is Sunk

        public AttackResult(int playerIndex, Position position, AttackResultType attackResultType= AttackResultType.Miss, ShipTypes sunkShip = ShipTypes.None)
        {
            PlayerIndex = playerIndex;
            Position = position;
            ResultType = attackResultType;
            SunkShip = sunkShip;
        }
    }
}
