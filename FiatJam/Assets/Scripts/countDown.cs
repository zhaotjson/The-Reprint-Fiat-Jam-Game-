using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countDown : MonoBehaviour, IResettable
{
    public float timeLeft = 60.0f;

    [SerializeField] private TMP_Text countdownText;

    private float initialTime;

    void Start()
    {
        initialTime = timeLeft;
        StartCoroutine(CountdownCoroutine());
    }

    void Update()
    {
        if (countdownText != null)
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString();
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeLeft -= 1.0f;
        }


        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ResetGame();
        }
    }


    public void ResetObject()
    {

        timeLeft = initialTime;


        StopAllCoroutines();
        StartCoroutine(CountdownCoroutine());

        Debug.Log("Countdown reset to initial time.");
    }
}