using System.ComponentModel;
using UnityEngine;

public class EnvolopScript : MonoBehaviour
{
    private bool Hovering = false;
    private bool isDragging = false; // Track whether the object is currently being dragged

    public GameObject ChangeGameObject; // Assign this in the Unity Inspector with the GameObject representing the zoomed-in view

    private void OnMouseOver()
    {
        Hovering = true;
    }

    private void OnMouseExit()
    {
        Hovering = false;
    }

    private void Update()
    {
        if (Hovering && Input.GetMouseButtonDown(1)) // Check if right mouse button is clicked while hovering
        {
            ChangeGameObject.SetActive(true); // Change GameObject to show the zoomed-in view
            this.gameObject.SetActive(false); // Deactivate the current GameObject
        }
    }

    private void OnMouseDrag()
    {
        isDragging = true;

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
    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging)
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
}
