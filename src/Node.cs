using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    // Set from editor
    public GameObject[] _edges;
    
    public GameObject[] GetEdges()
    {
        return _edges;
    }

    public Node[] GetNodes()
    {
        Node[] nodes = new Node[_edges.Length];
        int i = 0;
        foreach (GameObject go in _edges)
        {
            nodes[i] = go.GetComponent<Node>();
            i++;
        }

        return nodes;
    }

    /*Gets the following node from the current one to the target throught the nearest path*/
    public Node NextNodeTo(Node target)
    {
        Node node = null;
        List<Node> visited_nodes = new List<Node>();//avoid infinite loop with visited nodes

        visited_nodes.Add(this); //adding this one
        Node[] neig = this.GetNodes();

        int distance = -1; //Not found = -1

        foreach (Node n in neig) //For each neighbour, calculate distances and select the best choose
        {
            int d;
            d = n.GetDistanceTo(target, visited_nodes);
            if ((d < distance || distance == -1) && (d != -1)) //If found and is more near than another one, choose that node
            {
                distance = d;
                node = n;
            }
        }

        return node; //Return the next node from this one to the target
    }

    /* For the current node to the target, calculate distance saving all visited nodes */
    public int GetDistanceTo(Node target, List<Node> visited_nodes = null)
    {
        int distance = 0;

        if (visited_nodes == null)
        {
            visited_nodes = new List<Node>();
        }
        visited_nodes.Add(this); //This node is now visited

        if (target != this) //If the same node isn't the target
        {
            Node[] neig = this.GetNodes();
            distance++; //Incrase the distance

            int d_temp;
            int d = -1;
            foreach (Node n in neig) //Get distance recursively for the not visited nodes
            {
                if(visited_nodes.Contains(n))
                {
                    continue;
                }
                else
                {
                    d_temp = n.GetDistanceTo(target, visited_nodes); //Get distance
                    if ((d_temp < d || d == -1) && d_temp != -1)
                    {
                        d = d_temp;
                    }
                }

            }
            if (d != -1) //If found, plus the temp distance with the cumulative
            {
                distance += d;
            }
            else //Else, mark as not found
            {
                distance = -1;
            }
        }
        else
        {
            distance = 0; //If is the target, mark distance as 0
        }
        visited_nodes.Remove(this); //For each recursively iteration finishes, mark now as unvisited the node for search better paths.
        return distance;
    }
}