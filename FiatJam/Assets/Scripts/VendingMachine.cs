using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : Interactable
{

    [SerializeField] private GameObject vendingMachineCanvas;


    protected override void Start()
    {
        base.Start();

        if (vendingMachineCanvas != null)
        {
            vendingMachineCanvas.SetActive(false);
        }
    }

    public override void Interact()
    {
        Debug.Log("*beep* *beep* *beep*");
        if (vendingMachineCanvas != null)
        {
            vendingMachineCanvas.SetActive(!vendingMachineCanvas.activeSelf);
        }


    }


    public void CloseCanvas()
    {
        if (vendingMachineCanvas != null)
        {
            vendingMachineCanvas.SetActive(false);
        }
    }
}

