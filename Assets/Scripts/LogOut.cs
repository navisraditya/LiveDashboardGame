using App;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogOut : MonoBehaviour
{
    [SerializeField] CanvasGroup logOutCanvasGroup;
    // [SerializeField] Button logoutBtn;
    SupabaseStuff.User user;

    void Start()
    {
        user = SupabaseStuff.Instance.GetLoggedInUser();
        
        if (user != null) {
            logOutCanvasGroup.alpha = 1f; // Show
            logOutCanvasGroup.interactable = true;
            logOutCanvasGroup.blocksRaycasts = true;
        } else {
            logOutCanvasGroup.alpha = 0f; // Hide
            logOutCanvasGroup.interactable = false;
            logOutCanvasGroup.blocksRaycasts = false;
        }
    }

    public async void LogOutUser()
    {
        string url = $"{SupabaseStuff.Instance.GetURL()}/auth/v1/logout";

        using UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");
        request.SetRequestHeader("apikey", SupabaseStuff.Instance.GetAPIKey());
        request.SetRequestHeader("Authorization", $"Bearer {SupabaseStuff.Instance.GetLoggedInUserACT()}");

        _ = await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User logged out successfully.");
            SupabaseStuff.Instance.ClearUserSession();

            logOutCanvasGroup.alpha = 0f; // Hide after logout
            logOutCanvasGroup.interactable = false;
            logOutCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            Debug.LogError($"Logout failed: {request.error}");
            Debug.LogError($"Response Code: {request.responseCode}");
            Debug.LogError("Response Body: " + request.downloadHandler.text);
        }
    }
}
