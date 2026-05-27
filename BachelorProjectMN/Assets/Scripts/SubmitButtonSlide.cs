using System.Collections;
using UnityEngine;

public class SubmitButtonSlide : MonoBehaviour
{
    public float duration = 1f;
    public float slideLength = 5f;
    private Vector3 target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SlideInFrame", 0);
        target = transform.position;
        target.x = target.x - slideLength;
    }



    private void SlideInFrame()
    {
        StartCoroutine(Slide());
    }

    IEnumerator Slide()
    {
        Vector3 start = transform.position;
        Vector3 end = target;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = 1f - Mathf.Pow(1f - t, 3f); // ease-out cubic
            transform.position = Vector3.Lerp(start, end, eased);
            yield return null;
        }
    }
}
