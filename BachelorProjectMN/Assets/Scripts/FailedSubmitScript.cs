using System.Collections;
using UnityEngine;

public class FailedSubmitScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillMyself()
    {
        StartCoroutine(DelayedKill());
    }

    private IEnumerator DelayedKill()
    {
        yield return new WaitForSeconds(3f); // Wait for 2 seconds
        this.gameObject.SetActive(false); // Deactivate the GameObject
    }
}
