using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI itemCountText;

    [SerializeField] public Slider _HpSlider;

    [SerializeField] public GameObject _StageClearImage;
    [SerializeField] public GameObject _GameOverPanel; // �Q�[���I�[�o�[�p�l�� 
    [SerializeField] private GameObject _PausePanel; // �|�[�Y�p�l�� 

    [SerializeField] private Button _ReturnToTitleButton;
    [SerializeField] private Button _RetryButton;

    [SerializeField] private Button _GameOverRetryButton; // �Q�[���I�[�o�[���̃��g���C�{�^�� 
    [SerializeField] private Button _GameOverReturnToTitleButton; // �Q�[���I�[�o�[���̃^�C�g���{�^�� 

    [SerializeField] private Button _PauseRetryButton; // �Q�[���I�[�o�[���̃��g���C�{�^�� 
    [SerializeField] private Button _PauseToTitleButton; // �Q�[���I�[�o�[���̃^�C�g���{�^�� 

    [SerializeField] private Image _ExitInf;    // �E�o���ē�����摜
    [SerializeField] private Image _StartInf;
    public float displayDelay = 2.0f; // ��\���܂ł̑ҋ@���ԁi�b�j

    private float timer;
    private bool isDisplayed;
    private PlayerController player;
    private bool buttonsActivated = false;
    public bool isPauseing = false;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();

        _ReturnToTitleButton.gameObject.SetActive(false);
        _RetryButton.gameObject.SetActive(false);
        _GameOverPanel.SetActive(false);
        _PausePanel.SetActive(false); // �����͔�\��
        _ExitInf.gameObject.SetActive(false);

        _ReturnToTitleButton.onClick.AddListener(LoadTitle);
        _RetryButton.onClick.AddListener(RetryStage);

        _GameOverRetryButton.onClick.AddListener(RetryStage);
        _GameOverReturnToTitleButton.onClick.AddListener(LoadTitle);

        timer = 0.0f;
        isDisplayed = false;


    }

    void Update()
    {
        if (player != null)
        {
            UpdateHPText(player.currentHealth);
            UpdateItemText(player.itemCount);
        }

        if (player != null && player._isClear && !buttonsActivated)
        {
            ShowStageClearUI();
        }

        if (player != null && player._isDead && !buttonsActivated)
        {
            ShowGameOverUI();
        }

        // �|�[�Y�����̃g�O�� (Escape�L�[�ɑΉ�)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePauseUI();
        }

        if (!isDisplayed)
        {
            StartInformation();
        }

        if (player._canExit == true)
        {
            ExitInformation();
        }
    }

    void StartInformation()
    {
        timer += Time.deltaTime; // �o�ߎ��Ԃ��J�E���g

        if (timer >= displayDelay)
        {
            // ��莞�Ԍo�߂�����I�u�W�F�N�g��\��
            _StartInf.gameObject.SetActive(false);
            isDisplayed = true;
        }
    }

    void ShowStageClearUI()
    {
        _StageClearImage.gameObject.SetActive(true);
        _ReturnToTitleButton.gameObject.SetActive(true);
        _RetryButton.gameObject.SetActive(true);
        _RetryButton.Select();
        buttonsActivated = true;
    }

    void ShowGameOverUI()
    {
        _GameOverPanel.SetActive(true);
        _GameOverRetryButton.gameObject.SetActive(true);
        _GameOverReturnToTitleButton.gameObject.SetActive(true);
        _GameOverRetryButton.Select(); // �����I���{�^�� 
        buttonsActivated = true;
    }

    public void TogglePauseUI()
    {
        if (player.OpeningMap == true)
            return;
        _PauseRetryButton.Select(); // �����I���{�^�� 
        isPauseing = !isPauseing;
        _PausePanel.SetActive(isPauseing);
        if (isPauseing)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void ExitInformation()
    {
        if (player._canExit == true)
        {
            _ExitInf.gameObject.SetActive(true);
        }
    }

    void UpdateHPText(int currentHealth)
    {
        _HpSlider.value = currentHealth;
        hpText.text = $" {currentHealth}/200";
    }

    void UpdateItemText(int itemCount)
    {
        itemCountText.text = $"{itemCount}/6 ";
    }

    public void LoadTitle()
    {
        Time.timeScale = 1f; // �V�[���J�ڎ��Ɏ��ԃX�P�[�������Z�b�g
        SceneManager.LoadScene("Title");
    }

    public void RetryStage()
    {
        Time.timeScale = 1f; // �V�[���J�ڎ��Ɏ��ԃX�P�[�������Z�b�g
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
