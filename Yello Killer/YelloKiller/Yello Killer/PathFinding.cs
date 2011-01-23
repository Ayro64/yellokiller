using System;
using System.Collections.Generic;
using System.Text;

namespace Yellokiller.Yello_Killer
{
    class PathFinding
    {
        public static List<Case> CalculatePathWithAStar(Carte carte, Case startCase, Case endCase)
        {
            List<Case> result = new List<Case>();
            NodeList<Node> openList = new NodeList<Node>();
            NodeList<Node> closedList = new NodeList<Node>();
            List<Node> possibleNodes;
            int possibleNodesCount;
            Node startNode = new Node(startCase, null, endCase);
            openList.Add(startNode);
            

            while (openList.Count > 0)
            {
                Node current = openList[0];
                openList.RemoveAt(0);
                closedList.Add(current);

                if (current.Case == endCase)
                {
                    List<Case> sol = new List<Case>();
                    while (current.Parent != null)
                    {
                        sol.Add(current.Case);
                        current = current.Parent;
                    }
                    return sol;
                }

                possibleNodes = current.GetPossibleNode(carte, endCase);
                possibleNodesCount = possibleNodes.Count;

                for (int i = 0; i < possibleNodesCount; i++)
                {
                    if (!closedList.Contains(possibleNodes[i]))
                    {
                        if (openList.Contains(possibleNodes[i]))
                        {
                            if (possibleNodes[i].EstimatedMovement < openList[possibleNodes[i]].EstimatedMovement)
                                openList[possibleNodes[i]].Parent = current;
                        }
                        else
                            openList.DichotomicInsertion(possibleNodes[i]);
                    }
                }
            }
            return null;
        }
    }
}
