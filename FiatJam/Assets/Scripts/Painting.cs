using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : Interactable, IResettable
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private string dialogueMessage = "Oh, there's a keycard behind this painting.";

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

                if (totalTaken < 1)
                {
                    bool added = player.AddToInventory(itemPrefab);
                    if (added)
                    {
                        Debug.Log("Keycard added to inventory.");
                        totalTaken++;


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
                    else
                    {
                        Debug.Log("Inventory is full. Cannot add keycard.");
                    }
                }
                else
                {
                    Debug.Log("Keycard has already been taken.");
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
        // Reset totalTaken to 0
        totalTaken = 0;

        Debug.Log("Painting reset: totalTaken set to 0.");
    }
}