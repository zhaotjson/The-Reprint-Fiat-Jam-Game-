using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas; 
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;

    private void Start()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
    }

    public void ShowDialogue(string message)
    {
        if (dialogueCanvas != null && dialogueText != null)
        {
            dialogueCanvas.SetActive(true);

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }


            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }
    }

    private IEnumerator TypeDialogue(string message)
    {
        dialogueText.text = "";
        foreach (char c in message)
        {
            dialogueText.text += c; 
            yield return new WaitForSeconds(typingSpeed);
        }


        yield return new WaitForSeconds(3f);

        HideDialogue();
    }

    public void HideDialogue()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
    }
}