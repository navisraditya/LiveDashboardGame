using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private float previousVolume = 1f; 

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f); 
        }

        LoadVolume();
    }

    public void ChangeVolume()
    {
        if (volumeSlider.value > 0)
        {
            previousVolume = volumeSlider.value; 
        }

        AudioListener.volume = volumeSlider.value; 
        SaveVolume();
    }

    public void SetVolume(float volume)
    {
        previousVolume = volumeSlider.value; 
        volumeSlider.value = volume; 
        AudioListener.volume = volume; 
    }

    public void ResetVolume()
    {
        volumeSlider.value = previousVolume; 
        AudioListener.volume = previousVolume; 
    }

    public float GetVolume()
    {
        return volumeSlider.value; 
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
