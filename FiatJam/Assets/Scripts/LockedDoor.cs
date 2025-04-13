using System.Collections;
using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private string keycardName = "keycard";
    [SerializeField] private string lockedMessage = "Can't seem to open it";

    public override void Interact()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {

                GameObject keycard = player.inventory.Find(item => item.name == keycardName);

                if (keycard != null)
                {


                    Debug.Log("Keycard used. Door unlocked.");
                    base.Interact(); 
                }
                else
                {

                    Debug.Log("Keycard not found. Door remains locked.");
                    DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                    if (dialogueManager != null)
                    {
                        dialogueManager.ShowDialogue(lockedMessage);
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
}