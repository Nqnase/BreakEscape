using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Button backButton;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;

    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject selectCanvas;

    void Start()
    {
        selectCanvas.SetActive(false);

        // ボタンの初期設定
        startButton.Select();  // 最初に選択するボタンを設定

        // ボタンのイベントリスナー設定
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);

        backButton.onClick.AddListener(BackTitle);

        level1Button.onClick.AddListener(ChangeLevel1);
        level2Button.onClick.AddListener(ChangeLevel2);

        // シーンが読み込まれた後にボタン選択を行う
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
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
    }

    /// <summary>
    /// シーンが読み込まれた後に呼ばれる
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // タイトルシーンの場合、最初のボタン選択を設定
        if (scene.name == "Title")
        {
            startButton.Select();  // 初期選択ボタンを設定
        }
    }

    private void OnDestroy()
    {
        // シーンが読み込まれるイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// セレクト画面に移動
    /// </summary>
    public void StartGame()
    {
        backButton.Select();
        Debug.Log("セレクト画面に移動");
        selectCanvas.SetActive(true);
        titleCanvas.SetActive(false);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();

        // エディターでの動作をサポート（UnityEditor名前空間が必要）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    public void BackTitle()
    {
        startButton.Select();
        titleCanvas.SetActive(true);
        selectCanvas.SetActive(false);
    }

    /// <summary>
    /// レベル1ステージ遷移
    /// </summary>
    public void ChangeLevel1()
    {
        SceneManager.LoadScene("Level1"); // ゲームシーン名を適切に変更してください
    }

    /// <summary>
    /// レベル2ステージ遷移
    /// </summary>
    public void ChangeLevel2()
    {
        SceneManager.LoadScene("Level2"); // ゲームシーン名を適切に変更してください
    }
}
