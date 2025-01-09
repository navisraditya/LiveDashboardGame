using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private float previousVolume = 1f; // Default volume

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f); // Default to max volume
        }

        LoadVolume();
    }

    public void ChangeVolume()
    {
        if (volumeSlider.value > 0)
        {
            previousVolume = volumeSlider.value; // Save the current volume
        }

        AudioListener.volume = volumeSlider.value; // Apply the slider value to AudioListener
        SaveVolume();
    }

    public void SetVolume(float volume)
    {
        previousVolume = volumeSlider.value; // Save the current volume
        volumeSlider.value = volume; // Set the new slider value
        AudioListener.volume = volume; // Apply to AudioListener
    }

    public void ResetVolume()
    {
        volumeSlider.value = previousVolume; // Restore the previous volume
        AudioListener.volume = previousVolume; // Apply to AudioListener
    }

    public float GetVolume()
    {
        return volumeSlider.value; // Return the current slider value
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        AudioListener.volume = volumeSlider.value;
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
