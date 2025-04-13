using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour, IResettable
{

    [SerializeField] private Transform frame;
    private Vector3 initialFramePosition;


    public float speed = 5f;

    public int subjectNum;


    public bool isBuffed = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private Camera cam;
    private float camWidth;

    private bool inventoryOpen = false;

    private bool canMove = true;


    public List<GameObject> inventory = new List<GameObject>();
    public int maxInventorySize = 5;

    [SerializeField] private GameObject inventoryCanvas;

    [SerializeField] private List<GameObject> inventorySlots;

    private Vector3 startingPosition;

    void Start()
    {

        isBuffed = false;
        rb = GetComponent<Rigidbody2D>();    
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        camWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;


        startingPosition = transform.position;

        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }

        if (frame != null)
        {
            initialFramePosition = frame.position; 
        }

        subjectNum = 89;

    }


    void Update()
    {

        /*

            TESTING PURPOSES, REMOVE LATER


        */
        if (Input.GetKeyDown(KeyCode.R))
        {

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.ResetGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventoryOpen)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }


        // Movement

        if (!canMove) return;



        if (IsGrounded()) {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                rb.AddForce(new Vector2(-1, 1) * speed, ForceMode2D.Impulse);
                sprite.flipX = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                rb.AddForce(new Vector2(1, 1) * speed, ForceMode2D.Impulse);
                sprite.flipX = false;
            }
        }




        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        Vector3 camPos = cam.transform.position;

        if (viewPos.x < 0)
            camPos.x -= camWidth;
        else if (viewPos.x > 1)
            camPos.x += camWidth;

        cam.transform.position = camPos;
    
    }

    bool IsGrounded()
    {
        return rb.velocity.y == 0;
    }



    public bool AddToInventory(GameObject item)
    {
        if (inventory.Count < maxInventorySize)
        {
            inventory.Add(item);
            Debug.Log("Item added to inventory: " + item.name);
            return true;
        }
        else
        {
            Debug.Log("Inventory is full");
            return false;
        }
    }


    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

    }


    public void OpenInventory()
    {

        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(true);
            canMove = false;
            Debug.Log("Inventory opened");
            UpdateInventoryUI();
            inventoryOpen = true;
        }


    }

    public void CloseInventory()
    {
        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
            canMove = true;
            Debug.Log("Inventory closed");
            inventoryOpen = false;
        }
    }





    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Image slotImage = inventorySlots[i].GetComponent<Image>();
            Button slotButton = inventorySlots[i].GetComponent<Button>();

            if (slotImage != null)
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1, 1, 1, 0);
            }

            if (slotButton != null)
            {
                slotButton.onClick.RemoveAllListeners();
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (i < inventorySlots.Count && inventory[i] != null) 
            {
                Image slotImage = inventorySlots[i].GetComponent<Image>();
                Button slotButton = inventorySlots[i].GetComponent<Button>();

                if (slotImage != null)
                {
                    SpriteRenderer itemSprite = inventory[i].GetComponent<SpriteRenderer>();

                    if (itemSprite != null)
                    {
                        slotImage.sprite = itemSprite.sprite;
                        slotImage.color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        Debug.LogWarning("Item does not have a SpriteRenderer component: " + inventory[i].name);
                    }
                }

                if (slotButton != null)
                {
                    int index = i;
                    slotButton.onClick.AddListener(() => OnInventorySlotClicked(index));
                }
            }
        }
    }

    private void OnInventorySlotClicked(int index)
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (index >= 0 && index < inventory.Count && inventory[index] != null)
        {
            GameObject item = inventory[index];
            string itemName = item.name;

            Debug.Log($"Clicked on inventory item: {itemName}");

            if (itemName == "hotBurger" || itemName == "hotBurger(Clone)")
            {
                ConsumeItem(index);
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue("WOW, I feel soo much stronger");

                }
                isBuffed = true;
            }
            else if (itemName == "hardNoodles" || itemName == "hardNoodles(Clone)")
            {
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue("I don't feel like eating it, but it seems like I can break something with it");
                }
            }
            else if (itemName == "pills" || itemName == "pills(Clone)")
            {
                if (dialogueManager != null)
                {
                    ConsumeItem(index);
                    dialogueManager.ShowDialogue("I don't feel soo good");
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    if (gameManager != null)
                    {
                        gameManager.ResetGame("pill");
                    }
                }
            }
            else if (itemName == "coldNoodles" || itemName == "coldNoodles(Clone)" || itemName == "coldBurger" || itemName == "coldBurger(Clone)")
            {
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue("Need to heat this first");
                }
            }
            else if (itemName == "money" || itemName == "money(Clone)")
            {
                ConsumeItem(index);
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue("I don't feel like eating it, but it seems like I can break something with it");
                }
            }
            else
            {
                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue($"Can't do anything with this right now");
                }
            }
        }
        else
        {
            Debug.LogWarning("Clicked on an empty or invalid inventory slot.");
        }
    }

    private void ConsumeItem(int index)
    {
        if (index >= 0 && index < inventory.Count && inventory[index] != null)
        {
            GameObject item = inventory[index];
            inventory.RemoveAt(index);
            UpdateInventoryUI();
            Debug.Log($"Consumed item: {item.name}");
        }
        else
        {
            Debug.LogWarning("Attempted to consume an invalid or empty inventory slot.");
        }
    }






    public void ResetObject()
    {

        transform.position = startingPosition;
        isBuffed = false;

        subjectNum++;


        if (frame != null && cam != null)
        {
            cam.transform.position = new Vector3(frame.position.x, frame.position.y, cam.transform.position.z);
            Debug.Log("Camera reset to match frame's position.");
        }
        speed = 5f;


        CloseInventory();


        inventory.Clear();
        UpdateInventoryUI();

        Debug.Log("Player reset to starting position, inventory cleared, and camera aligned with frame.");
    }



}
