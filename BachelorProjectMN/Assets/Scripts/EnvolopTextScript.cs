using UnityEngine;

public class EnvolopTextScript : MonoBehaviour
{
    public GameObject ChangeGameObject; // Assign this in the Unity Inspector with the GameObject representing the zoomed-in view

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check if Space key is pressed while zooming
        {
            ChangeGameObject.SetActive(true); // Cahgne GameObject to show the envolop
            ChangeGameObject.transform.rotation = Quaternion.Euler(ChangeGameObject.GetComponent<EnvolopScript>().GetRandomRotation()); // Reset the rotation to a random rotation
            this.gameObject.SetActive(false); // Reactivate the current GameObject
        }
    }
}
