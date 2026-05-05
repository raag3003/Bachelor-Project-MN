using UnityEngine;

public class ArticleScript : MonoBehaviour
{
    public GameObject submitButton;

    public bool isInSubmitArea = false; // Track whether the article is currently in the submit area

    private Vector3 dragOffset; // offset between object and mouse world point when drag starts

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        submitButton.SetActive(false); // Ensure the submit button is hidden at the start
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

        Vector3 target = mouseWorld + dragOffset;

        // Keep the object's original z
        transform.position = new Vector3(target.x, target.y, transform.position.z);
    }

    private void OnMouseDown()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;
        float distanceToobjectTransform = Mathf.Abs(transform.position.z - cam.transform.position.z);
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToobjectTransform));
        
        dragOffset = transform.position - mouseWorld; // Calculate the offset between the object and the mouse world point

        Debug.Log("Mouse down on article! Initial mouse position: " + mouseWorld); // Debug log to confirm the mouse down event is detected
    }

    private void OnMouseUp()
    {
        if (isInSubmitArea)
        {
            submitButton.SetActive(true); // Show the submit button when the article is released in the submit area
        }
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
            submitButton.SetActive(false); // Hide the submit button when the article leaves the submit area
            isInSubmitArea = false; // Set the flag to false when the article exits the submit area
            Debug.Log("Article left the submit area!"); // Debug log to confirm the collision exit is detected
        }
    }

}
