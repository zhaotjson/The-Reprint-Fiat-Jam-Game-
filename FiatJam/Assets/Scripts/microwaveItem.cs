using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class microwaveItem : MonoBehaviour
{

    [SerializeField] private GameObject microwave;


    public void OnItemClicked()
    {   

        Debug.Log("Item clicked:" + gameObject.name);

        


    }


    private void OnMouseDown()
    {
        OnItemClicked();
    }

}
