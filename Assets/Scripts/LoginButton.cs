using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Postgrest.Responses;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Client = Supabase.Client;
using RequestException = Postgrest.RequestException;

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

        private void Awake() {
            _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

            if(_supabase == null) {
                Debug.LogError("supabase kosong.");
            }
        }
        
        public async void LoginClose() {
            await loginBackend();
            loginCanvasGroup.gameObject.SetActive(false);
            await ScoreManager.Instance.SaveScoreToSupabase();
            SceneManager.LoadScene("Leaderboard");
        }
        public async void LoginUser() {
            await loginBackend();
            SupabaseStuff.Instance.GetLoggedInUser();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public async void LoginUser(string targetWindow) {
            await loginBackend();
            SupabaseStuff.Instance.GetLoggedInUser();
            SceneManager.LoadScene(targetWindow);
        }
        
        private async Task loginBackend() {
            if(_supabase == null) {
                Debug.LogError("supabase kosong_1");
                ErrorPopUp();
            }

            Debug.Log("starting sign in");
            Task<Session> signUp = _supabase.Auth.SignInWithPassword(email.text, password.text);
            try {
                await signUp;
            } catch (BadRequestException badRequestException) {
                Debug.Log("BadRequestException") ;
                Debug.Log($"{badRequestException.Message}") ;
                Debug.Log($"{badRequestException.Content}") ;
                Debug.Log($"{badRequestException.StackTrace}") ;
                ErrorPopUp();
            } catch (UnauthorizedException unauthorizedException) {
                Debug.Log("UnauthorizedException") ;
                Debug.Log(unauthorizedException.Message) ;
                Debug.Log(unauthorizedException.Content) ;
                Debug.Log(unauthorizedException.StackTrace) ;
                ErrorPopUp();
            } catch (ExistingUserException existingUserException) {
                Debug.Log("ExistingUserException") ;
                Debug.Log(existingUserException.Message) ;
                Debug.Log(existingUserException.Content) ;
                Debug.Log(existingUserException.StackTrace) ;
                ErrorPopUp();
            } catch (ForbiddenException forbiddenException) {
                Debug.Log("ForbiddenException") ;
                Debug.Log(forbiddenException.Message) ;
                Debug.Log(forbiddenException.Content) ;
                Debug.Log(forbiddenException.StackTrace) ;
                ErrorPopUp();
            // } catch (InvalidProviderException invalidProviderException) {
            //     Debug.Log() "invalidProviderException";
            //     Debug.Log() invalidProviderException.Message;
            //     Debug.Log() invalidProviderException.StackTrace;
            //     return;
            } catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException) {
                Debug.Log("invalidEmailOrPasswordException") ;
                Debug.Log(invalidEmailOrPasswordException.Message) ;
                Debug.Log(invalidEmailOrPasswordException.Content) ;
                Debug.Log(invalidEmailOrPasswordException.StackTrace) ;
                ErrorPopUp();
            } catch (Exception exception) {
                Debug.Log("unknown exception") ;
                Debug.Log(exception.Message) ;
                Debug.Log(exception.StackTrace) ;
                ErrorPopUp();
            }

            if (!signUp.IsCompletedSuccessfully) {
                Debug.Log(JsonUtility.ToJson(signUp.Exception));
                ErrorPopUp();
            }

            Session session = signUp.Result;

            if (session == null)
                Debug.Log( "nope");
            else {
                if(loginPopUpYes != null) {
                    loginPopUpYes.gameObject.SetActive(true);
                }
                Debug.Log($"Sign in success {session.User?.Id} {session.AccessToken} {session.User?.Aud} {session.User?.Email} {session.RefreshToken}");
                
                Timer.Instance.BeginCouting(3);
                
                if(Timer.Instance.isCounting) {
                    if(loginPopUpYes != null) {
                        loginPopUpYes.gameObject.SetActive(false);
                    }
                    if(loginCanvasGroup != null){
                        loginCanvasGroup.gameObject.SetActive(false);
                    }
                }
                
            }
        }

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