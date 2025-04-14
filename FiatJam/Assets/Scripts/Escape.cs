using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : Interactable
{
    [SerializeField] private string victorySceneName = "VictoryScene";

    public override void Interact()
    {
        Debug.Log("Escape triggered! Loading victory scene...");
        LoadVictoryScene();
    }

    private void LoadVictoryScene()
    {
        if (!string.IsNullOrEmpty(victorySceneName))
        {
            SceneManager.LoadScene(victorySceneName);
        }
        else
        {
            Debug.LogWarning("Victory scene name is not set.");
        }
    }
}