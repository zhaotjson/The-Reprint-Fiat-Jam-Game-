using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Interactable
{

    [SerializeField] private GameObject itemPrefab;


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

                if (totalTaken < 3){
                    bool added = player.AddToInventory(itemPrefab);
                    if (added)
                    {
                        Debug.Log("Item added to inventory.");
                    }
                    else
                    {
                        Debug.Log("Inventory is full. Cannot add item.");
                    }
                    totalTaken++;


                }


            }
        }

        else
        {
            Debug.LogWarning("Player object not found.");
        }

    }
}
