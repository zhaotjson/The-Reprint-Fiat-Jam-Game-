using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Microwave : Interactable, IResettable
{



    [SerializeField] private GameObject microwaveCanvas;

    [SerializeField] private List<GameObject> inventorySlots =  new List<GameObject>();


    [SerializeField] private List<GameObject> inventoryButtons = new List<GameObject>();
    [SerializeField] public GameObject microwaveItem = null;
    [SerializeField] private GameObject microwaveButton;

    [SerializeField] private GameObject microwaveItemPrefab;

    [SerializeField] private GameObject hotBurgerPrefab;
    [SerializeField] private GameObject hardNoodlesPrefab;

    [SerializeField] private AudioSource microwaveOpenSound;
    [SerializeField] private AudioSource microwaveCloseSound;
    [SerializeField] private AudioSource microwaveUseSound;
    [SerializeField] private AudioSource microwaveBeepSound;

    private Vector3 originalPrefabPosition;

    public bool isCooking = false;
    public float cookingTime = 60f;
    public float remainingTime = 0f;

    [SerializeField] private TMP_Text timerText;
    private Coroutine cookingCoroutine;




    protected override void Start()
    {
        base.Start();



        if (microwaveCanvas != null){
            microwaveCanvas.SetActive(false);
        }


        for (int i = 0; i < inventoryButtons.Count; i++)
        {
            int index = i;
            Button button = inventoryButtons[i].GetComponent<Button>();
            if (button != null)
            {
                Debug.Log($"Adding listener to button {index}");
                button.onClick.AddListener(() => OnInventoryButtonClicked(index));
            }
            else
            {
                Debug.LogWarning($"Button component missing on inventoryButtons[{i}]");
            }
        }
        if (microwaveButton != null){
            microwaveButton.transform.SetAsLastSibling();
            Button button = microwaveButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnMicrowaveButtonClicked);
            }
        }

        if (microwaveItemPrefab != null)
        {
            RectTransform prefabRectTransform = microwaveItemPrefab.GetComponent<RectTransform>();
            if (prefabRectTransform != null)
            {
                originalPrefabPosition = prefabRectTransform.localPosition;
                Debug.Log($"Original prefab position saved: {originalPrefabPosition}");
            }
        }

        if (timerText != null)
        {
            timerText.text = "";
        }


    }


    private void UpdateInventoryDisplay(Player player)
    {
        if (player != null)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                Image slotImage = inventorySlots[i].GetComponent<Image>();
                if (slotImage != null)
                {
                    if (i < player.inventory.Count && player.inventory[i] != null)
                    {

                        SpriteRenderer itemSpriteRenderer = player.inventory[i].GetComponent<SpriteRenderer>();
                        if (itemSpriteRenderer != null)
                        {
                            slotImage.sprite = itemSpriteRenderer.sprite;
                            slotImage.color = new Color(1, 1, 1, 1);
                        }
                    }
                    else
                    {

                        slotImage.sprite = null;
                        slotImage.color = new Color(1, 1, 1, 0);
                    }
                }
            }
            player.UpdateInventoryUI();
        }
    }

    public void OnInventoryButtonClicked(int index)
    {
        Debug.Log($"Inventory button {index} clicked.");

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null && index >= 0 && index < player.inventory.Count)
            {
                GameObject inventoryItem = player.inventory[index];

                if (inventoryItem == null && microwaveItem == null)
                {
                    Debug.Log("Both inventory slot and microwave are empty. No action performed.");
                    return;
                }


                if (inventoryItem != null && microwaveItem == null)
                {
                    string itemName = inventoryItem.name;
                    if (itemName != "coldBurger" && itemName != "coldNoodles")
                    {
                        Debug.Log($"Item '{itemName}' cannot be placed in the microwave.");
                        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                        if (dialogueManager != null)
                        {
                            dialogueManager.ShowDialogue($"I don't think I can microwave that.");
                        }
                        return;
                    }
                }

                if (inventoryItem != null && microwaveItem != null)
                {
                    string itemName = inventoryItem.name;
                    if (itemName != "coldBurger" && itemName != "coldNoodles")
                    {
                        Debug.Log($"Item '{itemName}' cannot be swapped into the microwave.");
                        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                        if (dialogueManager != null)
                        {
                            dialogueManager.ShowDialogue($"I don't think I can microwave that.");
                        }
                        return;
                    }
                }


                if (microwaveItem != null)
                {
                    GameObject temp = microwaveItem;
                    microwaveItem = inventoryItem;
                    player.inventory[index] = temp;
                }
                else
                {
                    microwaveItem = inventoryItem;
                    player.inventory[index] = null;
                }

                UpdateMicrowaveDisplay();
                UpdateInventoryDisplay(player);
            }
        }
    }
    public void OnMicrowaveButtonClicked()
    {
        if (microwaveItem != null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Player player = playerObject.GetComponent<Player>();
                if (player != null)
                {
                    bool itemAdded = false;

                    for (int i = 0; i < player.inventory.Count; i++)
                    {
                        if (player.inventory[i] == null)
                        {
                            player.inventory[i] = microwaveItem;
                            Debug.Log($"Added {microwaveItem.name} to inventory slot {i}.");
                            itemAdded = true;
                            break;
                        }
                    }


                    if (!itemAdded && player.inventory.Count < 5)
                    {
                        // player.inventory.Add(microwaveItem);
                        player.AddToInventory(microwaveItem);
                        Debug.Log($"Added {microwaveItem.name} to a new inventory slot. Inventory size is now {player.inventory.Count}.");
                        itemAdded = true;
                    }

                    if (!itemAdded)
                    {
                        Debug.LogWarning("No empty inventory slots available. Item not added.");
                    }

                    microwaveItem = null;

                    UpdateMicrowaveDisplay();
                    UpdateInventoryDisplay(player);
                }
            }
        }
        else
        {
            Debug.Log("Microwave is empty, nothing to add to inventory.");
        }
    }

    public override void Interact()
    {


        if (microwaveCanvas != null)
        {
            microwaveCanvas.SetActive(true);

        }


        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                player.SetMovementEnabled(false);
                UpdateInventoryDisplay(player);
                microwaveOpenSound.Play();
            }
        }

        UpdateMicrowaveDisplay();
    }

    private void UpdateMicrowaveDisplay()
    {
        if (microwaveItem != null)
        {
            Debug.Log("Microwave item is not null. Attempting to update display.");


            microwaveItemPrefab.SetActive(true);


            SpriteRenderer microwaveSpriteRenderer = microwaveItem.GetComponent<SpriteRenderer>();

            Image prefabImage = microwaveItemPrefab.GetComponent<Image>();

            if (microwaveSpriteRenderer != null)
            {
                Debug.Log($"Microwave item sprite found: {microwaveSpriteRenderer.sprite?.name ?? "No sprite assigned"}");
            }
            else
            {
                Debug.LogWarning("Microwave item does not have a SpriteRenderer component.");
            }

            if (prefabImage != null)
            {
                Debug.Log("Microwave prefab Image component found.");
            }
            else
            {
                Debug.LogWarning("Microwave prefab does not have an Image component.");
            }


            if (microwaveSpriteRenderer != null && prefabImage != null)
            {
                prefabImage.sprite = microwaveSpriteRenderer.sprite;
                prefabImage.color = new Color(1, 1, 1, 1); 

                if (prefabImage.sprite != null)
                {
                    Debug.Log($"Microwave prefab image successfully updated to: {prefabImage.sprite.name}");
                }
                else
                {
                    Debug.LogWarning("Microwave prefab image is null after update.");
                }
            }


            RectTransform prefabRectTransform = microwaveItemPrefab.GetComponent<RectTransform>();
            if (prefabRectTransform != null)
            {
                prefabRectTransform.localPosition = originalPrefabPosition;
                Debug.Log($"Microwave prefab local position reset to original: {prefabRectTransform.localPosition}");
            }
            else
            {
                Debug.LogWarning("Microwave prefab does not have a RectTransform component.");
            }
        }
        else
        {
            Debug.Log("Microwave item is null. Hiding the microwaveItemPrefab.");

            microwaveItemPrefab.SetActive(false);
        }
    }
    public void CloseCanvas()
    {
        if (microwaveCanvas != null){

            microwaveCanvas.SetActive(false);

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Player player = playerObject.GetComponent<Player>();
                if (player != null)
                {
                    player.SetMovementEnabled(true);
                }
            }

            microwaveCloseSound.Play();
        }
    }




    public void StartCooking()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (microwaveItem == null)
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue("Hmmm, nothing to cook here");
            }
            return;
        }

        if (isCooking)
        {
            Debug.Log("Microwave is already cooking.");
            return;
        }

        Debug.Log("Microwave started cooking.");
        if (dialogueManager != null)
        {
            dialogueManager.ShowDialogue("I don't think this will finish in time. Maybe the next person can use it.");
        }
        isCooking = true;
        cookingCoroutine = StartCoroutine(CookingTimer());
        microwaveUseSound.loop = true;
        microwaveUseSound.Play();
    }


    private IEnumerator CookingTimer()
    {
        float remainingTime = cookingTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = $"{Mathf.CeilToInt(remainingTime)}";
            }

            yield return null; // Wait for the next frame
        }

        FinishCooking();
    }

    private void FinishCooking()
    {
        Debug.Log("Microwave finished cooking.");
        isCooking = false;

        if (timerText != null)
        {
            timerText.text = "";
        }

        if (microwaveItem != null && microwaveItem.name == "coldBurger" && hotBurgerPrefab != null)
        {
            Debug.Log("Replacing coldBurger with hotBurger.");
            microwaveItem = Instantiate(hotBurgerPrefab);
        }
        else if (microwaveItem != null && microwaveItem.name == "coldNoodles" && hardNoodlesPrefab != null)
        {
            Debug.Log("Replacing coldNoodles with hardNoodles.");
            microwaveItem = Instantiate(hardNoodlesPrefab);
        }

        microwaveUseSound.Stop();
        microwaveBeepSound.Play();
        UpdateMicrowaveDisplay();
    }




    public void ResetObject()
    {
        CloseCanvas();

        if (isCooking)
        {
            Debug.Log("Microwave was cooking during reset. Finishing cooking process.");
            FinishCooking();
        }

        isCooking = false;
        remainingTime = 0f;

        if (cookingCoroutine != null)
        {
            StopCoroutine(cookingCoroutine);
            cookingCoroutine = null;
        }

        if (timerText != null)
        {
            timerText.text = "";
        }

        UpdateMicrowaveDisplay();
    }
}
