// using System;
// using System.Text;
// using AppleAuth;
// using AppleAuth.Enums;
// using AppleAuth.Extensions;
// using AppleAuth.Interfaces;
// using AppleAuth.Native;
// using TMPro;
// using UnityEngine;

// namespace App {
//     public class AppleSignIn : MonoBehaviour {
//         private AppleAuthManager _appleAuthManager;

//         private SupabaseStuff _supabaseStuff;

//         public TMP_InputField errorLog;

//         private string _nonce;
//         private string _nonceVerify;

//         private void Start() {
//             // If the current platform is supported
//             if (AppleAuthManager.IsCurrentPlatformSupported) {
//                 // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
//                 PayloadDeserializer deserializer = new PayloadDeserializer();
//                 // Creates an Apple Authentication manager with the deserializer
//                 _appleAuthManager = new AppleAuthManager(deserializer);
//                 errorLog.text += "Apple auth started";
//             } else {
//                 errorLog.text += "Apple auth not supported";
//             }


//             if (TryGetComponent(out SupabaseStuff o))
//                 _supabaseStuff = o;
//             else {
//                 errorLog.text += "Can't find Supabase Stuff";
//             }
//         }

//         private void Update() {
//             _appleAuthManager?.Update();
//         }

//         private const string APPLE_USER_ID_KEY = "appleUserKey";

//         public void PerformSignIn() {
//             _nonce = null;
//             _nonceVerify = null;

//             if (_appleAuthManager == null) {
//                 errorLog.text = "Apple Sign In Not Available";
//                 return;
//             }

//             AppleAuthLoginArgs loginArgs =
//                 new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

//             try {
//                 errorLog.text += "Apple login start (no nonce)";
//                 _appleAuthManager.LoginWithAppleId(loginArgs, SuccessCallback, ErrorCallback);
//                 errorLog.text += "Apple login done (no nonce)";
//             } catch (Exception e) {
//                 errorLog.text += e.Message;
//                 Debug.LogException(e);
//             }
//         }

//         public void PerformSignInWithNonce() {
//             if (_appleAuthManager == null) {
//                 errorLog.text = "Apple Sign In Not Available";
//                 return;
//             }

//             _nonce = Supabase.Gotrue.Helpers.GenerateNonce();
//             _nonceVerify = Supabase.Gotrue.Helpers.GenerateSHA256NonceFromRawNonce(_nonce);

//             errorLog.text += $"\nNonce sent to Apple:\n {_nonce}";
//             errorLog.text += $"\nNonce Hash: {_nonceVerify}\n";

//             AppleAuthLoginArgs loginArgs =
//                 new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName, _nonceVerify);

//             try {
//                 errorLog.text += "Apple login start (nonce)";
//                 _appleAuthManager.LoginWithAppleId(loginArgs, SuccessCallback, ErrorCallback);
//                 errorLog.text += "Apple login done (nonce)";
//             } catch (Exception e) {
//                 errorLog.text += e.Message;
//                 Debug.LogException(e);
//             }
//         }

//         private void ErrorCallback(IAppleError error) {
//             errorLog.text += $"{error.GetAuthorizationErrorCode()} {error.LocalizedFailureReason}";
//         }

//         private void SuccessCallback(ICredential credential) {
//             if (credential == null) errorLog.text += " null credential?";

//             errorLog.text += "success!";

//             // Obtained credential, cast it to IAppleIDCredential
//             if (credential is IAppleIDCredential appleIdCredential) {
//                 // Apple User ID
//                 // You should save the user ID somewhere in the device
//                 string userId = appleIdCredential.User;
//                 PlayerPrefs.SetString(APPLE_USER_ID_KEY, userId);

//                 // Email (Received ONLY in the first login)
//                 string email = appleIdCredential.Email;
//                 if (email != null) PlayerPrefs.SetString("email", email);

//                 // Full name (Received ONLY in the first login)
//                 // IPersonName fullName = appleIdCredential.FullName;
//                 // PlayerPrefs.SetString("familyName", appleIdCredential.FullName.FamilyName);
//                 // PlayerPrefs.SetString("givenName", appleIdCredential.FullName.GivenName);

//                 // Identity token
//                 string identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken,
//                     0,
//                     appleIdCredential.IdentityToken.Length);
//                 PlayerPrefs.SetString("identityToken", identityToken);

//                 // Authorization code
//                 string authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode,
//                     0,
//                     appleIdCredential.AuthorizationCode.Length);
//                 PlayerPrefs.SetString("authCode", authorizationCode);

//                 errorLog.text += "Apple sign in worked!\n";

//                 // And now you have all the information to create/login a user in your system

//                 errorLog.text += $"Identity token from Apple: \n{identityToken}";

//                 _supabaseStuff.AssignId(identityToken, _nonce, _nonceVerify);
//             } else {
//                 errorLog.text += "credential is not a IAppleIDCredential";
//             }
//         }
//     }
// }