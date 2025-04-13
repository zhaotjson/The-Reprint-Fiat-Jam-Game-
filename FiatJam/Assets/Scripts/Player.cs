using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour, IResettable
{

    [SerializeField] private Transform frame;
    private Vector3 initialFramePosition;


    public float speed = 5f;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        camWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;


        startingPosition = transform.position; // Save the starting position

        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }

        if (frame != null)
        {
            initialFramePosition = frame.position; // Save the initial frame position
        }


    }

    // Update is called once per frame
    void Update()
    {


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


        // Camera Follow

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
            if (slotImage != null)
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1, 1, 1, 0);
            }
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            if (i < inventorySlots.Count)
            {
                Image slotImage = inventorySlots[i].GetComponent<Image>();
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

            }
        }

    }




    public void ResetObject()
    {

        transform.position = startingPosition;


        if (frame != null && cam != null)
        {
            cam.transform.position = new Vector3(frame.position.x, frame.position.y, cam.transform.position.z);
            Debug.Log("Camera reset to match frame's position.");
        }


        CloseInventory();


        inventory.Clear();
        UpdateInventoryUI();

        Debug.Log("Player reset to starting position, inventory cleared, and camera aligned with frame.");
    }



}
