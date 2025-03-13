using App;
using UnityEngine;
using UnityEngine.SceneManagement;
using Client = Supabase.Client;



public class logoutbutton : MonoBehaviour
{
    private Client _supabase;

    private void Awake()
    {
        _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

        if(_supabase == null){
            Debug.LogError("supabase kosong");
        }
    }

    public async void LogoutUser() {
        if(_supabase == null) {
            Debug.LogError("supabase kosong");
            return;
        }

        try {
            await _supabase.Auth.SignOut();
            Debug.Log("logout success");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } catch (System.Exception e) {
            Debug.LogError($"eror logout: {e.Message}");
        }
    }
}
