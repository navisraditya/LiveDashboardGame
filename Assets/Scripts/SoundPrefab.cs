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
    level1,
    level2,
    level3,
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
    [SerializeField] AudioSource level1;
    [SerializeField] AudioSource level2;
    [SerializeField] AudioSource level3;
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

        if (mainBgm != null) mainBgm.loop = true;
        if (level1 != null) level1.loop = true;
        if (level2 != null) level2.loop = true;
        if (level3 != null) level3.loop = true;

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
            case SFX.ButtonClicked:
                buttonClicked.Play();
                break;
        }
    }

    public void PlayBGM(BGM bgmType)
    {
        AudioSource target = null;

        StopAllBGM();
        switch (bgmType)
        {
            case BGM.MainBGM:
                target = mainBgm;
                break;
            case BGM.level1:
                target = level1;
                break;
            case BGM.level2:
                target = level2;
                break;
            case BGM.level3:
                target = level3;
                break;
        }

        if (target != null)
        {
            if (!target.isPlaying)
            {
                Debug.Log($"{bgmType} played");
                target.Play();
            }
        }
    }

    private void StopAllBGM()
    {
        mainBgm.Stop();
        level1.Stop();
        level2.Stop();
        level3.Stop();
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
                btn.onClick.AddListener(() => PlaySFX(SFX.ButtonClicked));
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