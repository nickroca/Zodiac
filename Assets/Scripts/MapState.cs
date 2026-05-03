using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapState : MonoBehaviour
{
    public static MapState Instance;
    public List<int> unlockedNodes = new List<int>();
    public int currentNode = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
