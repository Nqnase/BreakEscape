using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // シングルトンインスタンス 

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        [Range(0f, 1f)] public float defaultVolume = 1f; // 各サウンドのデフォルト音量 
    }

    [SerializeField]
    private SoundData[] soundDatas;

    private AudioSource[] audioSourceList = new AudioSource[20];
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    // 音量管理用ディクショナリ 
    private Dictionary<string, float> soundVolumes = new Dictionary<string, float>();

    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public string bgmName;
    }

    [SerializeField]
    private SceneBGM[] sceneBGMs;
    private Dictionary<string, string> sceneBGMMapping = new Dictionary<string, string>();

    private AudioSource currentBGMSource;

    private bool isMuted = false; // サウンドのオンオフ状態を保持 

    private void Awake()
    {
        // シングルトンの設定 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする 
        }
        else
        {
            Destroy(gameObject); // すでにインスタンスが存在する場合は新しいインスタンスを破棄 
            return;
        }

        // AudioSourceの初期化 
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }

        // soundDictionaryにセット 
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
            soundVolumes[soundData.name] = soundData.defaultVolume; // デフォルト音量設定 
        }

        // sceneBGMMappingにセット 
        foreach (var sceneBGM in sceneBGMs)
        {
            sceneBGMMapping[sceneBGM.sceneName] = sceneBGM.bgmName;
        }

        // シーンロード時のBGM設定 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Update()
    {
        // F8キーでサウンドのオンオフを切り替える 
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ToggleMute();
        }
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        foreach (var audioSource in audioSourceList)
        {
            audioSource.mute = isMuted;
        }
        Debug.Log(isMuted ? "サウンドオフ" : "サウンドオン");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            Play(soundData.audioClip, soundVolumes[name]);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません: {name}");
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (soundVolumes.ContainsKey(name))
        {
            soundVolumes[name] = Mathf.Clamp01(volume);
            Debug.Log($"{name} の音量が {volume} に設定されました。");
        }
        else
        {
            Debug.LogWarning($"音量を設定できるサウンドが見つかりません: {name}");
        }
    }

    public void PlayBGMForScene(string sceneName)
    {
        if (sceneBGMMapping.TryGetValue(sceneName, out var bgmName))
        {
            PlayBGM(bgmName);
        }
        else
        {
            Debug.LogWarning($"シーン名に対応するBGMが見つかりません: {sceneName}");
        }
    }

    public void PlayBGM(string bgmName)
    {
        if (currentBGMSource != null && currentBGMSource.isPlaying)
        {
            currentBGMSource.Stop();
        }

        if (soundDictionary.TryGetValue(bgmName, out var soundData))
        {
            currentBGMSource = GetUnusedAudioSource();
            if (currentBGMSource != null)
            {
                currentBGMSource.clip = soundData.audioClip;
                currentBGMSource.volume = soundVolumes[bgmName];
                currentBGMSource.loop = true;
                currentBGMSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"BGMが見つかりません: {bgmName}");
        }
    }

    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (!audioSourceList[i].isPlaying) return audioSourceList[i];
        }
        return null;
    }
}
