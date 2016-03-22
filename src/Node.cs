using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    // Set from editor
    public GameObject[] _edges;
    
    public GameObject[] GetEdges()
    {
        return _edges;
    }
}
