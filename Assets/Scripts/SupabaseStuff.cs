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
    public class SupabaseStuff : MonoBehaviour {
    public const string SUPABASE_URL = "https://[PROJECT_ID].supabase.co";

        public const string SUPABASE_PUBLIC_KEY =
            "the anon public key from your supabase project";

        public TMP_InputField result;
        public TMP_InputField email;
        public TMP_InputField password;

        private static Client _supabase;
        private string _id;
        private string _nonce;

        private async void Start() {
            if (_supabase == null) {
                _supabase = new Client(SUPABASE_URL, SUPABASE_PUBLIC_KEY);
                await _supabase.InitializeAsync();
            }
        }

        public async void RegisterUser() {
            Task<Session> signUp = _supabase.Auth.SignUp(email.text, password.text);
            try {
                await signUp;
            } catch (BadRequestException badRequestException) {
                result.text += "BadRequestException";
                result.text += $"{badRequestException.Message}";
                result.text += $"{badRequestException.Content}";
                result.text += $"{badRequestException.StackTrace}";
                return;
            } catch (UnauthorizedException unauthorizedException) {
                result.text += "UnauthorizedException";
                result.text += unauthorizedException.Message;
                result.text += unauthorizedException.Content;
                result.text += unauthorizedException.StackTrace;
                return;
            } catch (ExistingUserException existingUserException) {
                result.text += "ExistingUserException";
                result.text += existingUserException.Message;
                result.text += existingUserException.Content;
                result.text += existingUserException.StackTrace;
                return;
            } catch (ForbiddenException forbiddenException) {
                result.text += "ForbiddenException";
                result.text += forbiddenException.Message;
                result.text += forbiddenException.Content;
                result.text += forbiddenException.StackTrace;
                return;
            // } catch (InvalidProviderException invalidProviderException) {
            //     result.text += "invalidProviderException";
            //     result.text += invalidProviderException.Message;
            //     result.text += invalidProviderException.StackTrace;
            //     return;
            } catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException) {
                result.text += "invalidEmailOrPasswordException";
                result.text += invalidEmailOrPasswordException.Message;
                result.text += invalidEmailOrPasswordException.Content;
                result.text += invalidEmailOrPasswordException.StackTrace;
                return;
            } catch (Exception exception) {
                result.text += "unknown exception";
                result.text += exception.Message;
                result.text += exception.StackTrace;
                return;
            }

            Session session = signUp.Result;

            result.text = $"Supabase sign in user id: {session?.User?.Id}";
        }

        public async void LogInUser() {
            Debug.Log("starting sign up");
            Task<Session> signUp = _supabase.Auth.SignInWithPassword(email.text, password.text);
            try {
                await signUp;
            } catch (BadRequestException badRequestException) {
                result.text = "\nBadRequestException";
                result.text += $"\n{badRequestException.Message}";
                result.text += $"\n{badRequestException.Content}";
                result.text += $"\n{badRequestException.StackTrace}";
                return;
            } catch (UnauthorizedException unauthorizedException) {
                result.text += "UnauthorizedException";
                result.text += unauthorizedException.Message;
                result.text += unauthorizedException.Content;
                result.text += unauthorizedException.StackTrace;
                return;
            } catch (ExistingUserException existingUserException) {
                result.text += "ExistingUserException";
                result.text += existingUserException.Message;
                result.text += existingUserException.Content;
                result.text += existingUserException.StackTrace;
                return;
            } catch (ForbiddenException forbiddenException) {
                result.text += "ForbiddenException";
                result.text += forbiddenException.Message;
                result.text += forbiddenException.Content;
                result.text += forbiddenException.StackTrace;
                return;
            // } catch (InvalidProviderException invalidProviderException) {
            //     result.text += "invalidProviderException";
            //     result.text += invalidProviderException.Message;
            //     result.text += invalidProviderException.StackTrace;
            //     return;
            } catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException) {
                result.text += "invalidEmailOrPasswordException";
                result.text += invalidEmailOrPasswordException.Message;
                result.text += invalidEmailOrPasswordException.Content;
                result.text += invalidEmailOrPasswordException.StackTrace;
                return;
            } catch (Exception exception) {
                result.text += "unknown exception";
                result.text += exception.Message;
                result.text += exception.StackTrace;
                return;
            }

            if (!signUp.IsCompletedSuccessfully) {
                result.text += JsonUtility.ToJson(signUp.Exception);
                return;
            }

            Session session = signUp.Result;

            if (session == null)
                result.text += "nope";
            else {
                result.text =
                    $"Sign in success {session.User?.Id} {session.AccessToken} {session.User?.Aud} {session.User?.Email} {session.RefreshToken}";
            }
        }

        private async void RpcCall() {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param["name"] = "howdy";

            Task<BaseResponse> rpc = _supabase.Rpc("hello_js_as_json", param);
            rpc.AsUniTask().GetAwaiter().OnCompleted(Complete);
            try {
                await rpc;
            } catch (RequestException requestException) {
                result.text += $"{requestException.Message}";
                result.text += $"\n{requestException.Error}";

                string content = await requestException.Response.Content.ReadAsStringAsync();

                result.text += $"\nResponse {content}";
            } catch (Exception e) {
                result.text = "RPC failed";
                result.text += $"{e.Message}";
                result.text += $"\n{e.StackTrace}";
            }

            if (rpc.IsCompleted) {
                result.text = rpc.Result.Content;
            } else {
                result.text = $"{rpc.Status}";
            }
        }

        private void Complete() {
            result.text += "Complete";
        }

        public void StartPublic() {
            result.text = "...";
            RpcCall();
        }

        // public void ManualAppleSignIn() {
        //     result.text += "starting supabase apple sign in\n";
        //     AppleSignIn(_id);
        //     result.text += "\nawaited supabase apple sign in";
        // }

        // // ReSharper disable Unity.PerformanceAnalysis
        // private async void AppleSignIn(string identityToken) {
        //     if (identityToken == null) {
        //         result.text += "Null identity token\n";
        //         return;
        //     }

        //     result.text += "Starting supabase auth attempt\n";

        //     Task<Session> t = null;
        //     try {
        //         result.text = $"{identityToken.Length}";

        //         _supabase.Auth.GetHeaders!()["apikey"] = SUPABASE_PUBLIC_KEY;
        //         _supabase.Auth.GetHeaders!()["Authorization"] = $"Bearer ${SUPABASE_PUBLIC_KEY}";

        //         if (!string.IsNullOrEmpty(_nonce)) {
        //             result.text += $"signing in with nonce {_nonce}";
        //             t = _supabase.Auth.SignInWithIdToken(Constants.Provider.Apple, identityToken, _nonce);
        //         } else {
        //             result.text += "signing in without nonce";
        //             t = _supabase.Auth.SignInWithIdToken(Constants.Provider.Apple, identityToken);
        //         }

        //         await t;
        //     } catch (UnauthorizedException e) {
        //         result.text = "Exception with SignInWithIdToken";
        //         result.text = $"Used nonce {_nonce}";
        //         result.text += $"\n Exception {e.Message}";
        //         result.text += $"\n {e.Content}";
        //     } catch (BadRequestException e) {
        //         result.text += "\n";
        //         result.text += e.Content;
        //     } catch (Exception e) {
        //         result.text = "Unknown Exception with SignInWithIdToken";
        //         result.text += $"\n Exception {e.Message}";
        //         result.text += $"\n {e.StackTrace}";
        //     }

        //     if (t?.IsCompletedSuccessfully == true) {
        //         result.text += $"\nsupabase login success\n {t.Result?.User?.Id}";
        //     } else {
        //         result.text += $"\nsupabase failure\n {t?.Exception}";
        //     }
        // }

        public void AssignId(string identityToken, string nonce, string nonceHash) {
            _id = identityToken;
            _nonce = nonce;
        }
    }
}