using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class countDown : MonoBehaviour
{

    public float timeLeft = 60.0f;


    [SerializeField] private TMP_Text countdownText;




    void Start()
    {
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

    }

}
