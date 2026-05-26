using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("Load First Scene");
        SceneManager.LoadScene("Pre-Day 1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is Exiting");
    }

    public void ContinueToDay1()
    {
        Debug.Log("Load day 1");
        SceneManager.LoadScene("SampleScene");
    }
}
