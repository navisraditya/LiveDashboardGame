// using System;
// using System.Threading.Tasks;
// using Cysharp.Threading.Tasks;
// using Supabase.Gotrue;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using Client = Supabase.Client;

// namespace App
// {
//     public class LoginButton : MonoBehaviour
//     {
//         [SerializeField] private TMP_InputField email;
//         [SerializeField] private TMP_InputField password;
//         [SerializeField] private CanvasGroup loginCanvasGroup;
//         [SerializeField] private CanvasGroup loginPopUpNo;
//         [SerializeField] private CanvasGroup loginPopUpYes;

//         private static Client _supabase;

//         private void Awake()
//         {
//             _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

//             if (_supabase == null)
//             {
//                 Debug.LogError("Supabase client is not initialized.");
//             }
//         }

//         public async void LoginClose()
//         {
//             await LoginBackend();
//             loginCanvasGroup.gameObject.SetActive(false);
//             await ScoreManager.Instance.SaveScoreToSupabase();
//             SceneManager.LoadScene("Leaderboard");
//         }

//         public async void LoginUser()
//         {
//             await LoginBackend();
//             SupabaseStuff.Instance.GetLoggedInUser();
//             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//         }

//         public async void LoginUser(string targetWindow)
//         {
//             await LoginBackend();
//             SupabaseStuff.Instance.GetLoggedInUser();
//             SceneManager.LoadScene(targetWindow);
//         }

//         private async UniTask LoginBackend()
//         {
//             if (_supabase == null)
//             {
//                 Debug.LogError("Supabase client is not initialized.");
//                 ShowErrorPopup();
//                 return;
//             }

//             if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text))
//             {
//                 Debug.LogError("Email or password is empty.");
//                 ShowErrorPopup();
//                 return;
//             }

//             Debug.Log("Starting sign in");

//             try
//             {
//                 var session = await _supabase.Auth.SignInWithPassword(email.text, password.text);

//                 if (session == null)
//                 {
//                     Debug.LogError("Sign in failed: Session is null.");
//                     ShowErrorPopup();
//                     return;
//                 }

//                 Debug.Log($"Sign in success: {session.User?.Id} {session.AccessToken} {session.User?.Aud} {session.User?.Email} {session.RefreshToken}");

//                 ShowSuccessPopup();
//                 await UniTask.Delay(3000); // Wait for 3 seconds

//                 if (loginPopUpYes != null)
//                 {
//                     loginPopUpYes.gameObject.SetActive(false);
//                 }

//                 if (loginCanvasGroup != null)
//                 {
//                     loginCanvasGroup.gameObject.SetActive(false);
//                 }
//             }
//             catch (BadRequestException ex)
//             {
//                 Debug.LogError($"BadRequestException: {ex.Message}");
//                 ShowErrorPopup();
//             }
//             catch (UnauthorizedException ex)
//             {
//                 Debug.LogError($"UnauthorizedException: {ex.Message}");
//                 ShowErrorPopup();
//             }
//             catch (ExistingUserException ex)
//             {
//                 Debug.LogError($"ExistingUserException: {ex.Message}");
//                 ShowErrorPopup();
//             }
//             catch (ForbiddenException ex)
//             {
//                 Debug.LogError($"ForbiddenException: {ex.Message}");
//                 ShowErrorPopup();
//             }
//             catch (InvalidEmailOrPasswordException ex)
//             {
//                 Debug.LogError($"InvalidEmailOrPasswordException: {ex.Message}");
//                 ShowErrorPopup();
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"Unknown exception: {ex.Message}");
//                 ShowErrorPopup();
//             }
//         }

//         private void ShowErrorPopup()
//         {
//             if (loginPopUpNo != null)
//             {
//                 loginPopUpNo.gameObject.SetActive(true);
//             }

//             email.text = "";
//             password.text = "";
//         }

//         private void ShowSuccessPopup()
//         {
//             if (loginPopUpYes != null)
//             {
//                 loginPopUpYes.gameObject.SetActive(true);
//             }
//         }
//     }
// }


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace App
{
    public class LoginButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private CanvasGroup loginCanvasGroup;
        [SerializeField] private CanvasGroup loginPopUpNo;
        [SerializeField] private CanvasGroup loginPopUpYes;

        private const string SUPABASE_URL = "https://rbmxqlqzyemtwsajfjtw.supabase.co";
        private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJibXhxbHF6eWVtdHdzYWpmanR3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzcwMDgyNjcsImV4cCI6MjA1MjU4NDI2N30.N2-ULM2_1zc_yCo3zoYlolIZhX8OPnixsILHqhZxTO8";

        private void Awake()
        {
            // No need to initialize Supabase client here
        }

        public async void LoginClose()
        {
            await LoginBackend();
            loginCanvasGroup.gameObject.SetActive(false);
            await ScoreManager.Instance.SaveScoreToSupabase();
            SceneManager.LoadScene("Leaderboard");
        }

        public async void LoginUser()
        {
            await LoginBackend();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public async void LoginUser(string targetWindow)
        {
            await LoginBackend();
            SceneManager.LoadScene(targetWindow);
        }

        private async UniTask LoginBackend()
        {
            if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text))
            {
                Debug.LogError("Email or password is empty.");
                ShowErrorPopup();
                return;
            }

            Debug.Log("Starting sign in");

            string url = $"{SUPABASE_URL}/auth/v1/token?grant_type=password";
            string jsonData = $"{{\"email\":\"{email.text}\",\"password\":\"{password.text}\"}}";

            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);

                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error logging in: " + webRequest.error);
                    Debug.LogError("Response: " + webRequest.downloadHandler.text);
                    ShowErrorPopup();
                }
                else
                {
                    var response = JsonUtility.FromJson<LoginResponse>(webRequest.downloadHandler.text);
                    Debug.Log($"Sign in success: {response.user?.id} {response.access_token}");

                    // Store the access token and user metadata for future authenticated requests
                    PlayerPrefs.SetString("supabase_access_token", response.access_token);
                    PlayerPrefs.SetString("supabase_user_id", response.user.id);
                    PlayerPrefs.SetString("supabase_user_email", response.user.email);

                    // Store user metadata (e.g., username) if available
                    if (response.user.user_metadata != null && response.user.user_metadata.ContainsKey("username"))
                    {
                        PlayerPrefs.SetString("supabase_user_username", response.user.user_metadata["username"].ToString());
                    }

                    // Initialize Supabase client with the logged-in user
                    SupabaseStuff.Instance?.InitializeSupabaseClient(response.access_token);

                    ShowSuccessPopup();

                    // Clear input fields for security
                    email.text = "";
                    password.text = "";
                }
            }
        }

        private void ShowErrorPopup()
        {
            if (loginPopUpNo != null)
            {
                loginPopUpNo.gameObject.SetActive(true);
            }

            email.text = "";
            password.text = "";
        }

        private void ShowSuccessPopup()
        {
            if (loginPopUpYes != null)
            {
                loginPopUpYes.gameObject.SetActive(true);
            }
        }

        [System.Serializable]
        private class LoginResponse
        {
            public string access_token;
            public string token_type;
            public int expires_in;
            public string refresh_token;
            public User user;
        }

        [System.Serializable]
        private class User
        {
            public string id;
            public string email;
            public Dictionary<string, object> user_metadata;
        }
    }
}