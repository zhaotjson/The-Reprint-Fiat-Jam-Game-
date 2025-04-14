using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableLadder : Interactable, IResettable
{
    [SerializeField] private GameObject ladderObject;
    [SerializeField] private Vector3 playerTargetPosition;

    private bool isLadderPlaced = false;

    protected override void Start()
    {
        base.Start();
        if (ladderObject != null)
        {
            ladderObject.SetActive(false); 
        }
    }

    public override void Interact()
    {
        Debug.Log("Interact called on PlaceableLadder.");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                if (!isLadderPlaced)
                {
                    Debug.Log("Ladder is not placed yet. Checking inventory...");
                    GameObject ladderItem = player.inventory.Find(item => item != null && (item.name == "ladderItem" || item.name == "ladderItem(Clone)"));

                    if (ladderItem != null)
                    {
                        Debug.Log("Ladder item found in inventory. Placing ladder...");
                        player.inventory.Remove(ladderItem);
                        player.UpdateInventoryUI();

                        if (ladderObject != null)
                        {
                            ladderObject.SetActive(true);
                        }

                        isLadderPlaced = true;
                        player.speed = 5f;
                        Debug.Log("Ladder placed successfully!");
                    }
                    else
                    {
                        Debug.LogWarning("Player does not have a ladder in their inventory.");
                    }
                }
                else
                {
                    Debug.Log("Ladder is already placed. Moving player to target position...");
                    player.transform.position = playerTargetPosition;
                    Debug.Log("Player moved to the ladder's target position.");
                }
            }
            else
            {
                Debug.LogWarning("Player component not found on player object.");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject playerObject = collision.gameObject;
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {

                bool hasLadder = player.inventory.Exists(item => item != null && item.name == "ladderItem");
                if (EIndicator != null)
                {
                    EIndicator.enabled = isLadderPlaced || hasLadder;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            if (EIndicator != null)
            {
                EIndicator.enabled = false;
            }
        }
    }


    public void ResetObject()
    {
        Debug.Log("Resetting PlaceableLadder.");
        isLadderPlaced = false;

        if (ladderObject != null)
        {
            ladderObject.SetActive(false);
        }
    }
}