﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mankalah
{
    /********************************************************************/
    /* This class creates two Players and runs a pair of Kalah games,
    /* one with each player starting. The match score is reported.
    /********************************************************************/

    public class KalahMatch
    {
        private static int timeLimit = 1000;						                // turn time in msec
        private static Player pTop = new HumanPlayer(Position.Top, timeLimit);	// TOP player (MAX)
        private static Player pBot = new AIPlayer(Position.Bottom, timeLimit);	// BOTTOM player	
        private static Board b;			                                // playing surface
        private static int move;

        /* 
         * Play one Kalah game with the two given players, with firstPlayer
         * starting. This function returns TOP's score.
         */
        public static int playGame(Player pTop, Player pBot, Position firstPlayer)
        {
            b = new Board(firstPlayer);

            if (firstPlayer == Position.Top)
                Console.WriteLine("Player " + pTop.getName() + " starts.");
            else
                Console.WriteLine("Player " + pBot.getName() + " starts.");

            b.Display();

            while (!b.GameOver())
            {
                Console.WriteLine();
                if (b.WhoseMove() == Position.Top)
                {
                    move = pTop.chooseMove(b);
                    Console.WriteLine(pTop.getName() + " chooses move " + move);
                }
                else
                {
                    move = pBot.chooseMove(b);
                    Console.WriteLine(pBot.getName() + " chooses move " + move);
                }

                b.MakeMove(move, true);		// last parameter says to be chatty
                b.Display();

                if (b.GameOver())
                {
                    if (b.Winner() == Position.Top)
                        Console.WriteLine("Player " + pTop.getName() +
                        " (TOP) wins " + b.ScoreTop() + " to " + b.ScoreBot());
                    else if (b.Winner() == Position.Bottom)
                        Console.WriteLine("Player " + pBot.getName() +
                        " (BOTTOM) wins " + b.ScoreBot() + " to " + b.ScoreTop());
                    else Console.WriteLine("A tie!");
                }
                else
                    if (b.WhoseMove() == Position.Top)
                    Console.WriteLine(pTop.getName() + " to move.");
                else
                    Console.WriteLine(pBot.getName() + " to move.");
            }
            return b.ScoreTop();
        }

        public static void Main(String[] args)
        {
            int topScore;

            Console.WriteLine("\n================ Game 1 ================");
            topScore = playGame(pTop, pBot, Position.Bottom);

            Console.WriteLine("\n================ Game 2 ================");
            topScore += playGame(pTop, pBot, Position.Top);

            Console.WriteLine("\n========================================");
            Console.Write("Match result: ");

            int botScore = 96 - topScore;
            if (topScore > 48)
            {
                Console.WriteLine(pTop.getName() + " wins " + topScore + " to " + botScore);
                pTop.gloat();
            }
            else if (botScore > 48)
            {
                Console.WriteLine(pBot.getName() + " wins " + botScore + " to " + topScore);
                pBot.gloat();
            }
            else
                Console.WriteLine("Match was a tie, 48-48!");

            Console.Read();
        }
    }
}