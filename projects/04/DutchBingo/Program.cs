/* Program.cs
 * Ian Christensen
 * Prof. Plantinga
 * Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Bingo
{
    class Program
    {
        private static RelationshipGraph rg;

        // Read RelationshipGraph whose filename is passed in as a parameter.
        // Build a RelationshipGraph in RelationshipGraph rg
        private static void ReadRelationshipGraph(string filename)
        {
            rg = new RelationshipGraph();                            // create a new RelationshipGraph object
            string name = "";                                        // name of person currently being read
            int numPeople = 0;                                       // counter of how many people are read
            string[] values;                                         // array of people in file
            Console.Write("Reading file " + filename + "\n");        // prompt user for the file
            try                                                      // begin processing contents of file
            {
                string input = System.IO.File.ReadAllText(filename); // read file into a variable
                input = input.Replace("\r", ";");                    // replace carriage returns with semicolons
                input = input.Replace("\n", ";");                    // replace new lines with semicolons
                string[] inputItems = Regex.Split(input, @";\s*");   // parse out the relationships with semicolons
                foreach (string item in inputItems)                  // begin processing each item in the new array
                {
                    if (item.Length > 2)                             // skip any empty relationships
                    {                                                // begin processing the current item
                        values = Regex.Split(item, @"\s*:\s*");      // parse out relationship from the name
                        if (values[0] == "name")                     // check to see if the current item is a new person
                        {                                            // begin processing the current name
                            name = values[1];                        // remember name for future relationships
                            rg.AddNode(name);                        // create the node in the relationship graph
                            numPeople++;                             // increment the number of people in the graph
                        }
                        else
                        {                                                             // begin processing the relationships of the current name
                            rg.AddEdge(name, values[1], values[0]);                   // add the given relationship of the current name to the graph
                            if (values[0] == "spouse" || values[0] == "friend") // check for any symmetric relationships (i.e. friends or spouse)
                                rg.AddEdge(values[1], name, values[0]);               // handle the symmetric relationship by adding the relationship to graph
                            else if (values[0] == "parent")                        // check for any parent relationships (i.e. does name have a parent)
                                rg.AddEdge(values[1], name, "child");              // handle the parent relationship by adding the child relationship to the graph
                            else if (values[0] == "child")                         // check for children relationships of the current name (i.e. does name have children)
                                rg.AddEdge(values[1], name, "parent");             // handle the child relationship by adding a parent relationship to the graph
                        }                                                             // finished processing this item return to the beginning of the foreach loop
                    }
                }
            }
            catch (Exception e)                                                          // if there is an error throw an exception
            {                                                                            // begin handling the exception
                Console.Write("Unable to read file {0}: {1}\n", filename, e.ToString()); // write the error message to the console
            }                                                                            // proceed if there were no exceptions while reading
            Console.WriteLine(numPeople + " people read");                               // write the total number of people in the graph
        }                                                                                // the main function of Program.cs has finished executing

        private static void ShowPerson(string name) // Show the relationships a person is involved in
        {
            GraphNode n = rg.GetNode(name);                 // create a graphnode variable for name
            if (n != null)                                  // check that the name is valid
                Console.Write(n.ToString());                // write the name and its relationships to the console
            else                                            // if the name wasn't valid
                Console.WriteLine("{0} not found", name);   // write that the name wasn't valid to the console
        }

        private static void ShowFriends(string name) // Show a person's friends
        {
            GraphNode n = rg.GetNode(name);                             // create a graphnode variable for name
            if (n != null)                                              // check that the name is valid
            {                                                           // begin processing name
                Console.Write("{0}'s friends: ", name);                 // write the initial statement to the console
                List<GraphEdge> friendEdges = n.GetEdges("friend");  // check all of name's relationships and create a list of name's friends
                foreach (GraphEdge e in friendEdges)                    // iterate through name's list of friends
                    Console.Write("{0} ", e.To());                      // write the current friend's name to the console
                Console.WriteLine();                                    // add a newline character after the last friend
            }                                                           // if the name the method is finished
            else                                                        // if the name wasn't valid
                Console.WriteLine("{0} not found", name);               // write that the name wasn't valid to the console
        }

        private static void ShowOrphans()
        {
            List<GraphNode> n = rg.nodes;                   // create a list of all the graphnodes
            foreach (GraphNode gn in n)
            {                                               // iterate through each node in the list
                if (gn.GetEdges("parent").Count == 0)       // check the current node for a parent relationship
                    Console.Write("{0} ", gn.Name);         // write the current node to the console
            }                                               // once all the nodes have been checked exit the loop
        }                                                   // finish execution

        private static void ShowDescendents(string name)
        {
            GraphNode root = rg.GetNode(name);                  // create node for name
            List<GraphNode> current = new List<GraphNode>();    // create a list for the current generation of names
            List<GraphNode> next = new List<GraphNode>();       // create a list for the next generation of names
            List<String> descendents = new List<String>();      // create a list of all the current descendents as strings instead of graphnodes
            Console.WriteLine("Descendents of {0}:", root.Name);// begin printing the outcome
            current.Add(root);                                  // initialize the current generation as the first name
            int generation = 0;                                 // keep track of the generation starting with name as the 0th gen
            while (current.Count > 0)                           // continue looping until there are no descendents
            {
                if (generation == 1)                    // if this is the first generation than 
                    Console.Write("Children:\n");       // write that they are the children of name
                else if (generation > 1)                // else add one "great" before grandchild for each generation
                {                                       // starting with zero instances of "great"
                    for (int i = generation; i > 2; i--)// begin at the current generation and decrement keeping in mind the exception of the first and second generation
                    {
                        if (i == generation)
                            Console.Write("Great ");
                        else
                            Console.Write("great ");    // output each "great" neccessary
                    }
                    Console.WriteLine("Grandchildren:");// finally write the grandchildren part
                }
                descendents.ForEach(item => Console.WriteLine("{0}", item));    // output the generation's names
                generation++;                                                   // increment to the next generation
                descendents = new List<String>();                               // reset the list containing the current descendents
                foreach(GraphNode gn in current)                                // go through each node in the list of current descendents
                    foreach(GraphEdge ge in gn.GetEdges("child"))               // go through each of the relationships of those nodes
                    {                                                           // 
                        next.Add(ge.ToNode());                                  // begin adding each child of the current descendents to the next descendents list
                        descendents.Add(ge.To());                               // add the current descendents to the string list for outputting later
                    }                                                           // 
                current = next;                                                 // increment the node pointer (kind of...) by setting current to next
                next = new List<GraphNode>();                                   // reset next to an empty list so that we can set it to the new current's descendents later on
            }
        }

        private static void ShowCousins(string name, int n, int k)
        {
            n += 1;
            Dictionary<String, bool> visited = new Dictionary<string, bool>();          // create a dictionary to keep track of which nodes have been visited
            Dictionary<GraphNode, bool> top = new Dictionary<GraphNode, bool>();        // create a dictionary 
            Dictionary<GraphNode, bool> end = new Dictionary<GraphNode, bool>();        // create a dictionary 
            if (rg.GetNode(name) == null)                                               // check for cases where the name doesn't exist
            {                                                                           // 
                Console.WriteLine("person does not exist");                             // output the error
                return;                                                                 // return nothing
            }                                                                           // 
            top.Add(rg.GetNode(name), true);                                            // add the root (name) to the top dict
            visited.Add(name, true);                                                    // add the root (name) to the visited dict
            for(int i = 0; i < n + k; i++)                                              // go up n generations
            {                                                                           // 
                Dictionary<GraphNode, bool> level = new Dictionary<GraphNode, bool>();  // create a dictionary to keep track of 
                foreach(GraphNode gn in top.Keys)                                       // check each node in the top dict
                    foreach(GraphEdge ge in gn.GetEdges("parent"))                      // check each edge of each of those nodes for a parent relation
                    {                                                                   // 
                        if (!level.ContainsKey(ge.ToNode()))                            // check to see if it is already exists
                            level.Add(ge.ToNode(), true);                               // add it otherwise
                        if (i < n - 1)                                                  // unless this is the last case than
                            visited.Add(ge.To(), true);                                 // add the current node to the visited list
                    }                                                                   // 
                if (i == n - 1 && k != 0)                                               // if this is the last case and the cousin is removed to a kth degree
                        end = level;                                                    // set the end value to the current depth
                top = level;                                                            // 
            }

            for (int i = 0; i < n; i++)                                                 // 
            {                                                                           // 
                Dictionary<GraphNode, bool> depth = new Dictionary<GraphNode, bool>();  // 
                foreach (GraphNode gn in top.Keys)                                      // 
                {                                                                       // 
                    if (visited.ContainsKey(gn.Name))                                   // 
                        continue;                                                       // 
                    if (!visited.ContainsKey(gn.Name))                                  // 
                        visited.Add(gn.Name, true);                                     // 
                    foreach (GraphEdge e in gn.GetEdges("child"))                       //
                    {                                                                   // 
                        if (!depth.ContainsKey(e.ToNode()))                             // 
                            depth.Add(e.ToNode(), true);                                // 
                    }                                                                   // 
                }                                                                       // 
                top = depth;                                                            // 
            }

            for (int i = 0; i < n + k; i++)                                             //
            {                                                                           //
                Dictionary<GraphNode, bool> depth = new Dictionary<GraphNode, bool>();  //
                foreach (GraphNode gn in end.Keys)                                      //
                {                                                                       //
                    if (visited.ContainsKey(gn.Name))                                   //
                        continue;                                                       //
                    if (!visited.ContainsKey(gn.Name))                                  //
                        visited.Add(gn.Name, true);                                     //
                    foreach (GraphEdge e in gn.GetEdges("child"))                       //
                    {                                                                   //
                        if (!depth.ContainsKey(e.ToNode()))                             //
                            depth.Add(e.ToNode(), true);                                //
                    }                                                                   //
                }                                                                       //
                end = depth;                                                            //
            }

            List<String> cousins = new List<String>();                  // create a list to store all of the values in
            foreach (GraphNode gn in top.Keys)                          // go through each value in top
                if (!visited.ContainsKey(gn.Name))                      // if it was a visited node that 
                    cousins.Add(gn.Name);                               // add the value to the list of cousins
            foreach (GraphNode gn in end.Keys)                          // go through each valuein end
                if (!visited.ContainsKey(gn.Name))                      // if it was a visited node that
                    cousins.Add(gn.Name);                               // add the value to the list of cousins
            cousins.ForEach(item => Console.WriteLine("{0}", item));    // output each of the cousins names
        }

        // didn't understand this method...used a source online for bingo
        private static void PlayBingo(string F, string T)
        {
            List<GraphNode> path = rg.bingoSearch(F, T);
            if (path != null)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    foreach (GraphEdge e in path[i - 1].GetEdges())
                    {
                        if (e.ToNode() == path[i])
                            Console.WriteLine(path[i - 1].Name + " is a " + e.Label + " of " + path[i].Name);
                    }
                }
            }
        }

        // accept, parse, and execute user commands
        private static void CommandLoop()
        {
            string command = "";
            string[] commandWords;
            Console.Write("Welcome to Ian Christensen's Dutch Bingo Parlor!\n");

            while (command != "exit")
            {
                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();
                commandWords = Regex.Split(command, @"\s+");        // split input into array of words
                command = commandWords[0];
                        if (command == "exit") ;                             // do nothing

                        // read a relationship graph from a file
                        else if (command == "read" && commandWords.Length > 1)
                            ReadRelationshipGraph(commandWords[1]);

                        // show information for one person
                        else if (command == "show" && commandWords.Length > 1)
                            ShowPerson(commandWords[1]);

                        else if (command == "friends" && commandWords.Length > 1)
                            ShowFriends(commandWords[1]);

                        else if (command == "orphans")
                            ShowOrphans();

                        else if (command == "descendents" && commandWords.Length > 1)
                            ShowDescendents(commandWords[1]);

                        else if (command == "cousins" && commandWords.Length > 1)
                            ShowCousins(commandWords[1], int.Parse(commandWords[2]), int.Parse(commandWords[3]));

                        else if (command == "bingo" && commandWords.Length > 1)
                            PlayBingo(commandWords[1], commandWords[2]);

                        // dump command prints out the graph
                        else if (command == "dump")
                            rg.Dump();

                        // illegal command
                        else
                            Console.Write("\nLegal commands:\nread [filename]\ndump\nshow [personname]\nfriends [personname]\norphans\ndescendents [personname]\ncousins [personname][nth degree][kth removed]\nexit\n");
            }
        }

        static void Main(string[] args)
        {
            CommandLoop();
        }
    }
}
