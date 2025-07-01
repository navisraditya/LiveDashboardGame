using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum SFX
{
    Jump,
    ButtonClicked,
}

public enum BGM
{
    MainBGM,
}

public class SoundPrefab : MonoBehaviour
{

    public static SoundPrefab Instance { get; private set; }


    #region SFX
    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource buttonClicked;
    #endregion

    #region BGM
    [SerializeField] AudioSource mainBgm;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterButtonSounds(scene);
    }

    public void PlaySFX(SFX sfxType)
    {
        switch (sfxType)
        {
            case SFX.Jump:
                jump.Play();
                break;
        }
    }

    public void PlayBGM(BGM bgmType)
    {
        switch (bgmType)
        {
            case BGM.MainBGM:
                mainBgm.Play();
                break;
        }
    }

    public void RegisterButtonSounds(Scene targetScene)
    {
        GameObject[] root = targetScene.GetRootGameObjects();
        List<Button> sceneBtns = new List<Button>();

        foreach (GameObject rootGO in root)
        {
            Button[] buttonsInRoot = rootGO.GetComponentsInChildren<Button>(true);
            sceneBtns.AddRange(buttonsInRoot);
        }

        foreach (Button btn in sceneBtns)
        {
            btn.onClick.RemoveAllListeners();

            if (this != null)
            {
                // btn.onClick.AddListener(() => SoundPrefab.Instance.PlaySFX(SFX.ButtonClicked));
                btn.onClick.AddListener(() => Debug.Log("pencet anjay"));
            }
            else
            {
                Debug.LogError("SoundPrefab kosong");
                break;
            }
        }
        Debug.Log("berhasil list semua");
    }
}