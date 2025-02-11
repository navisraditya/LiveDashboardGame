using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public SceneLoader sceneLoader;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if(rb != null && collision.gameObject.CompareTag("Player")) {
            sceneLoader.LoadScene("Leaderboard");
        }
    }
}
