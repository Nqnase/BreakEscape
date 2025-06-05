using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // �V���O���g���C���X�^���X 

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        [Range(0f, 1f)] public float defaultVolume = 1f; // �e�T�E���h�̃f�t�H���g���� 
    }

    [SerializeField]
    private SoundData[] soundDatas;

    private AudioSource[] audioSourceList = new AudioSource[20];
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    // ���ʊǗ��p�f�B�N�V���i�� 
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

    private bool isMuted = false; // �T�E���h�̃I���I�t��Ԃ�ێ� 

    private void Awake()
    {
        // �V���O���g���̐ݒ� 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��j������Ȃ��悤�ɂ��� 
        }
        else
        {
            Destroy(gameObject); // ���łɃC���X�^���X�����݂���ꍇ�͐V�����C���X�^���X��j�� 
            return;
        }

        // AudioSource�̏����� 
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }

        // soundDictionary�ɃZ�b�g 
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
            soundVolumes[soundData.name] = soundData.defaultVolume; // �f�t�H���g���ʐݒ� 
        }

        // sceneBGMMapping�ɃZ�b�g 
        foreach (var sceneBGM in sceneBGMs)
        {
            sceneBGMMapping[sceneBGM.sceneName] = sceneBGM.bgmName;
        }

        // �V�[�����[�h����BGM�ݒ� 
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
        // F8�L�[�ŃT�E���h�̃I���I�t��؂�ւ��� 
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
        Debug.Log(isMuted ? "�T�E���h�I�t" : "�T�E���h�I��");
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
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���: {name}");
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (soundVolumes.ContainsKey(name))
        {
            soundVolumes[name] = Mathf.Clamp01(volume);
            Debug.Log($"{name} �̉��ʂ� {volume} �ɐݒ肳��܂����B");
        }
        else
        {
            Debug.LogWarning($"���ʂ�ݒ�ł���T�E���h��������܂���: {name}");
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
            Debug.LogWarning($"�V�[�����ɑΉ�����BGM��������܂���: {sceneName}");
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
            Debug.LogWarning($"BGM��������܂���: {bgmName}");
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
