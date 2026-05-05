using UnityEngine;

public class ZoomAreaScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void ZoomIn(Sprite newSprite, bool needZoom)
    {
        if (needZoom)
        {
            transform.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
