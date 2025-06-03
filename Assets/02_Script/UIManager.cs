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
    [SerializeField] public GameObject _GameOverPanel; // ゲームオーバーパネル 
    [SerializeField] private GameObject _PausePanel; // ポーズパネル 

    [SerializeField] private Button _ReturnToTitleButton;
    [SerializeField] private Button _RetryButton;

    [SerializeField] private Button _GameOverRetryButton; // ゲームオーバー時のリトライボタン 
    [SerializeField] private Button _GameOverReturnToTitleButton; // ゲームオーバー時のタイトルボタン 

    [SerializeField] private Button _PauseRetryButton; // ゲームオーバー時のリトライボタン 
    [SerializeField] private Button _PauseToTitleButton; // ゲームオーバー時のタイトルボタン 

    [SerializeField] private Image _ExitInf;    // 脱出を案内する画像
    [SerializeField] private Image _StartInf;
    public float displayDelay = 2.0f; // 非表示までの待機時間（秒）

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
        _PausePanel.SetActive(false); // 初期は非表示
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

        // ポーズ処理のトグル (Escapeキーに対応)
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
        timer += Time.deltaTime; // 経過時間をカウント

        if (timer >= displayDelay)
        {
            // 一定時間経過したらオブジェクトを表示
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
        _GameOverRetryButton.Select(); // 初期選択ボタン 
        buttonsActivated = true;
    }

    public void TogglePauseUI()
    {
        if (player.OpeningMap == true)
            return;
        _PauseRetryButton.Select(); // 初期選択ボタン 
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
        Time.timeScale = 1f; // シーン遷移時に時間スケールをリセット
        SceneManager.LoadScene("Title");
    }

    public void RetryStage()
    {
        Time.timeScale = 1f; // シーン遷移時に時間スケールをリセット
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
