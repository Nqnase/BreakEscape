using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Collections;
using static UnityEngine.ParticleSystem;

public class PlayerItemController : MonoBehaviour
{
    [Header("アイテムスロット")]
    public int maxSlots = 4;
    private List<GameObject> itemSlots = new List<GameObject>(); // アイテムスロット

    [Header("UI")]
    public List<Image> slotImages; // スロットごとのUI画像
    public Sprite emptySlotSprite; // 空のスロットの画像

    public Sprite slot1EmptyImage; // スロット1用の空画像
    public Sprite slot2EmptyImage; // スロット2用の空画像
    public Sprite slot3EmptyImage; // スロット3用の空画像
    public Sprite slot4EmptyImage; // スロット4用の空画像

    public Sprite potionSprite; // ポーションの画像
    public Sprite gasMaskSprite; // ガスマスクの画像
    public Sprite cleaningSprite; // 浄化アイテムの画像
    public Sprite staminaSprite;  // スタミナポーションの画像

    [Header("アイテムエフェクト")]
    [SerializeField] private ParticleSystem potionEffect;    // ポーションのエフェクト
    [SerializeField] private ParticleSystem gasMaskEffect;   // ガスマスクのエフェクト
    [SerializeField] private ParticleSystem cleaningEffect;  // 浄化アイテムのエフェクト
    [SerializeField] private ParticleSystem staminaEffect;   // スタミナポーションのエフェクト

    [Header("浄化アイテム関連")]
    public bool isClean = false;
    public float cleanRange = 1.0f;
    public Color floorColor = Color.white;  // 床の色
    [SerializeField]
    private float loopTime;
    private PlayerController playerController;

    private bool isGasMaskActive = false;
    void Start()
    {
        // スロットを初期化
        for (int i = 0; i < maxSlots; i++)
        {
            itemSlots.Add(null); // 空のスロットを作成
        }

        playerController = FindAnyObjectByType<PlayerController>();

        // スロットUIを初期化
        UpdateSlotUI();
    }

    /// <summary>
    /// アイテムを拾う処理
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion") || other.CompareTag("Cleaning")
            || (other.CompareTag("StaminaPotion")))
        {
            SoundManager.Instance.Play("アイテム入手");
            AddItemToSlot(other.gameObject);
            UpdateSlotUI(); // UIを更新
        }
    }

    /// <summary>
    /// アイテムを拾った時に保存する
    /// </summary>
    /// <param name="item"></param>
    void AddItemToSlot(GameObject item)
    {
        // 空きスロットを探す
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i] == null)
            {
                itemSlots[i] = item;
                item.SetActive(false);
                Debug.Log($"Item added to slot {i + 1}: {item.name} ({item.tag})");
                UpdateSlotUI();
                return;
            }
        }
    }


    /// <summary>
    /// 拾ったアイテムに応じてUIを更新する
    /// </summary>
    void UpdateSlotUI()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (slotImages.Count > i && slotImages[i] != null)
            {
                if (itemSlots[i] == null)
                {
                    // スロットごとに異なる空画像を設定
                    switch (i)
                    {
                        case 0:
                            slotImages[i].sprite = slot1EmptyImage;
                            break;
                        case 1:
                            slotImages[i].sprite = slot2EmptyImage;
                            break;
                        case 2:
                            slotImages[i].sprite = slot3EmptyImage;
                            break;
                        case 3:
                            slotImages[i].sprite = slot4EmptyImage;
                            break;
                        default:
                            slotImages[i].sprite = emptySlotSprite;
                            break;
                    }
                }
                else
                {
                    // アイテムごとに対応する画像を設定
                    switch (itemSlots[i].tag)
                    {
                        case "Potion":
                            slotImages[i].sprite = potionSprite;
                            break;

                        case "gasMask":
                            slotImages[i].sprite = gasMaskSprite;
                            break;

                        case "Cleaning":
                            slotImages[i].sprite = cleaningSprite;
                            break;

                        case "StaminaPotion":
                            slotImages[i].sprite = staminaSprite;
                            break;

                        default:
                            Debug.LogWarning($"Unknown item tag for UI: {itemSlots[i].tag}");
                            slotImages[i].sprite = emptySlotSprite; // デフォルトは空スロット
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// タグによるアイテム効果を発動
    /// </summary>
    /// <param name="tag"></param>
    void ActivateItemEffectByTag(string tag)
    {
        switch (tag)
        {
            case "Potion":
                StartCoroutine(Heal(50));
                break;

            case "Cleaning":
                StartCoroutine(CleanLoop());
                break;

            case "StaminaPotion":
                StartCoroutine(Stamina());
                break;

            default:
                Debug.LogWarning($"Unknown item tag: {tag}. No effect applied.");
                break;
        }
    }

    /// <summary>
    /// アイテムの使用S
    /// </summary>
    /// <param name="slotIndex"></param>
    void UseItem(int slotIndex)
    {
        // 指定スロットのアイテムを使用
        if (slotIndex >= 0 && slotIndex < itemSlots.Count && itemSlots[slotIndex] != null)
        {
            GameObject item = itemSlots[slotIndex];

            ActivateItemEffectByTag(item.tag);
            itemSlots[slotIndex] = null;
            UpdateSlotUI(); 
        }
        else
        {
            Debug.Log($"Slot {slotIndex + 1} is empty!");
        }
    }

    /// <summary>
    /// 押したキーやボタンに応じたアイテムを使用
    /// </summary>
    /// <param name="context"></param>
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed && !playerController.OpeningMap)
        {
            // 入力されたキーまたはボタンに対応するスロット番号を取得 
            string input = context.control.name; 
            int slotIndex = -1;

            // 入力に対応するスロット番号を判定 
            switch (input)
            {
                case "1": // キーボード: スロット1 
                case "buttonNorth": // コントローラー: スロット1 (例: Yボタン) 
                    slotIndex = 0;
                    break;

                case "2": // キーボード: スロット2 
                case "buttonEast": // コントローラー: スロット2 (例: Bボタン) 
                    slotIndex = 1;
                    break;

                case "3": // キーボード: スロット3 
                case "buttonSouth": // コントローラー: スロット3 (例: Aボタン) 
                    slotIndex = 2;
                    break;

                case "4": // キーボード: スロット4 
                case "buttonWest": // コントローラー: スロット4 (例: Xボタン) 
                    slotIndex = 3;
                    break;

                default:
                    Debug.LogWarning($"Invalid input for slot: {input}");
                    break;
            }

            // 有効なスロット番号ならアイテムを使用 
            if (slotIndex >= 0 && slotIndex < maxSlots)
            {
                UseItem(slotIndex);
            }
            else if (slotIndex != -1) // 無効なスロット番号 
            {
                Debug.Log($"Invalid slot number: {slotIndex + 1}");
            }
        }
    }

    ////////////
    ///アイテム効果
    ////////////

    /// <summary>
    /// ヒールポーションの効果
    /// </summary>
    IEnumerator Heal(int healAmount)
    {
        SoundManager.Instance.Play("ポーション");

        // パーティクルシステムのインスタンスを生成する。
        ParticleSystem newParticle = Instantiate(potionEffect);
        // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
        newParticle.transform.position = this.transform.position;
        // パーティクルをプレイヤーの子オブジェクトにする
        newParticle.transform.parent = this.transform;
        // パーティクルを発生させる。
        newParticle.Play();
        playerController.currentHealth += healAmount;
        if (playerController.currentHealth > playerController.maxHealth)
            playerController.currentHealth = playerController.maxHealth;
        float remainTime = newParticle.duration * 2;
        float a = 0;
        while (a < remainTime)
        {
            newParticle.gameObject.transform.rotation = Quaternion.Euler
                (newParticle.gameObject.transform.rotation.x,
                newParticle.gameObject.transform.rotation.y - transform.root.rotation.y,
                newParticle.gameObject.transform.rotation.z);

            yield return new WaitForSeconds(Time.deltaTime);
            a += Time.deltaTime;
        }
        yield return null;
    }

    /// <summary>
    /// 浄化アイテムの効果
    /// </summary>
    /// <returns></returns>
    IEnumerator CleanLoop()
    {
        SoundManager.Instance.Play("浄化アイテム");

        // パーティクルシステムのインスタンスを生成する。
        ParticleSystem newParticle = Instantiate(cleaningEffect);
        // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
        newParticle.transform.position = this.transform.position;
        // パーティクルをプレイヤーの子オブジェクトにする
        newParticle.transform.parent = this.transform;
        // パーティクルを発生させる。
        newParticle.Play();

        float a = 0;
        while (a < loopTime)
        {
            newParticle.gameObject.transform.rotation = Quaternion.Euler
                (newParticle.gameObject.transform.rotation.x,
                newParticle.gameObject.transform.rotation.y - transform.root.rotation.y,
                newParticle.gameObject.transform.rotation.z);

            RaycastHit[] hits = Physics.SphereCastAll(
                this.gameObject.transform.position, cleanRange,
                Vector3.up);

            Debug.Log($"検出されたコライダーの数: {hits.Length}");

            foreach (var hit in hits)
            {
                Debug.Log($"検出されたオブジェクト {hit.collider.name}");
                if (hit.collider.CompareTag("poison"))
                {
                    // タグを poison に変更
                    hit.collider.tag = "floor";

                    // 色を変更
                    hit.collider.GetComponent<Renderer>().material.color = floorColor;
                }
                if (hit.collider.CompareTag("PoisonGas"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
            a += Time.deltaTime;
        }
        yield return null;
    }

    /// <summary>
    /// スタミナポーションの効果
    /// </summary>
    /// <returns></returns>
    IEnumerator Stamina()
    {
        SoundManager.Instance.Play("ポーション");

        // パーティクルシステムのインスタンスを生成する。
        ParticleSystem newParticle = Instantiate(staminaEffect);
        // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
        newParticle.transform.position = this.transform.position;
        // パーティクルをプレイヤーの子オブジェクトにする
        newParticle.transform.parent = this.transform;
        // パーティクルを発生させる。
        newParticle.Play();

        playerController.currentStamina = playerController.maxStamina;

        float remainTime = newParticle.duration * 2;
        float a = 0;
        while (a < remainTime)
        {
            newParticle.gameObject.transform.rotation = Quaternion.Euler
                (newParticle.gameObject.transform.rotation.x,
                newParticle.gameObject.transform.rotation.y - transform.root.rotation.y,
                newParticle.gameObject.transform.rotation.z);

            yield return new WaitForSeconds(Time.deltaTime);
            a += Time.deltaTime;
        }
        yield return null;
    }

}
