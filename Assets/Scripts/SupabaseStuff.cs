// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Cysharp.Threading.Tasks;
// using Postgrest.Responses;
// using Supabase.Gotrue;
// using TMPro;
// using UnityEngine;
// using Client = Supabase.Client;
// using RequestException = Postgrest.RequestException;

// namespace App {
//     public class SupabaseStuff : MonoBehaviour {
//     public static SupabaseStuff Instance { get; private set;}
//     public const string SUPABASE_URL = "https://rbmxqlqzyemtwsajfjtw.supabase.co";
//     public const string SUPABASE_PUBLIC_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJibXhxbHF6eWVtdHdzYWpmanR3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzcwMDgyNjcsImV4cCI6MjA1MjU4NDI2N30.N2-ULM2_1zc_yCo3zoYlolIZhX8OPnixsILHqhZxTO8";
//         // public TMP_InputField result;
//         public TMP_InputField email;
//         public TMP_InputField password;

//         private Client _supabase;
//         // private string _id;
//         // private string _nonce;

//         private async void Awake() {
//             if (Instance != null && Instance != this) {
//                 Destroy(gameObject);
//                 return;
//             }

//             Instance = this;
//             DontDestroyOnLoad(gameObject);


//             if (_supabase == null) {
//                 _supabase = new Client(SUPABASE_URL, SUPABASE_PUBLIC_KEY);
//                 await _supabase.InitializeAsync();
//                 Debug.Log("supabase intiated");
//             }
//         }

//         // public async void RegisterUser() {
//         //     Task<Session> signUp = _supabase.Auth.SignUp(email.text, password.text);
//         //     signUp.AsUniTask().GetAwaiter().OnCompleted(Complete);
//         //     try {
//         //         await signUp;
//         //     } catch (BadRequestException badRequestException) {
//         //         Debug.Log("BadRequestException") ;
//         //         Debug.Log($"{badRequestException.Message}") ;
//         //         Debug.Log($"{badRequestException.Content}") ;
//         //         Debug.Log($"{badRequestException.StackTrace}") ;
//         //         return;
//         //     } catch (UnauthorizedException unauthorizedException) {
//         //         Debug.Log("UnauthorizedException") ;
//         //         Debug.Log(unauthorizedException.Message) ;
//         //         Debug.Log(unauthorizedException.Content) ;
//         //         Debug.Log(unauthorizedException.StackTrace) ;
//         //         return;
//         //     } catch (ExistingUserException existingUserException) {
//         //         Debug.Log("ExistingUserException") ;
//         //         Debug.Log(existingUserException.Message) ;
//         //         Debug.Log(existingUserException.Content) ;
//         //         Debug.Log(existingUserException.StackTrace) ;
//         //         return;
//         //     } catch (ForbiddenException forbiddenException) {
//         //         Debug.Log("ForbiddenException") ;
//         //         Debug.Log(forbiddenException.Message) ;
//         //         Debug.Log(forbiddenException.Content) ;
//         //         Debug.Log(forbiddenException.StackTrace) ;
//         //         return;
//         //     // } catch (InvalidProviderException invalidProviderException) {
//         //     //     Debug.Log() "invalidProviderException";
//         //     //     Debug.Log() invalidProviderException.Message;
//         //     //     Debug.Log() invalidProviderException.StackTrace;
//         //     //     return;
//         //     } catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException) {
//         //         Debug.Log("invalidEmailOrPasswordException") ;
//         //         Debug.Log(invalidEmailOrPasswordException.Message) ;
//         //         Debug.Log(invalidEmailOrPasswordException.Content) ;
//         //         Debug.Log(invalidEmailOrPasswordException.StackTrace) ;
//         //         return;
//         //     } catch (Exception exception) {
//         //         Debug.Log("unknown exception") ;
//         //         Debug.Log(exception.Message) ;
//         //         Debug.Log(exception.StackTrace) ;
//         //         return;
//         //     }

//         //     Session session = signUp.Result;

//         //     Debug.Log($"Supabase sign in user id: {session?.User?.Id}");
//         // }

//         // public async void LogInUser() {
//         //     Debug.Log("starting sign up");
//         //     Task<Session> signUp = _supabase.Auth.SignInWithPassword(email.text, password.text);
//         //     signUp.AsUniTask().GetAwaiter().OnCompleted(Complete);
//         //     try {
//         //         await signUp;
//         //     } catch (BadRequestException badRequestException) {
//         //         Debug.Log("BadRequestException") ;
//         //         Debug.Log($"{badRequestException.Message}") ;
//         //         Debug.Log($"{badRequestException.Content}") ;
//         //         Debug.Log($"{badRequestException.StackTrace}") ;
//         //         return;
//         //     } catch (UnauthorizedException unauthorizedException) {
//         //         Debug.Log("UnauthorizedException") ;
//         //         Debug.Log(unauthorizedException.Message) ;
//         //         Debug.Log(unauthorizedException.Content) ;
//         //         Debug.Log(unauthorizedException.StackTrace) ;
//         //         return;
//         //     } catch (ExistingUserException existingUserException) {
//         //         Debug.Log("ExistingUserException") ;
//         //         Debug.Log(existingUserException.Message) ;
//         //         Debug.Log(existingUserException.Content) ;
//         //         Debug.Log(existingUserException.StackTrace) ;
//         //         return;
//         //     } catch (ForbiddenException forbiddenException) {
//         //         Debug.Log("ForbiddenException") ;
//         //         Debug.Log(forbiddenException.Message) ;
//         //         Debug.Log(forbiddenException.Content) ;
//         //         Debug.Log(forbiddenException.StackTrace) ;
//         //         return;
//         //     // } catch (InvalidProviderException invalidProviderException) {
//         //     //     Debug.Log() "invalidProviderException";
//         //     //     Debug.Log() invalidProviderException.Message;
//         //     //     Debug.Log() invalidProviderException.StackTrace;
//         //     //     return;
//         //     } catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException) {
//         //         Debug.Log("invalidEmailOrPasswordException") ;
//         //         Debug.Log(invalidEmailOrPasswordException.Message) ;
//         //         Debug.Log(invalidEmailOrPasswordException.Content) ;
//         //         Debug.Log(invalidEmailOrPasswordException.StackTrace) ;
//         //         return;
//         //     } catch (Exception exception) {
//         //         Debug.Log("unknown exception") ;
//         //         Debug.Log(exception.Message) ;
//         //         Debug.Log(exception.StackTrace) ;
//         //         return;
//         //     }

//         //     if (!signUp.IsCompletedSuccessfully) {
//         //         Debug.Log(JsonUtility.ToJson(signUp.Exception));
//         //         return;
//         //     }

//         //     Session session = signUp.Result;

//         //     if (session == null)
//         //         Debug.Log( "nope");
//         //     else {
//         //         Debug.Log($"Sign in success {session.User?.Id} {session.AccessToken} {session.User?.Aud} {session.User?.Email} {session.RefreshToken}");
//         //     }
//         // }

//         private async void RpcCall() {
//             Dictionary<string, object> param = new Dictionary<string, object>();
//             param["name"] = "howdy";

//             Task<BaseResponse> rpc = _supabase.Rpc("hello_js_as_json", param);
//             rpc.AsUniTask().GetAwaiter().OnCompleted(Complete);
//             try {
//                 await rpc;
//             } catch (RequestException requestException) {
//                 Debug.Log($"{requestException.Message}") ;
//                 Debug.Log($"\n{requestException.Error}") ;

//                 string content = await requestException.Response.Content.ReadAsStringAsync();

//                 Debug.Log($"\nResponse {content}");
//             } catch (Exception e) {
//                 Debug.Log("RPC failed") ;
//                 Debug.Log($"{e.Message}") ;
//                 Debug.Log($"\n{e.StackTrace}") ;
//             }

//             if (rpc.IsCompleted) {
//                 Debug.Log(rpc.Result.Content);
//             } else {
//                 Debug.Log($"{rpc.Status}");
//             }
//         }

//         private void Complete() {
//             Debug.Log("Complete") ;
//         }

//         public void StartPublic() {
//             Debug.Log("...");
//             RpcCall();
//         }
        
//         public Client GetSupabaseClient() {
//             return _supabase;
//         }

//         public User GetLoggedInUser() {
//                 if (_supabase == null) {
//                     Debug.LogError("supabase client is not initialized");
//                     return null;
//                 }

//                 var session = _supabase.Auth.CurrentSession;
//                 if (session == null) {
//                     Debug.Log("no active session found");
//                     return null;
//                 }

//                 var user = _supabase.Auth.CurrentUser;
//                 if (user == null) {
//                     Debug.Log("no user is currently logged in");
//                     return null;
//                 }

//                 Debug.Log($"Logged in user: {user.Email}");
//                 return user;
//         }

//         public bool CheckLoggedInUser() {
//             var user = GetLoggedInUser();
//             if (user != null){
//                 Debug.Log($"User is logged in: {user.Email}");
//                 return true;
//             } else {
//                 Debug.Log("No user is logged in.");
//                 return false;
//             }
//         }

//     //     public void ManualAppleSignIn() {
//     //         Debug.Log() "starting supabase apple sign in\n";
//     //         AppleSignIn(_id);
//     //         Debug.Log() "\nawaited supabase apple sign in";
//     //     }

//     //     // ReSharper disable Unity.PerformanceAnalysis
//     //     private async void AppleSignIn(string identityToken) {
//     //         if (identityToken == null) {
//     //             Debug.Log() "Null identity token\n";
//     //             return;
//     //         }

//     //         Debug.Log() "Starting supabase auth attempt\n";

//     //         Task<Session> t = null;
//     //         try {
//     //             Debug.Log = $"{identityToken.Length}";

//     //             _supabase.Auth.GetHeaders!()["apikey"] = SUPABASE_PUBLIC_KEY;
//     //             _supabase.Auth.GetHeaders!()["Authorization"] = $"Bearer ${SUPABASE_PUBLIC_KEY}";

//     //             if (!string.IsNullOrEmpty(_nonce)) {
//     //                 Debug.Log() $"signing in with nonce {_nonce}";
//     //                 t = _supabase.Auth.SignInWithIdToken(Constants.Provider.Apple, identityToken, _nonce);
//     //             } else {
//     //                 Debug.Log() "signing in without nonce";
//     //                 t = _supabase.Auth.SignInWithIdToken(Constants.Provider.Apple, identityToken);
//     //             }

//     //             await t;
//     //         } catch (UnauthorizedException e) {
//     //             Debug.Log = "Exception with SignInWithIdToken";
//     //             Debug.Log = $"Used nonce {_nonce}";
//     //             Debug.Log() $"\n Exception {e.Message}";
//     //             Debug.Log() $"\n {e.Content}";
//     //         } catch (BadRequestException e) {
//     //             Debug.Log() "\n";
//     //             Debug.Log() e.Content;
//     //         } catch (Exception e) {
//     //             Debug.Log = "Unknown Exception with SignInWithIdToken";
//     //             Debug.Log() $"\n Exception {e.Message}";
//     //             Debug.Log() $"\n {e.StackTrace}";
//     //         }

//     //         if (t?.IsCompletedSuccessfully == true) {
//     //             Debug.Log() $"\nsupabase login success\n {t.Result?.User?.Id}";
//     //         } else {
//     //             Debug.Log() $"\nsupabase failure\n {t?.Exception}";
//     //         }
//     //     }

//     //     public void AssignId(string identityToken, string nonce, string nonceHash) {
//     //         _id = identityToken;
//     //         _nonce = nonce;
//     //     }
//     }
// }

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

namespace App
{
    public class SupabaseStuff : MonoBehaviour
    {
        public static SupabaseStuff Instance { get; private set; }
        public const string SUPABASE_URL = "https://rbmxqlqzyemtwsajfjtw.supabase.co";
        public const string SUPABASE_PUBLIC_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJibXhxbHF6eWVtdHdzYWpmanR3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzcwMDgyNjcsImV4cCI6MjA1MjU4NDI2N30.N2-ULM2_1zc_yCo3zoYlolIZhX8OPnixsILHqhZxTO8";

        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;

        private Client _supabase;

private async void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject); // Ensure this object persists across scenes

    if (_supabase == null)
    {
        _supabase = new Client(SUPABASE_URL, SUPABASE_PUBLIC_KEY);
        await InitializeSupabase();
    }

    // Check if there's a stored access token and initialize the client with it
    string accessToken = PlayerPrefs.GetString("supabase_access_token", null);
    if (!string.IsNullOrEmpty(accessToken))
    {
        Debug.Log("Initializing Supabase client with stored access token.");
        InitializeSupabaseClient(accessToken);
    }
    else
    {
        Debug.Log("No access token found. Supabase client initialized without authentication.");
    }

}    

        private async UniTask InitializeSupabase()
        {
            try
            {
                await _supabase.InitializeAsync();
                Debug.Log("Supabase initialized");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to initialize Supabase: " + e.Message);
            }
        }

        /// <summary>
        /// Initializes the Supabase client with a specific access token.
        /// This is used after a user logs in to ensure authenticated requests.
        /// </summary>
public void InitializeSupabaseClient(string accessToken)
{
    if (_supabase == null)
    {
        _supabase = new Client(SUPABASE_URL, SUPABASE_PUBLIC_KEY);
    }

    // Set the access token for authenticated requests
    _supabase.Auth.SetAuth(accessToken);
    Debug.Log("Supabase client initialized with access token.");
}

public User GetLoggedInUser()
{
    if (_supabase == null)
    {
        Debug.LogError("Supabase client is not initialized.");
        return null;
    }

    var session = _supabase.Auth.CurrentSession;
    if (session == null)
    {
        Debug.Log("No active session found.");
        return null;
    }

    var user = _supabase.Auth.CurrentUser;
    if (user == null)
    {
        Debug.Log("No user is currently logged in.");
        return null;
    }

    Debug.Log($"Logged in user: {user.Email}");
    return user;
}
        public async UniTask<User> RegisterUser(string email, string password, string username)
        {
            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
                return null;
            }

            var options = new SignUpOptions
            {
                Data = new Dictionary<string, object>
                {
                    {"username", username}
                }
            };

            try
            {
                var session = await _supabase.Auth.SignUp(email, password, options);
                Debug.Log($"User registered: {session.User?.Id}");
                return session.User;
            }
            catch (Exception e)
            {
                Debug.LogError("Error registering user: " + e.Message);
                return null;
            }
        }

        public async UniTask<User> LogInUser(string email, string password)
        {
            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
                return null;
            }

            try
            {
                var session = await _supabase.Auth.SignInWithPassword(email, password);
                Debug.Log($"User logged in: {session.User?.Id}");

                // Store the access token for future authenticated requests
                PlayerPrefs.SetString("supabase_access_token", session.AccessToken);
                InitializeSupabaseClient(session.AccessToken);

                return session.User;
            }
            catch (Exception e)
            {
                Debug.LogError("Error logging in user: " + e.Message);
                return null;
            }
        }

        private async UniTask RpcCall()
        {
            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
                return;
            }

            var param = new Dictionary<string, object>
            {
                {"name", "howdy"}
            };

            try
            {
                var response = await _supabase.Rpc("hello_js_as_json", param);
                Debug.Log("RPC call successful: " + response.Content);
            }
            catch (RequestException e)
            {
                Debug.LogError($"RPC failed: {e.Message}");
                Debug.LogError($"Response: {await e.Response.Content.ReadAsStringAsync()}");
            }
            catch (Exception e)
            {
                Debug.LogError("RPC failed: " + e.Message);
            }
        }

        public void StartPublic()
        {
            Debug.Log("Starting RPC call...");
            RpcCall().Forget();
        }

        public Client GetSupabaseClient()
        {
            return _supabase;
        }

        public bool CheckLoggedInUser()
        {
            var user = GetLoggedInUser();
            if (user != null)
            {
                Debug.Log($"User is logged in: {user.Email}");
                return true;
            }
            else
            {
                Debug.Log("No user is logged in.");
                return false;
            }
        }

        /// <summary>
        /// Clears the current session and logs out the user.
        /// </summary>
        public async UniTask LogOutUser()
        {
            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
                return;
            }

            try
            {
                await _supabase.Auth.SignOut();
                PlayerPrefs.DeleteKey("supabase_access_token"); // Clear the stored access token
                Debug.Log("User logged out successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError("Error logging out user: " + e.Message);
            }
        }
    }
}