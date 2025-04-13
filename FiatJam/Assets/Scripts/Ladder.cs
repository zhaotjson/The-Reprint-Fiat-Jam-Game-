using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable, IResettable
{
    [SerializeField] private GameObject ladderPrefab;
    private bool isPickedUp = false;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {


        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                if (!player.isBuffed)
                {
                    DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                    if (dialogueManager != null)
                    {
                        dialogueManager.ShowDialogue("This ladder is too heavy, probably need some protein.");
                    }

                    return;
                }

                // Player is buffed, allow picking up the ladder
                bool added = player.AddToInventory(ladderPrefab);
                if (added)
                {
                    isPickedUp = true; // Mark the ladder as picked up
                    gameObject.SetActive(false); // Hide the ladder from the scene

                    DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                    if (dialogueManager != null)
                    {
                        dialogueManager.ShowDialogue("Dam, it's a bit heavy.");
                    }
                    player.speed = 2f;

                }
                else
                {
                    Debug.Log("Inventory is full. Cannot pick up the ladder.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player object not found.");
        }
    }

    public void ResetObject()
    {
        isPickedUp = false;
        gameObject.SetActive(true); // Make the ladder visible again
        Debug.Log("Ladder has been reset and is available to pick up again.");
    }
}