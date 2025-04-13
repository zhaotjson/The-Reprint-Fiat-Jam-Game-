using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Renderer EIndicator;
    public bool isInteractable = false;

    protected virtual void Start()
    {
        EIndicator.enabled = false;
    } 

    void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }   
    }

    public virtual void Interact()
    {
        Debug.Log("This interaction was not implemented ;(");
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInteractable = true;
            EIndicator.enabled = true;
        }       
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInteractable = false;
            EIndicator.enabled = false;
        }
    }
}
