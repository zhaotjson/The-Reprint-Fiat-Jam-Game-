using System.Collections;
using UnityEngine;
using TMPro;

public class ElevatorDoor : Door, IResettable
{
    [SerializeField] private GameObject numPadCanvas;
    [SerializeField] private TextMeshProUGUI inputDisplay;

    [SerializeField] private GameObject vendingMachine;

    public int correctCode;

    private bool isLocked = false;
    private string userInput = "";


    private VendingMachine vendingMachineScript;

    protected override void Start()
    {
        base.Start();



        if (vendingMachine != null)
        {
            vendingMachineScript = vendingMachine.GetComponent<VendingMachine>();
            if (vendingMachineScript != null)
            {
                correctCode = vendingMachineScript.numCarrotsPrevious;
                Debug.Log($"[ElevatorDoor] Initial correct code set to: {correctCode}");
            }
            else
            {
                Debug.LogWarning("[ElevatorDoor] VendingMachine script not found on the assigned GameObject.");
            }
        }
        

        if (numPadCanvas != null)
        {
            numPadCanvas.SetActive(false); 
        }

        if (inputDisplay != null)
        {
            inputDisplay.text = ""; 
        }
    }

    public override void Interact()
    {
        if (isLocked)
        {


            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue("The elevator is locked. Please wait a moment.");
            }


            return;
        }

        if (numPadCanvas != null)
        {
            numPadCanvas.SetActive(true);
        }
    }

    public void OnNumberButtonPressed(string number)
    {



        userInput = number;
        Debug.Log($"User input updated: {userInput}");


        if (inputDisplay != null)
        {
            inputDisplay.text = userInput;
        }
    }

    public void OnEnterButtonPressed()
    {
        if (int.TryParse(userInput, out int enteredCode))
        {
            if (enteredCode == correctCode)
            {
                Debug.Log("Correct code! Elevator Opened.");
                if (inputDisplay != null)
                {
                    inputDisplay.text = "";
                }
                numPadCanvas.SetActive(false);
                base.Interact();
            }
            else
            {
                DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue("Incorrect code. Try again in a minute.");

                }


                
                numPadCanvas.SetActive(false);

                StartCoroutine(LockElevator());
            }
        }


        userInput = "";
        if (inputDisplay != null)
        {
            inputDisplay.text = "";
        }
    }

    public void OnCloseButtonPressed()
    {
        if (numPadCanvas != null)
        {
            numPadCanvas.SetActive(false); 
        }
        userInput = "";
        if (inputDisplay != null)
        {
            inputDisplay.text = ""; 
        }

    }

    private IEnumerator LockElevator()
    {
        isLocked = true;
        yield return new WaitForSeconds(60);
        isLocked = false;

    }



    public void ResetObject()
    {
        if (vendingMachineScript != null)
        {

            correctCode = vendingMachineScript.numCarrots;
            Debug.Log($"[ElevatorDoor] Reset complete. New correct code is: {correctCode}");
        }
        else
        {
            Debug.LogWarning("[ElevatorDoor] VendingMachine script reference is missing during reset.");
        }

        isLocked = false;
        userInput = "";
        if (inputDisplay != null)
        {
            inputDisplay.text = "";
        }
        if (numPadCanvas != null)
        {
            numPadCanvas.SetActive(false);
        }
    }


}