using UnityEngine;
using UnityEngine.UI;

public class MuteManager : MonoBehaviour
{
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private SoundManager soundManager; 
    private bool muted;

    void Start()
    {
        // Ensure default values
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0); 
        }

        LoadMuteState();
        UpdateToggleState();
        ApplyMuteState();
    }

    public void OnMuteToggleChanged()
    {
        muted = !muteToggle.isOn; 
        ApplyMuteState();
        SaveMuteState();
    }

    public void OnVolumeChanged()
    {
        if (soundManager != null && soundManager.GetVolume() > 0)
        {
            muted = false; 
            UpdateToggleState();
            SaveMuteState();
        }
    }

    private void LoadMuteState()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void SaveMuteState()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }

    private void UpdateToggleState()
    {
        if (muteToggle != null)
        {
            muteToggle.isOn = !muted;
        }
    }

    private void ApplyMuteState()
    {
        if (muted)
        {
            AudioListener.pause = true;
            if (soundManager != null)
            {
                soundManager.SetVolume(0); 
            }
        }
        else
        {
            AudioListener.pause = false;
            if (soundManager != null)
            {
                soundManager.ResetVolume(); 
            }
        }
    }
}
