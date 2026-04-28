using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FailedSubmitScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine(TutorialText());
    }

    private IEnumerator TutorialText()
    {
        gameObject.GetComponent<Text>().text = "Tryk på mapperne for at få artikel-stykkerne frem og tilbage på bordet!";
        yield return new WaitForSeconds(10f);

        gameObject.GetComponent<Text>().text = "Lav nu din egen artikel ved at fylde de tomme felter af artiklen!";

        KillMyself(15f);
    }

    public void KillMyself(float delayTimer)
    {
        StartCoroutine(DelayedKill(delayTimer));
    }

    private IEnumerator DelayedKill(float delayTimer)
    {
        yield return new WaitForSeconds(delayTimer); // Wait for the specified delay
        this.gameObject.SetActive(false); // Deactivate the GameObject
    }
}
