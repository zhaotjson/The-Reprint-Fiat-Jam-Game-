using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : Interactable, IResettable
{

    [SerializeField] private GameObject vendingMachineCanvas;

    public int numCarrots = 0;

    public int numCarrotsPrevious = -1;

    [SerializeField] private List<GameObject> carrotPrefabs = new List<GameObject>();




    protected override void Start()
    {

        base.Start();

        numCarrots = Random.Range(0, 10);

        if (vendingMachineCanvas != null)
        {
            vendingMachineCanvas.SetActive(false);
        }

        for (int i = 0; i < carrotPrefabs.Count; i++)
        {
            carrotPrefabs[i].SetActive(false);
        }



        DisplayCarrots();

    }

    public override void Interact()
    {
        Debug.Log("*beep* *beep* *beep*");
        if (vendingMachineCanvas != null)
        {
            vendingMachineCanvas.SetActive(true);


            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Player player = playerObject.GetComponent<Player>();
                if (player != null)
                {
                    player.SetMovementEnabled(false);
                }
            }

        }


    }


    public void CloseCanvas()
    {
        if (vendingMachineCanvas != null)
        {
            vendingMachineCanvas.SetActive(false);


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


    public void DisplayCarrots()
    {
        for (int i = 0; i < carrotPrefabs.Count; i++)
        {
            carrotPrefabs[i].SetActive(false); // Ensure all carrots are hidden first
        }

        for (int i = 0; i < numCarrots; i++)
        {
            carrotPrefabs[i].SetActive(true);
        }
    }



    public void ResetObject()
    {

        numCarrotsPrevious = numCarrots;


        numCarrots = Random.Range(0, 10);


        DisplayCarrots();
        CloseCanvas();
    }
}

