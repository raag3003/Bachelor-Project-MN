using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Piece Settings")]
    [SerializeField, Range(-10, 10)]
    public int karmaScore = 0;
    public GameObject HomeBase;
    public GameObject ZoomArea;
    private Component ZoomAreaSpriteComponent;

    public SystemScript SystemScript;
    private ZoomAreaScript ZoomAreaScript;

    // These two sprites are for the zoom in and zoom out versions of the piece. They should be set in the inspector for each piece.
    [Header("Sprites")]
    public Sprite DeafaultSprite; // This is the default sprite for the piece when it's not zoomed in. It should be set in the inspector for each piece.
    public Sprite ZoomedInSprite; // This sprite is for when the piece is zoomed in. It should be set in the inspector for each piece.
    public Sprite StuckSprite; // This sprite is for when the piece is stuck to an article piece. It should be set in the inspector for each piece.


    private bool isDragging = false;
    private bool isStuck = false; // variable to make sure that once the piece is stuck, it follows the article around when it's dragged
    private bool mouseOver = false; // variable to make sure that the right mouse button only works when hovering over the piece

    private string currentHoverTag = null;
    private Transform currentStuckTarget; // Store the transform of the article piece that the player piece is currently stuck to

    private Vector3 lastKnownPosition; // Store the piece's last known position before returns to "home base"
    private Color originalColor;
    private Vector3 startRotation; // Store the piece's starting rotation so it can be reset when it goes back from home base

    private readonly string[] validTags = { "ArticleTitleTag", "ArticlePictureTag", "ArticleTextTag" }; // Define the valid tags for dropping the piece. This is used in multiple functions.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SystemScript = SystemScript.GetComponent<SystemScript>();

        ZoomAreaScript = ZoomArea.GetComponent<ZoomAreaScript>();

        ResetPosition(); // Call the ResetPosition function at the start to make sure that all pieces start at home base

        originalColor = this.gameObject.GetComponent<SpriteRenderer>().color; // Store the original color of the piece so it can be reset when zooming in and out
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && mouseOver)
        {
            Debug.Log("Right mouse button pressed");
            lastKnownPosition = transform.position; // Store the current position before zooming in so it doesn't look weird when you zoom out
            ZoomIn(); // If the right mouse button is clicked while the piece is zoomed in, zoom out before picking it up again so it doesn't look weird when you pick it up
        }

        if (isStuck)
            transform.position = new Vector3(currentStuckTarget.position.x, currentStuckTarget.position.y, 0); // Have the piece follow the article piece it's stuck to if it's currently stuck.
        
    }


    /// <summary>
    /// This function runs every frame while the mouse input = left mouse button is kept down and is moving around
    /// We use this function to move the pieces around on screen.
    /// </summary>
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


    /// <summary>
    /// This function calls when the left mouse button is released
    /// We use this function to let the pieces go on the article sections
    /// When the player drops the piece while hovering over a valid article section, it snaps to that section and becomes "stuck" to it so it follows the article around when it's dragged.
    /// Otherwise it drops the piece on the table and skip the rest of the function.
    /// </summary>
    void OnMouseUp()
    {
        isDragging = false;
        // If the article piece is released while hovering over a valid tag, snap it to that position
        if (!string.IsNullOrEmpty(currentHoverTag))
        {
            if (!validTags.Contains(currentHoverTag))
                return; // Exit the function if it's not a valid drop location
            
            isStuck = true; // Make sure that the piece is stuck so ResetPosition does not move it if pressed
            
            // Change the sprite to the stuck version of the piece when it snaps to the article piece so it looks better when it's on the article piece
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StuckSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white; // Change the color to white when it snaps to the article piece so it's easier to read

            transform.rotation = Quaternion.Euler(0, 0, 0); // Reset the rotation to 0 when it snaps to the article piece so it looks better when it's on the article piece

            // Get the transform for the article section that the piece is being dropped on so we can snap to it and follow it around when dragged
            currentStuckTarget = GameObject.FindGameObjectWithTag(currentHoverTag).GetComponent<Transform>();
            
            transform.position = new Vector3 (currentStuckTarget.position.x, currentStuckTarget.position.y, 0);

            currentStuckTarget.gameObject.SetActive(false); // Remove the section so there is not an accidentel background

            // Disable the collider of the article piece so it doesn't interfere with dragging other pieces around
            currentStuckTarget.GetComponent<BoxCollider2D>().enabled = false;

            // Add the karma score of the piece to the total karma score in the SystemScript
            SystemScript.AddPiece(karmaScore); 
        }
    }

    /// <summary>
    /// This function is called when the left mouse button is pressed down on the object
    /// The first thing it does is clear the currentHoverTag variable so it doesn't snap to the last hovered item when you pick it up again.
    /// It then checks if the piece is currently zoomed in. If it is, it calls the ZoomIn function to zoom out before picking it up again.
    /// If the piece is stuck to an article, it changes the sprite back to the default version of the piece and changes the color back to the original color so it looks better when it's on the table.
    /// </summary>
    private void OnMouseDown()
    {
        currentHoverTag = ""; // Clear the current hover tag since we are picking up the piece again
         
        if (isStuck)
        {
            // Change the sprite back to the default version of the piece when it gets unstuck so it looks better when it's on the table
            this.gameObject.GetComponent<SpriteRenderer>().sprite = DeafaultSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().color = originalColor; // Change the color back to the original color when it gets unstuck so it looks better when it's on the table

            transform.localScale = new Vector3(1.5f, 1f, 1); // Reset the scale to normal
            transform.rotation = Quaternion.Euler(GetRandomRotation());

            isStuck = false; // Make sure that when you click on the piece, it unsticks from the article so you can move it around again
            if (currentStuckTarget != null)
            {
                currentStuckTarget.GetComponent<BoxCollider2D>().enabled = true; // Re-enable the collider of the article piece

                currentStuckTarget.gameObject.SetActive(true); // Re-enable the article piece so it appears on the table again when you unstick the piece from it

                currentStuckTarget = null;

                SystemScript.RemovePiece(karmaScore); // Subtract the karma score of the piece from the total karma score in the SystemScript
            }
        }
    }

    /// <summary>
    /// These 2 functions are used to set the mouseOver variable to true when the mouse is over the piece and false when it's no longer over the piece.
    /// Then in Update() we can zoom in and out
    /// We can only zoom in if the mouse is currently over the piece, otherwise it would work no matter where you are on the screen which would look weird.
    /// </summary>
    private void OnMouseOver()
    {
        mouseOver = true; // Set the mouseOver variable to true when the mouse is over the piece so the right mouse button only works when hovering over the piece
    }
    private void OnMouseExit()
    {
        mouseOver = false; // Set the mouseOver variable to false when the mouse is no longer over the piece so the right mouse button only works when hovering over the piece
    }

    /// <summary>
    /// OnTriggerEnter2D is used to make sure that when the piece being dragged is entering a trigger, it checks to see if it's a valid drop location
    /// If it is a valid drop location, it changes the color of the drop location and changes the variable currentHoverTag -
    /// to the tag of the drop location so it knows where to snap the piece when it's released in the OnMouseUp function.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure that the piece being dragged is always on top of the piece it's hovering over.
        // Otherwise 2 different pieces with same script will be on top of each other and it will look weird.
        if (isDragging && !isStuck)
        {
            if (!validTags.Contains(collision.gameObject.tag))
                return; // Exit the function if it's not a valid drop location
            
            if (validTags.Contains(collision.gameObject.tag))
            {
                // Change the current hover tag so it knows where to drop the piece in the OnMouseUp function
                currentHoverTag = collision.gameObject.tag;
                // Change the color of the article piece to a semi-transparant green when hovering over it to indicate that it's a valid drop location
                collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 0f, 0.35f); 
            }
            

            // Since this is 2D. The higher the sorting order, the more on top it is. So set the sorting order to 1 more than the piece it's hovering over.
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;

            Debug.Log("Entered trigger with tag: " + collision.gameObject.tag);
            Debug.Log("Current hover tag: " + currentHoverTag);
        }
    }

    /// <summary>
    /// OnTriggerExit2D is used for a simpel task. Change the color of the drop location back to its original color when the piece being dragged is no longer hovering over it.
    /// And clear the current hover tag so it doesn't snap to the last hovered item when you drop it somewhere else on the table.
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (validTags.Contains(collision.gameObject.tag))
        {
            // Change the color of the article piece to red when no longer hovering over it to indicate that it's not a valid drop location
            collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 0.35f);
        }

        if (!isStuck)
        {
            // Clear the current hover tag since it's no longer hovering over it
            // Otherwise the piece would snap back to the last hovered item no matter where you drop it.
            currentHoverTag = null;
        }
       
    }

    /// <summary>
    /// We use this function to toggle between the zoomed in and zoomed out state for each piece
    /// When zoomed in, the piece gets larger and changes sprite to represent the zoomed in version to make it readable.
    /// This means that position, rotation and color changes so it gets centered.
    /// </summary>
    /// <param name="_startPosition">
    /// _startPosition is the position that the piece should return to when zooming out. 
    /// This is used to make sure that the piece returns to the same position it was in before zooming in, instead of returning to a default position which would look weird.
    /// </param>
    private void ZoomIn()
    {
        ZoomAreaScript.ZoomIn(ZoomedInSprite, true); // Call the ZoomIn function in the ZoomAreaScript and pass the current piece as a parameter so it can set the zoomed in piece to this piece

        /* if (isDragging)
         {
             Debug.Log("Cannot zoom in while dragging the piece. Please release the piece first by releasing the left mouse button.");
             return; // Exit the function if the piece is currently being dragged
         }
         if (isStuck)
         {
             Debug.Log("Cannot zoom in while the piece is stuck to an article piece. Please unstick the piece first by clicking on it again.");
             return; // Exit the function if the piece is currently stuck to an article piece
         }
         Vector3 zommedInPosition = new Vector3(5f, 3f, 0f);
         if (!isZoomedIn)
         {
             this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 100; // Set the sorting order to a high value so it appears on top of all the other pieces when zoomed in
             this.gameObject.GetComponent<SpriteRenderer>().color = Color.white; // Change the color to white when zooming in so it's easier to read
             this.gameObject.GetComponent<SpriteRenderer>().sprite = ZoomedInSprite; // Change the sprite to the zoomed in version of the piece when zooming in so it's easier to read

             isZoomedIn = true;

             transform.rotation = Quaternion.Euler(0, 0, 0); // Reset the rotation to 0 when zooming in so it looks better when it's zoomed in
             transform.position = zommedInPosition; // Move the piece to the center of the screen when zooming in so it's easier to read
             transform.localScale = new Vector3(3.7f, 2.2f, 2.2f); // Zoom in on the piece by increasing its local scale
         }
         else
         {
             this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 100; // Set the sorting order to a high value so it appears on top of all the other pieces when zoomed in
             this.gameObject.GetComponent<SpriteRenderer>().color = originalColor; // Change the color back to the original color when zooming out
             this.gameObject.GetComponent<SpriteRenderer>().sprite = DeafaultSprite; // Change the sprite back to the default version of the piece when zooming out

             isZoomedIn = false;

             transform.rotation = Quaternion.Euler(GetRandomRotation()); // Rotate the piece randomly when zooming out so it doesn't look the same as all the other pieces when it's back on the table
             transform.position = _startPosition; // Reset the position to the original position before zooming in so it doesn't look weird when you zoom out
             transform.localScale = new Vector3(1.5f, 1f, 1); // Reset the scale to normal
         }*/ // Old zoom method
    }

    /// <summary>
    /// This function are connected with a button in the inspector. Each button is connected to all pieces of a specific type
    /// When the button then is pressed, all pieces of that type will return to home base, or back onto the table if they are already at home base. 
    /// This is used to make it easier for the player to get orriented with all the pieces. 
    /// Otherwise the table would get crowded and it would be hard to find the pieces when you have moved them around a lot.
    /// </summary>
    public void ResetPosition()
    {
        ZoomAreaScript.ZoomIn(null, false); // Call the ZoomIn function in the ZoomAreaScript and pass null and false to reset the zoom area

        Vector3 homeBaseRotation = new Vector3(0, 0, 90); // Store the rotation for the home base so it can be reset when it goes back to home base
        if (transform.position != HomeBase.transform.position && !isStuck)
        {
            lastKnownPosition = transform.position; // Store the current position before resetting
            transform.position = HomeBase.transform.position;
            isStuck = false;

            transform.rotation = Quaternion.Euler(homeBaseRotation); // Reset the rotation to the home base rotation

            // disable the collider so the player cant drag it from home base while in home base
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (isStuck)
        {
            return; // Exit the function if the piece is currently stuck to an article piece since it shouldn't be able to go back to home base while it's stuck to an article piece
        }
        else
        {
            // enable the collider so the player can drag it again when its on the table
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            transform.position = lastKnownPosition; // Move back to the last known position if it's already at home base

            transform.rotation = Quaternion.Euler(GetRandomRotation()); // Reset the rotation to a random rotation
        }
    }

    /// <summary>
    /// Simpel function used multiple times to get a semi-random rotation for the pieces.
    /// Used at the start, back from homebase and back from being zoomed in.
    /// This gives the game a more realistic and fluent feel instead of robust mathematical true orienations for the pieces. 
    /// <returns>A Vector3 representing a random rotation for the piece.</returns>
    Vector3 GetRandomRotation() // Small Vector3 function to get a random rotation eachtime its reset from the pile
    { return new Vector3(0, 0, Random.Range(-15f, 15f)); }
}

