/* RelationshipGraph.cs
 * Ian Christensen
 * Prof. Plantinga
 * Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bingo
{
    /// <summary>
    /// Represents a directed labeled graph with a string name at each node
    /// and a string Label for each edge.
    /// </summary>
    class RelationshipGraph
    {
        /*
         *  This data structure contains a list of nodes (each of which has
         *  an adjacency list) and a dictionary (hash table) for efficiently 
         *  finding nodes by name
         */
        public List<GraphNode> nodes { get; private set; }
        private Dictionary<String, GraphNode> nodeDict;

        // constructor builds empty relationship graph
        public RelationshipGraph()
        {
            nodes = new List<GraphNode>();
            nodeDict = new Dictionary<String,GraphNode>();
        }

        // AddNode creates and adds a new node if there isn't already one by that name
        public void AddNode(string name)
        {
            if (!nodeDict.ContainsKey(name))
            {
                GraphNode n = new GraphNode(name);
                nodes.Add(n);
                nodeDict.Add(name, n);
            }
        }

        // AddEdge adds the edge, creating endpoint nodes if necessary.
        // Edge is added to adjacency list of from edges.
        public void AddEdge(string name1, string name2, string relationship) 
        {
            AddNode(name1);                     // create the node if it doesn't already exist
            GraphNode n1 = nodeDict[name1];     // now fetch a reference to the node
            AddNode(name2);
            GraphNode n2 = nodeDict[name2];
            GraphEdge e = new GraphEdge(n1, n2, relationship);
            n1.AddIncidentEdge(e);
        }

        // Get a node by name using dictionary
        public GraphNode GetNode(string name)
        {
            if (nodeDict.ContainsKey(name))
                return nodeDict[name];
            else
                return null;
        }

        // I used a source online for this method...I didn't understand it
        public List<GraphNode> bingoSearch(String searchFrom, String searchTo)      // breadth first search to find shortest connection
        {                                                                           //
            List<GraphNode> path = new List<GraphNode>();                           // create a list to keep track of the path from the first person to the second
            List<List<GraphNode>> levels = new List<List<GraphNode>>();             // keep track of how many levels of depth there are
            List<GraphNode> currentLevel = new List<GraphNode>();                   // 
            Dictionary<String, Boolean> visited = new Dictionary<String, Boolean>();//
            int depthCount = 0;                                                     // 
            GraphNode From = GetNode(searchFrom);                                   //
            GraphNode To = GetNode(searchTo);                                       //
            if (From == null || To == null || From == To)                           //
                return null;                                                        //
            currentLevel.Add(From);                                                 // add root value
            levels.Add(currentLevel);                                               // add root level
            while (true)                                                            //
            {
                currentLevel = new List<GraphNode>();                               // reset the current level
                foreach (GraphNode n in levels[levels.Count - 1])                   // 
                {                                                                   //
                    foreach (GraphEdge e in n.GetEdges())                           //
                    {                                                               //
                        if (e.To() == searchTo)                                     //
                            goto EndBuild;                                          // this is cool...didn't know you could jump like in assembly code
                        if (!visited.ContainsKey(e.To()))                           // Deduplication
                        {                                                           //
                            currentLevel.Add(e.ToNode());                           //
                            visited.Add(e.To(), true);                              //
                        }                                                           //
                    }                                                               //
                }                                                                   //
                levels.Add(currentLevel);                                           //
            }
        EndBuild:                                                               // figure out path in reverse
            path.Add(To);                                                       //
            depthCount = levels.Count - 1;                                      //
            try                                                                 //
            {                                                                   //
                while (path[path.Count - 1] != From && depthCount >= 0)         //
                {                                                               //
                    foreach (GraphEdge e in path[path.Count - 1].GetEdges())    //
                    {                                                           //
                        if (levels[depthCount].Contains(e.ToNode()))            //
                        {                                                       //
                            path.Add(e.ToNode());                               //
                            goto NextInWhile;                                   //
                        }                                                       //
                    }                                                           //
                    foreach (GraphNode n in levels[depthCount])                 //
                    {                                                           //
                        foreach (GraphEdge e in n.GetEdges())                   //
                        {                                                       //
                            if (e.ToNode() == path[path.Count - 1])             //
                            {                                                   //
                                path.Add(n);                                    //
                                goto NextInWhile;                               //
                            }                                                   //
                        }
                    }
                NextInWhile:                        //
                    depthCount--;                   //
                }                                   //
            }                                       //
            catch (Exception e)                     //
            {                                       //
                Console.WriteLine(e.Message);       //
            }                                       //
            path.Reverse();                         //
            if (path.Count == 1)                    //
                path = null;                        //
            return path;                            //
        }


        // Return a text representation of graph
        public void Dump()
        {
            foreach (GraphNode n in nodes)
            {
                Console.Write(n.ToString());
            }
        }
    }
}
