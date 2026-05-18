using UnityEngine;

public class StartDayScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f; // Pause the game at the start of the day
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check if the Space key is pressed
        {
            Time.timeScale = 1f; // Resume the game when Space is pressed
            Destroy(gameObject); // Destroy this script's GameObject to prevent it from running again
        }
    }
}
