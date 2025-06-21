using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody2D rb;

    private float moveX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal")  * moveSpeed;

        if (moveX > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        } else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveX;
        rb.linearVelocity = velocity;

        Vector2 position = rb.position;
        position.x = Mathf.Clamp(position.x, -2.5f, 2.5f);
        rb.position = position;
    }
}
