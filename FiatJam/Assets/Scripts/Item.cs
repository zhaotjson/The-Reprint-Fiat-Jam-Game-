using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TMP_Text hoverText;
    [SerializeField] private string itemDescription;


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


}
