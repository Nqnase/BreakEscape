using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        // �{�^���̏����ݒ�
        startButton.Select();  // �ŏ��ɑI������{�^����ݒ�

        // �{�^���̃C�x���g���X�i�[�ݒ�
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);

        // �V�[�����ǂݍ��܂ꂽ��Ƀ{�^���I�����s��
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // �V�[�����ǂݍ��܂ꂽ��ɌĂ΂��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �^�C�g���V�[���̏ꍇ�A�ŏ��̃{�^���I����ݒ�
        if (scene.name == "Title")
        {
            startButton.Select();  // �����I���{�^����ݒ�
        }
    }

    private void OnDestroy()
    {
        // �V�[�����ǂݍ��܂��C�x���g������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void StartGame()
    {
        Debug.Log("�Q�[���J�n");
        SceneManager.LoadScene("Level1"); // �Q�[���V�[������K�؂ɕύX���Ă�������
    }

    private void QuitGame()
    {
        // �A�v���P�[�V�����I��
        Application.Quit();

        // �G�f�B�^�[�ł̓�����T�|�[�g�iUnityEditor���O��Ԃ��K�v�j
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
