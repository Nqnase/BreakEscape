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
        // ボタンの初期設定
        startButton.Select();  // 最初に選択するボタンを設定

        // ボタンのイベントリスナー設定
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);

        // シーンが読み込まれた後にボタン選択を行う
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // シーンが読み込まれた後に呼ばれる
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

    private void StartGame()
    {
        Debug.Log("ゲーム開始");
        SceneManager.LoadScene("Level1"); // ゲームシーン名を適切に変更してください
    }

    private void QuitGame()
    {
        // アプリケーション終了
        Application.Quit();

        // エディターでの動作をサポート（UnityEditor名前空間が必要）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
