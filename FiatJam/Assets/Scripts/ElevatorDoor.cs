using System.Collections;
using UnityEngine;
using TMPro;

public class ElevatorDoor : Door
{
    [SerializeField] private GameObject numPadCanvas;
    [SerializeField] private TextMeshProUGUI inputDisplay;
    [SerializeField] private int correctCode = 5;

    private bool isLocked = false;
    private string userInput = "";

    protected override void Start()
    {
        base.Start();
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
            Debug.Log("Elevator is locked. Please wait.");

            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue("The elevator is locked. Please wait a moment.");
            }
            else
            {
                Debug.LogWarning("DialogueManager not found in the scene.");
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
                Debug.Log("Incorrect code. Try again in a minute.");
                DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue("Incorrect code. Try again in a minute.");

                    Debug.Log("Incorrect code. Try again in a minute.");

                }
                else
                {
                    Debug.LogWarning("DialogueManager not found in the scene.");
                }

                
                numPadCanvas.SetActive(false);

                StartCoroutine(LockElevator());
            }
        }
        else
        {
            Debug.LogWarning("Invalid input. Please enter a valid number.");
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
        Debug.Log("Number pad closed.");
    }

    private IEnumerator LockElevator()
    {
        isLocked = true;
        Debug.Log("Elevator locked for 1 minute.");
        yield return new WaitForSeconds(60);
        isLocked = false;
        Debug.Log("Elevator unlocked. You can try again.");
    }
}