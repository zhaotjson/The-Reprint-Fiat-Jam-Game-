using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGameScene()
    {
        SceneManager.LoadScene("MainGame");
    }
}