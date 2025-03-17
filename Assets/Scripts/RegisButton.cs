using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Client = Supabase.Client;

namespace App {
    public class RegisButton : MonoBehaviour {
        // public TMP_InputField result;
        private static Client _supabase;
        // private string _id;
        // private string _nonce;

        [SerializeField] CanvasGroup regisCanvasGroup;
        [SerializeField] CanvasGroup loginCanvasGroup;
        [SerializeField] TMP_InputField email;
        [SerializeField] TMP_InputField password;
        [SerializeField] TMP_InputField username;
        [SerializeField] CanvasGroup regisPopUpNo;
        [SerializeField] CanvasGroup regisPopUpYes;

        private void Awake() {
            _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

            if(_supabase == null) {
                Debug.LogError("supabase kosong.");
            }
        }

        // public async void RegisterUser() {
        //     if(_supabase == null) {
        //         Debug.LogError("supabase kosong_1");
        //         ErrorPopUp();
        //     }

        //     Debug.Log("starting sign up");

        //     var options = new SignUpOptions {
        //         Data = new Dictionary<string, object> {
        //             {"username", username.text}
        //         }
        //     };
        //     Task<Session> signUp = _supabase.Auth.SignUp(email.text, password.text, options);
        //     try {
        //         _ = await signUp;
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

        //     Session session = signUp.Result;

        //     Debug.Log($"Supabase sign in user id: {session?.User?.Id}; Username: {username.text};");
        // }

        public async void RegisterUser() {
            if(email.text == null || password.text == null || username.text == null) {
                ErrorPopUp();
            }
            
            string url = $"{SupabaseStuff.Instance.GetURL()}/auth/v1/signup";
            var payload = new Dictionary<string, object> {
                {"email", email.text},
                {"password", password.text},
                { "data", new Dictionary<string, string> { { "username", username.text } } }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);

            using UnityWebRequest request = new(url, "POST") {
                uploadHandler = new UploadHandlerRaw(jsonBytes),
                downloadHandler = new DownloadHandlerBuffer(),
            };

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apikey", SupabaseStuff.Instance.GetAPIKey());

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"User registered successfully: {request.downloadHandler.text}");
                // Store user session if needed
            }
            else
            {
                Debug.LogError($"Signup failed: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError("Response Body: " + request.downloadHandler.text);
                ErrorPopUp();
            }
        }

        public void RegisLogin()
        {
            RegisterUser();
            regisPopUpYes.gameObject.SetActive(true);
            
            Timer.Instance.BeginCouting(3);
            if(Timer.Instance.isCounting) {
                regisCanvasGroup.gameObject.SetActive(false);
            }
            Timer.Instance.BeginCouting(3);
            if(Timer.Instance.isCounting) {
                loginCanvasGroup.gameObject.SetActive(true);
            }

        }

        private void ErrorPopUp() {
            regisPopUpNo.gameObject.SetActive(true);
            email.text = "";
            password.text = "";
            username.text = "";
        }
    }
}