using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Newtonsoft.Json;
using Postgrest.Responses;
using Supabase.Gotrue;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Client = Supabase.Client;
using RequestException = Postgrest.RequestException;

namespace App {
    public class SupabaseStuff : MonoBehaviour {
        public static SupabaseStuff Instance { get; private set;}
        private const string SUPABASE_URL = "https://zvudtnmbgjsxhtgylxyw.supabase.co";
        private const string SUPABASE_PUBLIC_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inp2dWR0bm1iZ2pzeGh0Z3lseHl3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDcyMjE0MDIsImV4cCI6MjA2Mjc5NzQwMn0.nDTbbmOZwB8pw9xbZXYGHI8WBEJov3uxC6B7LO11Bvc";
        private string SUPABASE_USER_ACCESS_TOKEN;
        // public TMP_InputField result;
        public TMP_InputField email;
        public TMP_InputField password;

        private Client _supabase;
        // private string _id;
        // private string _nonce;

        private async void Start() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);


            if (_supabase == null) {
                _supabase = new Client(SUPABASE_URL, SUPABASE_PUBLIC_KEY);
                _ = await _supabase.InitializeAsync();
                Debug.Log("supabase intiated");
                PlayerPrefs.DeleteAll();
            }
        }

        private async void RpcCall() {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["name"] = "howdy";

            Task<BaseResponse> rpc = _supabase.Rpc("hello_js_as_json", param);
            rpc.AsUniTask().GetAwaiter().OnCompleted(Complete);
            try {
                _ = await rpc;
            } catch (RequestException requestException) {
                Debug.Log($"{requestException.Message}") ;
                Debug.Log($"\n{requestException.Error}") ;

                string content = await requestException.Response.Content.ReadAsStringAsync();

                Debug.Log($"\nResponse {content}");
            } catch (Exception e) {
                Debug.Log("RPC failed") ;
                Debug.Log($"{e.Message}") ;
                Debug.Log($"\n{e.StackTrace}") ;
            }

            if (rpc.IsCompleted) {
                Debug.Log(rpc.Result.Content);
            } else {
                Debug.Log($"{rpc.Status}");
            }
        }

        private void Complete() {
            Debug.Log("Complete") ;
        }

        public void StartPublic() {
            Debug.Log("...");
            RpcCall();
        }
        
        public Client GetSupabaseClient() {
            return _supabase;
        }

        public async UniTask LoginBackend(string email, string password)
        {
            string url = $"{GetURL()}/auth/v1/token?grant_type=password";
            string jsonData = $"{{\"email\": \"{email}\", \"password\": \"{password}\"}}";

            using UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apikey", GetAPIKey());
            webRequest.SetRequestHeader("Authorization", $"Bearer {GetAPIKey()}");

            _ = await webRequest.SendWebRequest().ToUniTask();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                PlayerPrefs.SetString("logged_in_user", response);
                PlayerPrefs.Save();

                Debug.Log("berhasil login user");
            }
            else
            {
                Debug.LogError("gk berhasil login pake restful");
            }
        }

        // public async Task<User> GetLoggedInUser() {
        //     string url = $"{GetURL()}/auth/v1/user";
        //     string acToken = PlayerPrefs.GetString("access_token");

        //     if (string.IsNullOrEmpty(acToken)) {
        //         Debug.LogError("AcToken kosong");
        //         return null;
        //     }

        //     using UnityWebRequest webRequest = new UnityWebRequest(url, "GET") {
        //         downloadHandler = new DownloadHandlerBuffer(),
        //     };

        //     webRequest.SetRequestHeader("Authorization", $"Bearer {acToken}");
        //     webRequest.SetRequestHeader("apikey", GetAPIKey());

        //     _ = await webRequest.SendWebRequest().ToUniTask();

        //     if (webRequest.result == UnityWebRequest.Result.Success) {
        //         string jsonResponse = webRequest.downloadHandler.text;
        //         Debug.LogWarning("Raw JSON Response GetLoggedInUser(): " + jsonResponse);

        //         try {
        //             var resp = JsonConvert.DeserializeObject<AuthResponse>(jsonResponse);

        //             if (resp?.User != null) {
        //                 Debug.Log("Player ID dari response: " + resp.User.UserMetadata?.Username);
        //                 return resp.User;
        //             } else {
        //                 Debug.LogError("Gagal mendapatkan user dari JSON response.");
        //             }
        //         } catch (Exception e) {
        //             Debug.LogError("Error parsing JSON: " + e.Message);
        //         }
        //     } else {
        //         Debug.LogError("Gagal ambil user login, HTTP error: " + webRequest.error);
        //     }

        //     return null;
        // }

        private AuthResponse GetAuthResponse() {
            string userJson = PlayerPrefs.GetString("logged_in_user", "");
            if (string.IsNullOrEmpty(userJson)) return null;

            try {
                var response = JsonConvert.DeserializeObject<AuthResponse>(userJson);
                return response;
            } catch (Exception e) {
                Debug.LogError("Error parsing logged-in user JSON: " + e.Message);
                return null;
            }
        }

        public User GetLoggedInUser() {
            return GetAuthResponse()?.User;
        }

        public string GetLoggedInUserACT() {
            return GetAuthResponse()?.AccessToken;
        }
        
        public bool CheckLoggedInUser() {
            var user = GetLoggedInUser();
            if (user != null) {
                Debug.Log($"User is logged in: {user.Email}");
                return true;
            } else {
                Debug.Log("No user is logged in.");
                return false;
            }
        }


    //     public void ManualAppleSignIn() {
    //         Debug.Log() "starting supabase apple sign in\n";
    //         AppleSignIn(_id);
    //         Debug.Log() "\nawaited supabase apple sign in";
    //     }

    //     // ReSharper disable Unity.PerformanceAnalysis
    //     private async void AppleSignIn(string identityToken) {
    //         if (identityToken == null) {
    //             Debug.Log() "Null identity token\n";
    //             return;
    //         }

    //         Debug.Log() "Starting supabase auth attempt\n";

    //         Task<Session> t = null;
    //         try {
    //             Debug.Log = $"{identityToken.Length}";

    //             _supabase.Auth.GetHeaders!()["apikey"] = SUPABASE_PUBLIC_KEY;
    //             _supabase.Auth.GetHeaders!()["Authorization"] = $"Bearer ${SUPABASE_PUBLIC_KEY}";

    //             if (!string.IsNullOrEmpty(_nonce)) {
    //                 Debug.Log() $"signing in with nonce {_nonce}";
    //                 t = _supabase.Auth.SignInWithIdToken(Constants.Provider.Apple, identityToken, _nonce);
    //             } else {
    //                 Debug.Log() "signing in without nonce";
    //                 t = _supabase.Auth.SignInWithIdToken(Constants.Provider.Apple, identityToken);
    //             }

    //             await t;
    //         } catch (UnauthorizedException e) {
    //             Debug.Log = "Exception with SignInWithIdToken";
    //             Debug.Log = $"Used nonce {_nonce}";
    //             Debug.Log() $"\n Exception {e.Message}";
    //             Debug.Log() $"\n {e.Content}";
    //         } catch (BadRequestException e) {
    //             Debug.Log() "\n";
    //             Debug.Log() e.Content;
    //         } catch (Exception e) {
    //             Debug.Log = "Unknown Exception with SignInWithIdToken";
    //             Debug.Log() $"\n Exception {e.Message}";
    //             Debug.Log() $"\n {e.StackTrace}";
    //         }

    //         if (t?.IsCompletedSuccessfully == true) {
    //             Debug.Log() $"\nsupabase login success\n {t.Result?.User?.Id}";
    //         } else {
    //             Debug.Log() $"\nsupabase failure\n {t?.Exception}";
    //         }
    //     }

    //     public void AssignId(string identityToken, string nonce, string nonceHash) {
    //         _id = identityToken;
    //         _nonce = nonce;
    //     }

            public string GetURL(){
                return SUPABASE_URL;
            }

            public string GetAPIKey() {
                return SUPABASE_PUBLIC_KEY;
            }

        internal void ClearUserSession()
        {
            PlayerPrefs.DeleteAll();
        }

        public class AuthResponse {
                [JsonProperty("access_token")]
                public string AccessToken { get; set; }

                [JsonProperty("token_type")]
                public string TokenType { get; set; }

                [JsonProperty("expires_in")]
                public int ExpiresIn { get; set; }

                [JsonProperty("expires_at")]
                public long ExpiresAt { get; set; }

                [JsonProperty("refresh_token")]
                public string RefreshToken { get; set; }

                [JsonProperty("user")]
                public User User { get; set; }
            }

            public class User {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("email")]
                public string Email { get; set; }

                [JsonProperty("user_metadata")]
                public UserMetadata UserMetadata { get; set; }
            }

            public class UserMetadata {
                [JsonProperty("username")]
                public string Username { get; set; }
            }
        }
}
