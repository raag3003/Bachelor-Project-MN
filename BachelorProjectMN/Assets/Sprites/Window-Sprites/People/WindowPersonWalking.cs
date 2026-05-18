using UnityEngine;

public class WindowPersonWalking : MonoBehaviour
{
    Animator animator;
    GameObject leftSpawn;
    GameObject rightSpawn;
    public bool walkingDirection = true;
    private float moveSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        leftSpawn = GameObject.FindGameObjectWithTag("WindowBorderLeft");
        rightSpawn = GameObject.FindGameObjectWithTag("WindowBorderRight");

        moveSpeed = Random.Range(0.005f, 0.008f);

        if (!walkingDirection)
        {
            moveSpeed = moveSpeed * -1;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WindowBorderRight") && walkingDirection)
        {
            Vector3 newPos = transform.position;
            newPos.x = leftSpawn.transform.position.x;
            transform.position = newPos;
            Debug.Log("Person reached right border!");
        }
        if (other.CompareTag("WindowBorderLeft") && !walkingDirection)
        {
            Vector3 newPos = transform.position;
            newPos.x = rightSpawn.transform.position.x;
            transform.position = newPos;
            Debug.Log("Person reached left border!");
        }
    }
}
