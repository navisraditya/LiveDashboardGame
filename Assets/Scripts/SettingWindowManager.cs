using UnityEngine;

public class SettingWindowManager : MonoBehaviour
{

    [SerializeField] CanvasGroup targetWindow;
    public static CanvasGroup currWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetActiveTrue() {
        if(targetWindow != null) {
            targetWindow.gameObject.SetActive(true);
        }
    }

    public void SetActiveTrueCloseCurr() {
        if(targetWindow != null) {
            targetWindow.gameObject.SetActive(true);
            currWindow.gameObject.SetActive(false);
            currWindow = targetWindow;
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
