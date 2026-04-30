using UnityEngine;

public class QuitGameScript : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.Quit();
        }
    }
}
