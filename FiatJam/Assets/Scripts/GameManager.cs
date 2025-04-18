using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject resetCanvas;
    [SerializeField] private TMP_Text resetText;

    private List<IResettable> resettableObjects;
    private Coroutine typingCoroutine;

    [SerializeField] private AudioSource textSoundSource;
    [SerializeField] private AudioSource printerSoundSource;

    void Start()
    {
        resettableObjects = new List<IResettable>(FindObjectsOfType<MonoBehaviour>().OfType<IResettable>());

        if (resetCanvas != null)
        {
            resetCanvas.SetActive(false);
        }
        // Screen.SetResolution(1920, 1080, true);

    }

    public void ResetGame(string resetType = "default")
    {

        
        Debug.Log("Here");

        if (resetCanvas != null && resetText != null)
        {
            Player player = FindObjectOfType<Player>();

            Debug.Log("there");
            player.SetMovementEnabled(false);
            if (player != null)
            {
                string message = GenerateResetMessage(player.subjectNum, resetType);
                textSoundSource.Play();
                printerSoundSource.Play();


                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeMessage(message));
            }
            
            Debug.Log("Canvs set active");
            resetCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Reset canvas or reset text is not assigned.");
            PerformReset();
        }
    }

    private string GenerateResetMessage(int subjectNum, string resetType)
    {
        string message = $"SUBJECT #{subjectNum}\n";
        switch (resetType)
        {
            case "pill":
                message += "Successfully tested poisoned pills. \n Displayed great results of use. \n";
                break;

            case "credits":
                message += "Unfortunately our last test subject escaped. \n However, that does not stop us from continuing our tests. \n";
                break;
            case "default":
            default:
                message += "Successfully tested laser weapon.\n Weapon has great promise for future use. \n";
                break;
        }
        message += $"\n Now printing SUBJECT #{subjectNum + 1} \n     - Anthill Industries \n     - 2043";
        return message;
    }

    private IEnumerator TypeMessage(string message)
    {
        resetText.text = "";
        foreach (char c in message)
        {
            resetText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void PerformReset()
    {


        textSoundSource.Stop();
        printerSoundSource.Stop();


        if (resetCanvas != null)
        {
            resetCanvas.SetActive(false);
        }

        foreach (var resettable in resettableObjects)
        {
            resettable.ResetObject();
        }

        Debug.Log("Game has been reset.");
    }
}