using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Postgrest.Responses;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using Client = Supabase.Client;
using RequestException = Postgrest.RequestException;

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

        private void Awake() {
            _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

            if(_supabase == null) {
                Debug.LogError("supabase kosong.");
            }
        }

        public async void RegisterUser() {
            if(_supabase == null) {
                Debug.LogError("supabase kosong_1");
                regisCanvasGroup.gameObject.SetActive(false);
            }

            Debug.Log("starting sign up");

            var options = new SignUpOptions {
                Data = new Dictionary<string, object> {
                    {"username", username.text}
                }
            };
            Task<Session> signUp = _supabase.Auth.SignUp(email.text, password.text, options);
            try {
                await signUp;
            } catch (BadRequestException badRequestException) {
                Debug.Log("BadRequestException") ;
                Debug.Log($"{badRequestException.Message}") ;
                Debug.Log($"{badRequestException.Content}") ;
                Debug.Log($"{badRequestException.StackTrace}") ;
                return;
            } catch (UnauthorizedException unauthorizedException) {
                Debug.Log("UnauthorizedException") ;
                Debug.Log(unauthorizedException.Message) ;
                Debug.Log(unauthorizedException.Content) ;
                Debug.Log(unauthorizedException.StackTrace) ;
                return;
            } catch (ExistingUserException existingUserException) {
                Debug.Log("ExistingUserException") ;
                Debug.Log(existingUserException.Message) ;
                Debug.Log(existingUserException.Content) ;
                Debug.Log(existingUserException.StackTrace) ;
                return;
            } catch (ForbiddenException forbiddenException) {
                Debug.Log("ForbiddenException") ;
                Debug.Log(forbiddenException.Message) ;
                Debug.Log(forbiddenException.Content) ;
                Debug.Log(forbiddenException.StackTrace) ;
                return;
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
                return;
            } catch (Exception exception) {
                Debug.Log("unknown exception") ;
                Debug.Log(exception.Message) ;
                Debug.Log(exception.StackTrace) ;
                return;
            }

            Session session = signUp.Result;

            Debug.Log($"Supabase sign in user id: {session?.User?.Id}; Username: {username.text};");
        }

        public void RegisLogin()
        {
            RegisterUser();
            loginCanvasGroup.gameObject.SetActive(true);
        }
    }
}