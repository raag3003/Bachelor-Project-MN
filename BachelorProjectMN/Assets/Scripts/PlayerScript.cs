using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField, Range(-10, 10)]
    public int karmaScore = 0;
    public GameObject HomeBase;

    private bool isDragging = false;
    private bool isStuck = false; // variable to make sure that once the piece is stuck, it follows the article around when it's dragged

    private string currentHoverTag = "";

    private Vector3 lastKnownPosition; // Store the piece's last known position before returns to "home base".

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStuck)
        {
            transform.position = GameObject.FindGameObjectWithTag(currentHoverTag).GetComponent<Transform>().position;
        }
    }


    // This function runs every frame while the mouse input = left mouse button is kept down
    // We use this function to move the piecesb around on screen.
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


    // This function calls when the left mouse button is released
    // We use this function to let the pieces go on the article sections
    void OnMouseUp()
    {
        Debug.Log("Mouse released on gameobject: " + this.gameObject.name);
        isDragging = false;

        // If the article piece is released while hovering over a valid tag, snap it to that position
        if (currentHoverTag != "" || currentHoverTag == "PaperTestTag") //Change PaperTestTag to the tag name once it's decided
        {
            isStuck = true; // 
            transform.position = GameObject.FindGameObjectWithTag(currentHoverTag).GetComponent<Transform>().position;
        }
    }

    // This function is called when the left mouse button is pressed down on the object
    // At the moment, it is used to debug and make sure the piece gets unstuck from the current location 
    private void OnMouseDown()
    {
        Debug.Log("Mouse pressed on gameobject: " + this.gameObject.name);
        isStuck = false; // Make sure that when you click on the piece, it unsticks from the article so you can move it around again
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure that the piece being dragged is always on top of the piece it's hovering over.
        // Otherwise 2 different pieces with same script will be on top of each other and it will look weird.
        if (isDragging && !isStuck)
        {
            // Change the current hover tag so it knows where to drop the piece in the OnMouseUp function
            currentHoverTag = collision.gameObject.tag;
            Debug.Log("Collided with " + currentHoverTag);

            // Since this is 2D. The higher the sorting order, the more on top it is. So set the sorting order to 1 more than the piece it's hovering over.
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isStuck)
        {
            // Clear the current hover tag since it's no longer hovering over it
            // Otherwise the piece would snap back to the last hovered item no matter where you drop it.
            currentHoverTag = "";
        }
       
    }

    public void ResetPosition()
    {
        if (transform.position != HomeBase.transform.position && !isStuck)
        {
            lastKnownPosition = transform.position; // Store the current position before resetting
            transform.position = HomeBase.transform.position;
            isStuck = false;
            // disable the collider so the player cant drag it from home base while in home base
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            // enable the collider so the player can drag it again when its on the table
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            transform.position = lastKnownPosition; // Move back to the last known position if it's already at home base
        }
    }
}

