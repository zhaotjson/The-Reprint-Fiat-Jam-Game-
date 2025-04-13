using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class microwaveItem : MonoBehaviour,  IPointerClickHandler
{

    [SerializeField] private GameObject microwave;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Microwave clicked: " + gameObject.name);
        OnItemClicked();
    }


    public void OnItemClicked()
    {   

        Debug.Log("Item clicked:" + gameObject.name);

        


    }



}
