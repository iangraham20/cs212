/* aiPlayer.cs is player class for Mankalah that uses
 * a staged depth first search, MiniMax search and an
 * evaluate function that contains four different hueristics
 * to find the best possible move within the given timelimit
 * 
 * Ian Christensen (igc2)
 * Prof. Plantinga
 * CS-212-A
 * Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Mankalah;

namespace Mankalah
{
    public class AIPlayer : Player
    {
        private Position position;                                                                          // global variable for aiPlayers board position
        Stopwatch timer = new Stopwatch();                                                                  // global variable for keeping track of time
        public AIPlayer(Position pos, int time) : base(pos, "Ian Christensen", time) { position = pos; }    // 
        public override string getImage() { return "grimreaper.jpg"; }
        public override string gloat() { return "I won! HA!"; }                                             // gloat method
        
        // this method uses a staged DFS to choose the next move
        public override int chooseMove(Board board)
        {
            timer.Start();                                      // start the timer
            int currentDepth = 1;                               // keep track of the depth
            int currentResult = 0;                              // keep track of the current depths result
            int previousResult = 0;                             // keep track of the total result from the previous depth
            while(timer.ElapsedMilliseconds < getTimePerMove()) // Continue searching until the timelimit is up
            {
                previousResult = currentResult;                                                                       // iterate the final result
                currentResult = MiniMax(board, currentDepth, double.NegativeInfinity, double.PositiveInfinity).Item1; // search for the best move option until time runs out
                currentDepth++;                                                                                       // iterate the depth
            }
            timer.Stop();           // the search finished so we can stop the timer
            timer.Reset();          // we need to reset the timer for the next move
            return previousResult;  // return the best move available
        }

        // this is a method that evaluates the current board using hueristics to find the best possible move
        public override int evaluate(Board board)
        {
            // declaring the evaulate hueristics
            int captures = 0;
            int extraMoves = 0;
            int finalState = 0;
            int score = 0;
            int upperStones = 0;
            int lowerStones = 0;

            if (board.WhoseMove() == Position.Top)
            {
                // score is increased or decreased by the current score (it's added both times because the score can be negative)
                if (position == Position.Top)
                    score += board.StonesAt(13) - board.StonesAt(6);
                else
                    score += board.StonesAt(6) - board.StonesAt(13);
                
                // extraMoves is increased or decreased by two because the player is being rewarded for both increasing their own score and getting to go again
                for (int i = 12; i >= 7; i--)
                    if (board.StonesAt(i) == 13 - i)
                    {
                        if (position == Position.Top)
                            extraMoves += 2;
                        else
                            extraMoves -= 2;
                    }

                // capture is increased or decreased by the number of stones that are captured if the move is executed
                for (int i = 12; i >= 7; i--)                                                       // check for captures
                    if (board.StonesAt((board.StonesAt(i) + i) % 14) == 0 &&                        // check for an empty bowl
                        ((board.StonesAt(i) + i) % 14) <= 12 &&
                        ((board.StonesAt(i) + i) % 14) >= 7 &&                      // check that it is not landing on the other side
                        board.StonesAt(6 - (((board.StonesAt(i) + i) % 14) - 6)) > 0)   // check that we aren't capturing zero
                    {
                        if (position == Position.Top)
                            captures += board.StonesAt(6 - (((board.StonesAt(i) + i) % 14) - 6));
                        else
                            captures -= board.StonesAt(6 - (((board.StonesAt(i) + i) % 14) - 6));
                    }

                // finalState looks for moves that cause one side of the board to be empty and increases or decreases the score according to how many stones each player would capture
                for (int i = 12; i >= 7; i--)
                    upperStones += board.StonesAt(i);
                for (int i = 5; i >= 0; i--)
                    lowerStones += board.StonesAt(i);
                if (lowerStones == 0 && upperStones > 0)
                    finalState += upperStones;
                if (upperStones == 0 && lowerStones > 0)
                    finalState -= lowerStones;
            }
            else    // These are all the same methods with slight variation that takes into account if the Player is on the bottom
            {
                // score hueristic
                if (position == Position.Bottom)
                    score += board.StonesAt(6) - board.StonesAt(13);
                else
                    score += board.StonesAt(13) - board.StonesAt(6);

                // extraMoves hueristic
                for (int i = 5; i >= 0; i--)
                    if (board.StonesAt(i) == 6 - i)
                    {
                        if (position == Position.Bottom)
                            extraMoves += 2;
                        else
                            extraMoves -= 2;
                    }

                // captures hueristic
                for (int i = 5; i >= 0; i--)
                    if (board.StonesAt((board.StonesAt(i) + i) % 14) == 0 &&
                        ((board.StonesAt(i) + i) % 14) >= 0 &&
                        ((board.StonesAt(i) + i) % 14) <= 5 &&
                        board.StonesAt( 6 + (((board.StonesAt(i) + i) % 14) - 6)) > 0)
                {
                    if (position == Position.Bottom)
                        captures += board.StonesAt(6 + (((board.StonesAt(i) + i) % 14) - 6));
                    else
                        captures -= board.StonesAt(6 + (((board.StonesAt(i) + i) % 14) - 6));
                }

                // finalState hueristic
                for (int i = 12; i >= 7; i--)
                    upperStones += board.StonesAt(i);
                for (int i = 5; i >= 0; i--)
                    lowerStones += board.StonesAt(i);
                if (lowerStones == 0 && upperStones > 0)
                    finalState -= upperStones;
                if (upperStones == 0 && lowerStones > 0)
                    finalState += lowerStones;
            }
            upperStones = lowerStones = 0;  // reset the upper and lower stone values
            return (score + captures + extraMoves + finalState); // output the board quality
        }

        public Tuple<int, double> MiniMax(Board board, int depth, double alpha, double beta)
        {
            double bestResult = 0;
            double result = 0;
            int bestMove = 0;

            if (board.GameOver() || depth == 0)
                return new Tuple<int, double>(0, evaluate(board));

            int start = 0;
            if (board.WhoseMove() == Position.Top)
                start = 7;
            else
                start = 0;

            if (board.WhoseMove() == position)
            {
                bestResult = double.NegativeInfinity;
                for (int move = start; move <= start + 5; move++)
                {
                    if (board.LegalMove(move) && timer.ElapsedMilliseconds < getTimePerMove())
                    {
                        Board tempBoard = new Board(board);             //duplicate board
                        tempBoard.MakeMove(move, false);                //make the move
                        result = MiniMax(tempBoard, depth - 1, alpha, beta).Item2;
                        if (result > bestResult)                        //find its value
                        {
                            bestResult = result;                        //remember if best
                            bestMove = move;
                        }
                        if (bestResult > alpha)
                            alpha = bestResult;
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            else
            {
                bestResult = double.PositiveInfinity;
                for (int move = start; move <= start + 5; move++)
                {
                    if (board.LegalMove(move) && timer.ElapsedMilliseconds < getTimePerMove())
                    {
                        Board tempBoard = new Board(board);             //duplicate board
                        tempBoard.MakeMove(move, false);                //make the move
                        result = MiniMax(tempBoard, depth - 1, alpha, beta).Item2;
                        if (result < bestResult)                        //find its value
                        {
                            bestResult = result;                        //remember if best
                            bestMove = move;
                        }
                        if (beta < bestResult)
                            beta = bestResult;
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return new Tuple<int, double>(bestMove, bestResult);
        }
    }
}
