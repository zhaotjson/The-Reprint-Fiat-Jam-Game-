using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microwave : Interactable
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        Debug.Log("*beep* *beep* *beep*");
    }
}
