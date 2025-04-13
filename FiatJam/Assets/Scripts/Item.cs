using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IResettable
{

    [SerializeField] private TMP_Text hoverText;

    [TextArea]
    [SerializeField] private string itemDescription;

    [SerializeField] private GameObject itemPrefab;

    private Vector3 initialPosition;
    private bool isCollected = false;

    private void Start()
    {

        initialPosition = transform.position;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.text = itemDescription;
            Debug.Log($"Hover text set to: {itemDescription}");
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.text = string.Empty;
            Debug.Log($"Hover text set to: {itemDescription}");
        }
    }

    public void OnItemClicked()
    {

        Debug.Log("Item clicked: " + gameObject.name);
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if(playerObject != null){

            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                GameObject money = player.inventory.Find(item => item.name == "money");

                if (money != null)
                {
                    player.inventory.Remove(money);

                    bool added = player.AddToInventory(itemPrefab);
                    if (added)
                    {
                        Debug.Log("Item added to inventory.");
                        hoverText.text = string.Empty;

                        isCollected = true; // Mark the item as collected
                        gameObject.SetActive(false); // Hide the item instead of destroying it
                    }
                    else
                    {
                        Debug.Log("Inventory is full. Cannot add item.");
                    }

                }
                else
                {
                    Debug.Log("Money not found in inventory.");
                    DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                    if (dialogueManager != null)
                    {
                        dialogueManager.ShowDialogue("Damn, I need a dollar.");
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

        transform.position = initialPosition;
        isCollected = false;
        gameObject.SetActive(true);
        Debug.Log($"Item {gameObject.name} has been reset.");
    }


}
