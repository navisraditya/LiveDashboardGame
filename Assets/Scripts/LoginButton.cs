using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Client = Supabase.Client;

namespace App {
    public class LoginButton : MonoBehaviour {
        // public TMP_InputField result;
        public TMP_InputField email;
        public TMP_InputField password;

        private static Client _supabase;
        // private string _id;
        // private string _nonce;

        [SerializeField] CanvasGroup loginCanvasGroup;
        [SerializeField] CanvasGroup loginPopUpNo;
        [SerializeField] CanvasGroup loginPopUpYes;
        [SerializeField] CanvasGroup logOutBtn;

        private void Awake() {
            _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

            if(_supabase == null) {
                Debug.LogError("supabase kosong.");
            }
        }
        
        // buat di leaderboard
        public async void LoginClose() {
            await SupabaseStuff.Instance.LoginBackend(email.text, password.text);
            loginCanvasGroup.gameObject.SetActive(false);
            // SupabaseStuff.User user = SupabaseStuff.Instance.GetLoggedInUser();
            // Debug.LogError("ini di button LoginClose" + user.UserMetadata.Username.ToString());
            await ScoreManager.Instance.SaveScoreToSupabase();
            SceneManager.LoadScene("Leaderboard");
        }

        // buat di dashboard
        public async void LoginUser() {
            await SupabaseStuff.Instance.LoginBackend(email.text, password.text);
            _ = SupabaseStuff.Instance.GetLoggedInUser();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // buat di settings
        public async void LoginUserSetting(){
            await SupabaseStuff.Instance.LoginBackend(email.text, password.text);
            _ = SupabaseStuff.Instance.GetLoggedInUser();
            loginCanvasGroup.gameObject.SetActive(false);
            logOutBtn.gameObject.SetActive(true);
        }

        // buat kalau mau langsung ganti scene
        public async void LoginUser(string targetWindow) {
            await SupabaseStuff.Instance.LoginBackend(email.text, password.text);
            _ = SupabaseStuff.Instance.GetLoggedInUser();
            SceneManager.LoadScene(targetWindow);
        }

        // private async UniTask loginBackend() {
        //     if(_supabase == null) {
        //         Debug.LogError("supabase kosong_1");
        //         ErrorPopUp();
        //         return;
        //     }

        //     Debug.Log("starting sign in");
        //     Task<Session> signUp = _supabase.Auth.SignInWithPassword(email.text, password.text);

        //     try {
        //         await signUp;
        //     } catch (BadRequestException badRequestException) {
        //         Debug.Log("BadRequestException") ;
        //         Debug.Log($"{badRequestException.Message}") ;
        //         Debug.Log($"{badRequestException.Content}") ;
        //         Debug.Log($"{badRequestException.StackTrace}") ;
        //         ErrorPopUp();
        //     } catch (UnauthorizedException unauthorizedException) {
        //         Debug.Log("UnauthorizedException") ;
        //         Debug.Log(unauthorizedException.Message) ;
        //         Debug.Log(unauthorizedException.Content) ;
        //         Debug.Log(unauthorizedException.StackTrace) ;
        //         ErrorPopUp();
        //     } catch (ExistingUserException existingUserException) {
        //         Debug.Log("ExistingUserException") ;
        //         Debug.Log(existingUserException.Message) ;
        //         Debug.Log(existingUserException.Content) ;
        //         Debug.Log(existingUserException.StackTrace) ;
        //         ErrorPopUp();
        //     } catch (ForbiddenException forbiddenException) {
        //         Debug.Log("ForbiddenException") ;
        //         Debug.Log(forbiddenException.Message) ;
        //         Debug.Log(forbiddenException.Content) ;
        //         Debug.Log(forbiddenException.StackTrace) ;
        //         ErrorPopUp();
        //     // } catch (InvalidProviderException invalidProviderException) {
        //     //     Debug.Log() "invalidProviderException";
        //     //     Debug.Log() invalidProviderException.Message;
        //     //     Debug.Log() invalidProviderException.StackTrace;
        //     //     return;
        //     } catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException) {
        //         Debug.Log("invalidEmailOrPasswordException") ;
        //         Debug.Log(invalidEmailOrPasswordException.Message) ;
        //         Debug.Log(invalidEmailOrPasswordException.Content) ;
        //         Debug.Log(invalidEmailOrPasswordException.StackTrace) ;
        //         ErrorPopUp();
        //     } catch (Exception exception) {
        //         Debug.Log("unknown exception") ;
        //         Debug.Log(exception.Message) ;
        //         Debug.Log(exception.StackTrace) ;
        //         ErrorPopUp();
        //     }

        //     if (!signUp.IsCompletedSuccessfully) {
        //         Debug.Log(JsonUtility.ToJson(signUp.Exception));
        //         ErrorPopUp();
        //         return;
        //     }

        //     Session session = signUp.Result;

        //     if (session == null) {
        //         Debug.Log("nope");
        //     } else {
        //         if(loginPopUpYes != null) {
        //             loginPopUpYes.gameObject.SetActive(true);
        //         }
        //         Debug.Log($"Sign in success {session.User?.Id} {session.AccessToken} {session.User?.Aud} {session.User?.Email} {session.RefreshToken}");

        //         Timer.Instance.BeginCouting(3);

        //         if(Timer.Instance.isCounting) {
        //             if(loginPopUpYes != null) {
        //                 loginPopUpYes.gameObject.SetActive(false);
        //             }
        //             if(loginCanvasGroup != null){
        //                 loginCanvasGroup.gameObject.SetActive(false);
        //             }
        //         }
        //     }
        // }

        // private async UniTask LoginBackend()
        // {
        //     string url = $"{SupabaseStuff.Instance.GetURL()}/auth/v1/token?grant_type=password";
        //     string jsonData = $"{{\"email\": \"{email.text}\", \"password\": \"{password.text}\"}}";

        //     using UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        //     byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        //     webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        //     webRequest.downloadHandler = new DownloadHandlerBuffer();
        //     webRequest.SetRequestHeader("Content-Type", "application/json");
        //     webRequest.SetRequestHeader("apikey", SupabaseStuff.Instance.GetAPIKey());
        //     webRequest.SetRequestHeader("Authorization", $"Bearer {SupabaseStuff.Instance.GetAPIKey()}");

        //     await webRequest.SendWebRequest().ToUniTask();

        //     if (webRequest.result == UnityWebRequest.Result.Success)
        //     {
        //         Debug.Log("berhasil login: pake restful" + webRequest.downloadHandler.text);
        //     }
        //     else
        //     {
        //         Debug.LogError("gk berhasil login pake restful");
        //     }
        // }

        private void ErrorPopUp() {
            if(loginPopUpNo != null) {
                loginPopUpNo.gameObject.SetActive(true);
            }
            email.text = "";
            password.text = "";
            // loginCanvasGroup.gameObject.SetActive(false);
        }
    }
}