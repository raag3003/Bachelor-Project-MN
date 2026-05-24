using UnityEngine;

public class ArticleScript : MonoBehaviour
{    private Vector3 dragOffset; // offset between object and mouse world point when drag starts

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
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

}
