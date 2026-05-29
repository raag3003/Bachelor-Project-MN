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

    public void ContinueDay2Truth()
    {
        Debug.Log("Load Day 2 truth");
        SceneManager.LoadScene("True news scene 1");
    }

    public void ContinueDay2Fake()
    {
        Debug.Log("Load Day 2 fake");
        SceneManager.LoadScene("Fake News Scene 1");
    }

    public void ContinueDay2Middle()
    {
        Debug.Log("Load Day 2 middle");
        SceneManager.LoadScene("Mediocer news scene 1");
    }

    public void GoBackToMainMenu()
    {
        Debug.Log("Load Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

}
