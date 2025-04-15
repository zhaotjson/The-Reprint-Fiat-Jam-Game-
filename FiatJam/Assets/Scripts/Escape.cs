using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : Interactable
{
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private RectTransform creditsText;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float watchTime = 23f;

    private Vector2 initialPosition;

    [SerializeField] private AudioSource creditsAudioSource;

    void Start()
    {
        base.Start();
        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false);
        }

        if (creditsText != null)
        {
            initialPosition = creditsText.anchoredPosition;
        }
    }

    public override void Interact()
    {
        if (creditsCanvas != null && creditsText != null)
        {
            StopAllCounters();
            creditsCanvas.SetActive(true);
            StartCoroutine(ScrollCredits());
            if (creditsAudioSource != null)
            {
                creditsAudioSource.Play();
            }
        }
    }

    private IEnumerator ScrollCredits()
    {

        float elapsedTime = 0f;


        while (elapsedTime < watchTime)
        {
            creditsText.anchoredPosition -= Vector2.up * scrollSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, -creditsText.rect.height);



        creditsCanvas.SetActive(false);


        creditsText.anchoredPosition = initialPosition;


        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ResetGame("credits");
        }
    }

    private void StopAllCounters()
    {
        countDown[] counters = FindObjectsOfType<countDown>();
        foreach (countDown counter in counters)
        {
            counter.StopCounter();
        }

        Debug.Log("All counters have been stopped.");
    }
}