using UnityEngine;

public class SettingWindowManager : MonoBehaviour
{

    public CanvasGroup targetWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetActiveFalse();
    }


    public void SetActiveTrue() {
        if(targetWindow != null) {
            targetWindow.gameObject.SetActive(true);
        }
    }

    public void SetActiveFalse() {
        if(targetWindow != null){
            targetWindow.gameObject.SetActive(false);
        }
    }
    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
