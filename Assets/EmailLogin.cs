using UnityEngine;

public class EmailLogin : MonoBehaviour
{

    public CanvasGroup loginWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetActiveFalse();
    }


    public void SetActiveTrue() {
        if(loginWindow != null) {
            loginWindow.gameObject.SetActive(true);
        }
    }

    public void SetActiveFalse() {
        if(loginWindow != null){
            loginWindow.gameObject.SetActive(false);
        }
    }
    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
