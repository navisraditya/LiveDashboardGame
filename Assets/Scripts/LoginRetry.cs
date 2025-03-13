using UnityEngine;

public class LoginRetry : MonoBehaviour
{
    [SerializeField] CanvasGroup currWindow;
    [SerializeField] CanvasGroup loginCanvasGroup;
    
    public void RetryLogin() {
        loginCanvasGroup.gameObject.SetActive(true);
        currWindow.gameObject.SetActive(false);
        
    }
}
