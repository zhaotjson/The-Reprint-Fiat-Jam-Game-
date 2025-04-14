using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable, IResettable
{
    public Door doorTo;
    public Transform centerCamera;

    public Sprite spriteOpen;
    public Sprite spriteClosed;

    private SpriteRenderer sprite;

    [SerializeField] private AudioSource playOnInteract;

    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = spriteClosed;
    } 

    public override void Interact()
    {
        sprite.sprite = spriteOpen;
        if (playOnInteract != null) {
            playOnInteract.Play();
        }
        StartCoroutine(Teleport());
    }

    public void Close() {
        sprite.sprite = spriteClosed;
    }

    IEnumerator Teleport() {
        yield return new WaitForSeconds(0.25f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        playerPos.x = doorTo.transform.position.x;
        playerPos.y = doorTo.transform.position.y;
        player.transform.position = playerPos;

        if (centerCamera != null) {
            Vector3 camPos = Camera.main.transform.position;
            camPos.x = doorTo.centerCamera.transform.position.x;
            camPos.y = doorTo.centerCamera.transform.position.y;
            Camera.main.transform.position = camPos;
        }
        yield return new WaitForSeconds(0.25f);
        doorTo.Close();
    }


    public void ResetObject() {
        Close();
    }
}
