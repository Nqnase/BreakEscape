using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public int itemCount; //オブジェクトの数
    public GameObject[] objectToSpawn; // 生成するオブジェクト
    //public KeyCode spawnKey = KeyCode.Space; // 長押しするキー
    public float holdTime = 1f; // 長押しする必要がある時間

    private float keyHoldTimer = 0f;
    private bool isPlayerInRange = false;

    [Header("UI")]
    [SerializeField] private ParticleSystem chestUI;
    [SerializeField] private ParticleSystem itemUI;
    private ParticleSystem newParticle;
    private Vector3 chestPos;

    [SerializeField]
    private Slider progressBar; // 共有スライダー
    [SerializeField]
    private Canvas canvas; // 共有スライダーのキャンバス

    void Start()
    {
        //if (progressBar == null)
        //{
        //    // シーン内のスライダーとキャンバスを検索
        //    progressBar = FindObjectOfType<Slider>();
        //    if (progressBar != null)
        //    {
        //        canvas = progressBar.GetComponentInParent<Canvas>();
        //        canvas.gameObject.SetActive(false); // 最初は非表示
        //        progressBar.value = 0f; // 初期値
        //    }
        //    else
        //    {
        //        Debug.LogError("Sliderがシーン内に存在しません");
        //    }
        //}

        chestPos = this.transform.position;

        // パーティクルシステムのインスタンスを生成する。
        newParticle = Instantiate(chestUI);
        newParticle.transform.position = new Vector3
            (chestPos.x, chestPos.y - 0.5f, chestPos.z);
        // パーティクルを発生させる。
        newParticle.Play();

        canvas.gameObject.SetActive(false); // 最初は非表示
        progressBar.value = 0f; // 初期値
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
                canvas.transform.position = this.transform.position + Vector3.up; // 宝箱の上に表示
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            keyHoldTimer = 0f; // プレイヤーが範囲外に出たらタイマーリセット
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false); // 範囲外ならゲージを非表示
            }
            if (progressBar != null)
            {
                progressBar.value = 0f; // ゲージリセット
            }
        }
    }

    void Update()
    {
        Debug.Log(keyHoldTimer);

        if (isPlayerInRange)
        {
            if (progressBar != null)
            {
                progressBar.value = keyHoldTimer / holdTime; // ゲージ更新
            }
            keyHoldTimer += Time.deltaTime;

            if (keyHoldTimer >= holdTime)
            {
                SpawnObject();
                Destroy(this.gameObject);
                Destroy(newParticle);
                keyHoldTimer = 0f;
                if (canvas != null)
                {
                    canvas.gameObject.SetActive(false); // ゲージ非表示
                }
            }
        }
        else
        {
            keyHoldTimer = 0f; // キーを離したら進捗リセット
            if (progressBar != null)
            {
                progressBar.value = 0f; // ゲージリセット
            }
        }
    }

    void SpawnObject()
    {
        List<GameObject> availablePoints = new List<GameObject>(objectToSpawn);
        // 宝箱を開くサウンドを再生
        SoundManager.Instance.Play("宝箱");
        if (objectToSpawn != null)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            Instantiate(objectToSpawn[randomIndex], this.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Object to spawn or spawn point is not set!");
        }
    }
}
