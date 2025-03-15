using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using Client = Supabase.Client;

namespace App
{
    public class RegisButton : MonoBehaviour
    {
        [SerializeField] private CanvasGroup regisCanvasGroup;
        [SerializeField] private CanvasGroup loginCanvasGroup;
        [SerializeField] private TMP_InputField email;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private TMP_InputField username;
        [SerializeField] private CanvasGroup regisPopUpNo;
        [SerializeField] private CanvasGroup regisPopUpYes;

        private static Client _supabase;

        private void Awake()
        {
            _supabase = SupabaseStuff.Instance?.GetSupabaseClient();

            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
            }
        }

        public async void RegisterUser()
        {
            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
                ShowErrorPopup();
                return;
            }

            if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text) || string.IsNullOrEmpty(username.text))
            {
                Debug.LogError("Email, password, or username is empty.");
                ShowErrorPopup();
                return;
            }

            Debug.Log("Starting sign up");

            var options = new SignUpOptions
            {
                Data = new Dictionary<string, object>
                {
                    {"username", username.text}
                }
            };

            try
            {
                var session = await _supabase.Auth.SignUp(email.text, password.text, options);

                if (session == null)
                {
                    Debug.LogError("Sign up failed: Session is null.");
                    ShowErrorPopup();
                    return;
                }

                Debug.Log($"Supabase sign up user id: {session.User?.Id}; Username: {username.text};");
                ShowSuccessPopup();
            }
            catch (BadRequestException ex)
            {
                Debug.LogError($"BadRequestException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (UnauthorizedException ex)
            {
                Debug.LogError($"UnauthorizedException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (ExistingUserException ex)
            {
                Debug.LogError($"ExistingUserException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (ForbiddenException ex)
            {
                Debug.LogError($"ForbiddenException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (InvalidEmailOrPasswordException ex)
            {
                Debug.LogError($"InvalidEmailOrPasswordException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unknown exception: {ex.Message}");
                ShowErrorPopup();
            }
        }

        public async void RegisLogin()
        {
            await RegisterUserAsync();

            if (regisPopUpYes != null)
            {
                regisPopUpYes.gameObject.SetActive(true);
            }

            await UniTask.Delay(3000); // Wait for 3 seconds

            if (regisCanvasGroup != null)
            {
                regisCanvasGroup.gameObject.SetActive(false);
            }

            if (loginCanvasGroup != null)
            {
                loginCanvasGroup.gameObject.SetActive(true);
            }
        }

        private async UniTask RegisterUserAsync()
        {
            if (_supabase == null)
            {
                Debug.LogError("Supabase client is not initialized.");
                ShowErrorPopup();
                return;
            }

            if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text) || string.IsNullOrEmpty(username.text))
            {
                Debug.LogError("Email, password, or username is empty.");
                ShowErrorPopup();
                return;
            }

            Debug.Log("Starting sign up");

            var options = new SignUpOptions
            {
                Data = new Dictionary<string, object>
                {
                    {"username", username.text}
                }
            };

            try
            {
                var session = await _supabase.Auth.SignUp(email.text, password.text, options);

                if (session == null)
                {
                    Debug.LogError("Sign up failed: Session is null.");
                    ShowErrorPopup();
                    return;
                }

                Debug.Log($"Supabase sign up user id: {session.User?.Id}; Username: {username.text};");
                ShowSuccessPopup();
            }
            catch (BadRequestException ex)
            {
                Debug.LogError($"BadRequestException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (UnauthorizedException ex)
            {
                Debug.LogError($"UnauthorizedException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (ExistingUserException ex)
            {
                Debug.LogError($"ExistingUserException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (ForbiddenException ex)
            {
                Debug.LogError($"ForbiddenException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (InvalidEmailOrPasswordException ex)
            {
                Debug.LogError($"InvalidEmailOrPasswordException: {ex.Message}");
                ShowErrorPopup();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unknown exception: {ex.Message}");
                ShowErrorPopup();
            }
        }

        private void ShowErrorPopup()
        {
            if (regisPopUpNo != null)
            {
                regisPopUpNo.gameObject.SetActive(true);
            }

            email.text = "";
            password.text = "";
            username.text = "";
        }

        private void ShowSuccessPopup()
        {
            if (regisPopUpYes != null)
            {
                regisPopUpYes.gameObject.SetActive(true);
            }
        }
    }
}