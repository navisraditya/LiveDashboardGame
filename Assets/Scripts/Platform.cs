using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static Platform Instance { get; private set; }
    public float jumpForce = 10f;
    public ScoreManager scoreManager;
    public GameManager gameManager;

    private int index;

    public void SetIndex(int number)
    {
        index = number;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 velocity = rb.linearVelocity;
                velocity.y = jumpForce;
                rb.linearVelocity = velocity;

                if (this.gameObject.name != "Platform")
                {
                    // scoreManager.IncScore();
                    gameManager.latestPlatformIdx = index;
                    print(index);
                    Destroy(gameObject);
                }
            }
        }
    }
}
