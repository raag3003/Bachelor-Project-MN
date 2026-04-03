using UnityEngine;

public class ArticleScript : MonoBehaviour
{
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

}
