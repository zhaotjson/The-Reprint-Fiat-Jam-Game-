using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Microwave : Interactable
{
    // Start is called before the first frame update


    [SerializeField] private GameObject microwaveCanvas;

    [SerializeField] private List<GameObject> inventorySlots =  new List<GameObject>();


    [SerializeField] public GameObject microwaveItem;



    protected override void Start()
    {
        base.Start();



        if (microwaveCanvas != null){
            microwaveCanvas.SetActive(false);
        }



    }

    public void OnItemClicked()
    {   

        Debug.Log("Item clicked:");

        


    }





    public override void Interact()
    {
        Debug.Log("*beep* *beep* *beep*");

        if (microwaveCanvas != null){
            microwaveCanvas.SetActive(true);



            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                Player player = playerObject.GetComponent<Player>();
                if (player != null)
                {
                    player.SetMovementEnabled(false);
                }

                List<GameObject> inventory = player.inventory;

                for (int i = 0; i < 5; i++){
                    Image slotImage = inventorySlots[i].GetComponent<Image>();
                    if (slotImage != null)
                    {
                        slotImage.sprite = null;
                        slotImage.color = new Color(1, 1, 1, 0);

                    }
                }

                for (int i = 0; i < inventory.Count; i++)
                {
                    if (i < 5){

                        Image slotImage = inventorySlots[i].GetComponent<Image>();
                        if (slotImage != null)
                        {
                            SpriteRenderer itemSprite = inventory[i].GetComponent<SpriteRenderer>();

                            if (itemSprite != null)
                            {
                                slotImage.sprite = itemSprite.sprite;
                                slotImage.color = new Color(1, 1, 1, 1);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Item does not have a Sprite Renderer");
                        }
                    }
                }







            }
        }
    }

    




    public void CloseCanvas()
    {
        if (microwaveCanvas != null){

            microwaveCanvas.SetActive(false);

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Player player = playerObject.GetComponent<Player>();
                if (player != null)
                {
                    player.SetMovementEnabled(true);
                }
            }
        }
    }
}
