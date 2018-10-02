/* Floor.c# is a simplified floor function using logs
 * Ian Christensen (igc2)
 * Prof. Harry Plantinga
 * CS-212-A
 * Fall, 2017
 */

// using statments
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// create namespace
namespace FloorCalculations
{
    // create class
    class Program
    {
        // create main
        static void Main(string[] args)
        {
            Console.WriteLine("Enter an integer to find it's floor(lg lg n) value.");
            // enter loop to continue asking for values
            while (true)
            {
                // calculate input value using method FloorCalc
                Console.WriteLine("\nEnter an integer value: ");
                int n = int.Parse(Console.ReadLine());
                int floor = FloorCalc(n);
                // print result
                Console.WriteLine("The floor(lg lg n) value of {0} is {1}", n, floor);
            }
        }
        // create calculation method
        static int FloorCalc(int n)
        {
            // create iterator
            int i = 0;
            // loop through a simplified log function
            while (n >= 1)
            {
                n = n/2;
                i++;
            }
            // return result
            return i;
        }
    }
}
