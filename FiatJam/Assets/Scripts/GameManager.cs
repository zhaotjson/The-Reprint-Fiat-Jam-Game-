using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Add this line for LINQ support

public class GameManager : MonoBehaviour
{
    private List<IResettable> resettableObjects;

    void Start()
    {
        resettableObjects = new List<IResettable>(FindObjectsOfType<MonoBehaviour>().OfType<IResettable>());
    }

    public void ResetGame()
    {
        foreach (var resettable in resettableObjects)
        {
            resettable.ResetObject();
        }
    }
}