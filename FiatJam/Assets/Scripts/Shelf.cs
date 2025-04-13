using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Interactable, IResettable
{
    [SerializeField] private GameObject itemPrefab; 
    [SerializeField] private string dialogueMessage = "Hmmm, there's three dollars here. I should take it."; 

    public int totalTaken = 0;

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
                int itemsAdded = 0;


                while (totalTaken < 3 && itemsAdded < 3)
                {
                    bool added = player.AddToInventory(itemPrefab);
                    if (added)
                    {
                        Debug.Log("Item added to inventory.");
                        totalTaken++;
                        itemsAdded++;
                    }
                    else
                    {
                        Debug.Log("Inventory is full. Cannot add item.");
                        break; 
                    }
                }


                if (itemsAdded > 0)
                {
                    DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                    if (dialogueManager != null)
                    {
                        dialogueManager.ShowDialogue(dialogueMessage);
                    }
                    else
                    {
                        Debug.LogWarning("DialogueManager not found in the scene.");
                    }
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
        totalTaken = 0;
        Debug.Log("Shelf has been reset. Money can be taken again.");
    }
}
