using App;
using Supabase.Gotrue;
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
            bool isUserLoggedIn = true;
            _ = SupabaseStuff.Instance.GetLoggedInUser();
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
