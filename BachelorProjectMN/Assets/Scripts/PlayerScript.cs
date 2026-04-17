using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField, Range(-10, 10)]
    public int karmaScore = 0;
    public GameObject HomeBase;

    public SystemScript SystemScript;

    private bool isDragging = false;
    private bool isStuck = false; // variable to make sure that once the piece is stuck, it follows the article around when it's dragged

    private string currentHoverTag = "";
    private Transform currentStuckTarget; // Store the transform of the article piece that the player piece is currently stuck to

    private Vector3 lastKnownPosition; // Store the piece's last known position before returns to "home base"

    private Vector3 startRotation; // Store the piece's starting rotation so it can be reset when it goes back from home base
    private Vector3 homeBaseRotation = new Vector3(0, 0, 90); // Store the rotation for the home base so it can be reset when it goes back to home base

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SystemScript = SystemScript.GetComponent<SystemScript>();

        startRotation = GetRandomRotation(); // Rotate the piece randomly on start so they don't all look the same when they spawn in
        transform.rotation = Quaternion.Euler(startRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStuck)
        {            
            transform.position = currentStuckTarget.position; // Have the piece follow the article piece it's stuck to if it's currently stuck.
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
        isDragging = false;
        // If the article piece is released while hovering over a valid tag, snap it to that position
        if (!string.IsNullOrEmpty(currentHoverTag))
        {
            string[] validTags = { "ArticleTitleTag", "ArticlePictureTag", "ArticleTextTag" }; // Define the valid tags for dropping the piece
            if (!validTags.Contains(currentHoverTag))
            {
                Debug.Log("Invalid drop location. Please drop the piece on an article piece.");
                return; // Exit the function if it's not a valid drop location
            }
            isStuck = true; // Make sure that the piece is stuck so ResetPosition does not move it if pressed

            transform.rotation = Quaternion.Euler(0, 0, 0); // Reset the rotation to 0 when it snaps to the article piece so it looks better when it's on the article piece

            // Get the transform for the article section that the piece is being dropped on so we can snap to it and follow it around when dragged
            currentStuckTarget = GameObject.FindGameObjectWithTag(currentHoverTag).GetComponent<Transform>();
            
            transform.position = currentStuckTarget.position;

            // Disable the collider of the article piece so it doesn't interfere with dragging other pieces around
            currentStuckTarget.GetComponent<BoxCollider2D>().enabled = false;

            // Add the karma score of the piece to the total karma score in the SystemScript
            SystemScript.AddPiece(karmaScore); 
        }
    }

    // This function is called when the left mouse button is pressed down on the object
    // At the moment, it is used to debug and make sure the piece gets unstuck from the current location 
    private void OnMouseDown()
    {
        Debug.Log("Mouse pressed on gameobject: " + this.gameObject.name);
        currentHoverTag = ""; // Clear the current hover tag since we are picking up the piece again

        if (isStuck)
        {
            transform.rotation = Quaternion.Euler(startRotation);

            isStuck = false; // Make sure that when you click on the piece, it unsticks from the article so you can move it around again
            if (currentStuckTarget != null)
            {
                currentStuckTarget.GetComponent<BoxCollider2D>().enabled = true; // Re-enable the collider of the article piece
                               
                currentStuckTarget = null;

                SystemScript.RemovePiece(karmaScore); // Subtract the karma score of the piece from the total karma score in the SystemScript
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure that the piece being dragged is always on top of the piece it's hovering over.
        // Otherwise 2 different pieces with same script will be on top of each other and it will look weird.
        if (isDragging && !isStuck)
        {
            // Change the current hover tag so it knows where to drop the piece in the OnMouseUp function
            currentHoverTag = collision.gameObject.tag;

            // Since this is 2D. The higher the sorting order, the more on top it is. So set the sorting order to 1 more than the piece it's hovering over.
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited trigger with gameobject: " + collision.gameObject.name);
        if (!isStuck)
        {
            // Clear the current hover tag since it's no longer hovering over it
            // Otherwise the piece would snap back to the last hovered item no matter where you drop it.
            currentHoverTag = null;
        }
       
    }

    // This function is for the button that resets all pieces back to the "home base" position.
    // If the piece is already at home base, it will move back to the last known position before it was reset.
    public void ResetPosition()
    {
        if (transform.position != HomeBase.transform.position && !isStuck)
        {
            lastKnownPosition = transform.position; // Store the current position before resetting
            transform.position = HomeBase.transform.position;
            isStuck = false;

            transform.rotation = Quaternion.Euler(homeBaseRotation); // Reset the rotation to the home base rotation

            // disable the collider so the player cant drag it from home base while in home base
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            // enable the collider so the player can drag it again when its on the table
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            transform.position = lastKnownPosition; // Move back to the last known position if it's already at home base

            transform.rotation = Quaternion.Euler(GetRandomRotation()); // Reset the rotation to a random rotation
        }
    }

    Vector3 GetRandomRotation() // Small Vector3 function to get a random rotation eachtime its reset from the pile
    { return new Vector3(0, 0, Random.Range(-15f, 15f)); }
}

