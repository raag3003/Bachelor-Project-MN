using UnityEngine;
using UnityEngine.UI;

public class ZoomAreaScript : MonoBehaviour
{
    public Text factsText;

    public GameObject background; // background for fact box

    [HideInInspector]
    public string currentZoomedObjectTag; // Store the tag of the currently zoomed object 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        factsText.text = null;
        transform.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void ZoomIn(Sprite newSprite, bool needZoom, string facts)
    {
        
        if (needZoom) // If needZoom is true, set the sprite to newSprite
        {
            
            factsText.text = facts; // Update the facts text with the provided facts
            transform.GetComponent<SpriteRenderer>().sprite = newSprite;
            if (!string.IsNullOrEmpty(facts))
            {
                background.SetActive(true); // turns on factbox background when see fact
            }
            else
            {
                background.SetActive(false);
            }
            
        }
        else // Reset everthing to the default state if needZoom is false
        {
            background.SetActive(false); // turns off factbox background when not see fact
            currentZoomedObjectTag = null;
            factsText.text = null;
            transform.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
