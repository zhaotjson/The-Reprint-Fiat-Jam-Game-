using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TMP_Text hoverText;

    [TextArea]
    [SerializeField] private string itemDescription;

    [SerializeField] private GameObject itemPrefab;


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
                        DestroyImmediate(gameObject, true);
                    }
                    else
                    {
                        Debug.Log("Inventory is full. Cannot add item.");
                    }

                }
                else
                {
                    Debug.Log("Money not found in inventory.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player object not found.");

        }

    }


}
