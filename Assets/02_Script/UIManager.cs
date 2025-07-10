using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

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
    [SerializeField] public Image _DontExit;
    [SerializeField] private Image _StartInf;
    [SerializeField] private Button _StartButton;

    private float timer;
    private bool isDisplayed;
    private PlayerController player;
    private bool buttonsActivated = false;
    public bool isPauseing = false;
    private bool startDontPause = false;

    void Start()
    {
        _StartButton.Select();
        _StartButton.onClick.AddListener(StartInformation);
        player = FindAnyObjectByType<PlayerController>();
        _ReturnToTitleButton.gameObject.SetActive(false);
        _RetryButton.gameObject.SetActive(false);
        _GameOverPanel.SetActive(false);
        _PausePanel.SetActive(false); // 初期は非表示
        _ExitInf.gameObject.SetActive(false);
        _DontExit.gameObject.SetActive(false); ;

        _ReturnToTitleButton.onClick.AddListener(LoadTitle);
        _RetryButton.onClick.AddListener(RetryStage);

        _GameOverRetryButton.onClick.AddListener(RetryStage);
        _GameOverReturnToTitleButton.onClick.AddListener(LoadTitle);

        timer = 0.0f;
        isDisplayed = false;
        startDontPause = true;

        player.GetComponent<PlayerController>().enabled = false; //ゲーム開始時にプレイヤーコントローラーをオフ
    }

    void Update()
    {
        // マウスがUI上にあるかどうか確認
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // マウスの下にあるUIを取得（Raycastを使う）
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            var raycastResults = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            foreach (var result in raycastResults)
            {
                var selectable = result.gameObject.GetComponent<Selectable>();
                if (selectable != null && result.gameObject != EventSystem.current.currentSelectedGameObject)
                {
                    // マウス下のSelectableを強制選択
                    selectable.Select();
                    break;
                }
            }
        }

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

        if (player._canExit == true)
        {
            ExitInformation();
        }
    }

    public void StartInformation()
    {
        player.GetComponent<PlayerController>().enabled = true; // ゲーム開始時のインフォメーションが終わったらプレイヤーコントローラーをオン

        // 一定時間経過したらオブジェクトを表示
        _StartInf.gameObject.SetActive(false);
        isDisplayed = true;
        startDontPause = false;
    }

    /// <summary>
    /// ゲームクリア時起動
    /// </summary>
    void ShowStageClearUI()
    {
        _StageClearImage.gameObject.SetActive(true);
        _ReturnToTitleButton.gameObject.SetActive(true);
        _RetryButton.gameObject.SetActive(true);
        _RetryButton.Select();// 初期選択ボタン 
        buttonsActivated = true;
    }

    /// <summary>
    /// ゲームオーバー時起動
    /// </summary>
    void ShowGameOverUI()
    {
        _GameOverPanel.SetActive(true);
        _GameOverRetryButton.gameObject.SetActive(true);
        _GameOverReturnToTitleButton.gameObject.SetActive(true);
        _GameOverRetryButton.Select(); // 初期選択ボタン 
        buttonsActivated = true;
    }

    /// <summary>
    /// ポーズのオン・オフ
    /// </summary>
    public void TogglePauseUI()
    {
        if (startDontPause == false)
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
