using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static Platform Instance { get; private set; }
    public float jumpForce = 10f;
    public ScoreManager scoreManager;
    public GameManager gameManager;
    public int score = 1;
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
                    if (gameManager.currLevel == 1)
                    {
                        scoreManager.IncScore();
                    }
                    else if (gameManager.currLevel == 2)
                    {
                        scoreManager.IncScore(5);
                    }
                    else
                    {
                        scoreManager.IncScore(10);
                    }
                    SoundPrefab.Instance.PlaySFX(SFX.Jump);
                    print(gameManager.latestPlatformIdx);
                    Destroy(gameObject);
                }
            }
        }
    }
}
