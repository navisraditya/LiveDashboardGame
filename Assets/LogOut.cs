using App;
using Mono.Cecil.Cil;
using UnityEngine;
using Client = Supabase.Client;

public class LogOut : MonoBehaviour
{
    [SerializeField] CanvasGroup logOutCanvasGroup;

    private static Client _supabase;

    void Start()
    {
        var user = SupabaseStuff.Instance.GetLoggedInUser();
        
        if(user != null && _supabase != null) {
            logOutCanvasGroup.gameObject.SetActive(true);
        } else {
            logOutCanvasGroup.gameObject.SetActive(false);
        }
    }
}
