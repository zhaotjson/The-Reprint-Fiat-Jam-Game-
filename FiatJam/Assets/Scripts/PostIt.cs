using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PostIt : Interactable, IResettable
{
    [SerializeField] private GameObject postItCanvas; 
    [SerializeField, TextArea] private string postItText;

    [SerializeField] private TMP_Text postItTextComponent;
    private void Start()
    {

        base.Start();
        if (postItCanvas != null)
        {
            postItCanvas.SetActive(false);
        }
    }

    public override void Interact()
    {
        if (postItCanvas != null)
        {
            postItCanvas.SetActive(true);

            if (postItTextComponent != null)
            {
                postItTextComponent.text = postItText;
            }
            else
            {
                Debug.LogWarning("PostItTextComponent is not assigned or missing in the canvas.");
            }
        }
        else
        {
            Debug.LogWarning("PostIt canvas is not assigned.");
        }
    }

    public void CloseCanvas()
    {
        if (postItCanvas != null)
        {
            postItCanvas.SetActive(false);
        }
    }

    public void ResetObject()
    {
        CloseCanvas();
    }
}