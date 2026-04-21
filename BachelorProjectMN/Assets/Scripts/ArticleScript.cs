using UnityEngine;

public class ArticleScript : MonoBehaviour
{
    public GameObject submitButton;

    public bool isInSubmitArea = false; // Track whether the article is currently in the submit area

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        // Get the main camera
        Camera cam = Camera.main;
        if (cam == null) return;

        // Distance from camera to the object's z plane
        float distanceToObject = Mathf.Abs(transform.position.z - cam.transform.position.z);

        // Convert screen point to world point at that distance
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToObject));

        // Keep the object's original z
        transform.position = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SubmitTag"))
        {
            isInSubmitArea = true; // Set the flag to true when the article enters the submit area
            Debug.Log("Article entered the submit area!"); // Debug log to confirm the collision is detected
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SubmitTag"))
        {
            isInSubmitArea = false; // Set the flag to false when the article exits the submit area
            Debug.Log("Article left the submit area!"); // Debug log to confirm the collision exit is detected
        }
    }

}
