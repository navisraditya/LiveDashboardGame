using System;
using App;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] CanvasGroup loginCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(SupabaseStuff.Instance == null) {
            Login();
        } else {
            bool isUserLoggedIn = SupabaseStuff.Instance.CheckLoggedInUser();
            if (!isUserLoggedIn){
                Login();
            }
        }
    }

    private void Login(){
        loginCanvas.gameObject.SetActive(true);
        SettingWindowManager.currWindow = loginCanvas;
    }
}
