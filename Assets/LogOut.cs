using App;
using Mono.Cecil.Cil;
using UnityEngine;
// using Client = Supabase.Client;

public class LogOut : MonoBehaviour
{
    [SerializeField] CanvasGroup logOutCanvasGroup;

    // private static Client _supabase;

    void Start()
    {
        var user = SupabaseStuff.Instance.GetLoggedInUser();
        
        if(user != null && SupabaseStuff.Instance != null) {
            logOutCanvasGroup.alpha = 1f;
            logOutCanvasGroup.interactable = true;
        } else {
            logOutCanvasGroup.alpha = 0f;
            logOutCanvasGroup.interactable = false;
        }
    }
}
