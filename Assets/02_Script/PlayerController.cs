using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Input System 用
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤー")]
    public float moveSpeed = 5f; // 通常の移動速度
    public float dashSpeed = 10f; // ダッシュ時の移動速度
    public float rotationSpeed = 720f;

    // プレイヤーの HP 管理
    public int maxHealth = 100;
    public int currentHealth;
    public float damagePerSecond = 10f;

    [Header("ダメージ処理")]
    public float damageInterval = 0.5f;
    public float currentDamageInterval;
    private float damageTimer = 0f;

    [Header("ハンマー関係")]
    public GameObject hammer;
    public float swingDuration = 0.5f;
    public float hammerRadius = 1.5f;
    public bool isSwinging = false;
    [Header("エフェクト")]
    public GameObject wallDestroyEffectPrefab; // エフェクトのプレハブ
    [SerializeField] private ParticleSystem poisonEffect;
    private ParticleSystem newParticle;
    private bool effectFlag = false;
    private bool firstEffect = false;

    [Header("スタミナ")]
    public float maxStamina = 100;
    public float currentStamina;
    public float staminaCost = 30f;
    public float dashStaminaCost = 1f;
    public float staminaRecoveryRate = 1f;
    private float staminaRecoveryTimer = 0f;
    public float staminaRecoveryDelay = 3f;
    private float staminaRecoveryWaitTimer = 0f;

    [Header("脱出できない時")]
    [SerializeField] private Image dontExit;

    public Slider staminaSlider;

    public float rayDistance = 1.1f;

    [Header("フラグ")]
    public bool _isDead = false;
    public bool _canExit = false;
    public bool _isClear = false;

    public bool _isInPoison = false;
    public bool _isDashing = false;
    public bool _isWalking = false;
    public bool _isWaiting = false;

    public bool OpeningMap = false;

    [SerializeField] public int itemCount = 0;


    [SerializeField] private GameObject MAP;

    [Header("アニメーション")]
    public Animator animator;

    [Header("視界エフェクト")]
    [SerializeField] private Volume postProcessVolume; // Post-process Volume
    private Vignette vignette;
    public float lowHPThreshold = 30f; // HPが少ないとみなす閾値


    private Vector2 moveInput; // 入力ベクトル
    private bool isDashingInput; // ダッシュ入力


    public float footstepInterval = 0.5f; // 通常時の足音間隔
    public float dashFootstepInterval = 0.3f; // ダッシュ時の足音間隔
    private float footstepTimer = 0f; // 足音用のタイマー

    private SoundManager soundManager;
    private UIManager uiManager;
    void Start()
    {
        if (postProcessVolume.profile.TryGet(out Vignette vignetteEffect))
        {
            vignette = vignetteEffect;
        }
        currentHealth = maxHealth;
        currentDamageInterval = damageInterval;

        currentStamina = maxStamina;
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }

        // パーティクルシステムのインスタンスを生成する。
        newParticle = Instantiate(poisonEffect);
        // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
        newParticle.transform.position = this.transform.position;
        // パーティクルをプレイヤーの子オブジェクトにする
        newParticle.transform.parent = this.transform;

        hammer.SetActive(false);
        dontExit.enabled = false;

        uiManager = FindAnyObjectByType<UIManager>();
    }

    void Update()
    {
        if (!_isDead && !_isClear)
        {
            HandleMovement();
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        CheckForPoison();

        if (_isInPoison && !_isDead)
        {
            //ApplyPoisonDamage();
            StartCoroutine(PoisonDamage());
        }
        else
        {
            damageTimer = 0f;
        }

        RecoverStamina();

        if (itemCount == 6)
        {
            _canExit = true;
        }

        UpdateStaminaUI();
        UpdateVignetteEffect();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力ベクトルを取得
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        // ダッシュ入力を取得
        isDashingInput = context.performed;
    }

    public void MapControll(InputAction.CallbackContext context)
    {

        if (uiManager.isPauseing == true)
        { return; }
        if (!OpeningMap)
        {
            OpeningMap = true;
            MAP.SetActive(true);
            Time.timeScale = 0f; // ゲームの時間を停止
        }
        else
        {
            OpeningMap = false;
            MAP.SetActive(false);
            Time.timeScale = 1f; // ゲームの時間を再開
        }
    }


    public void OnSwingHammer(InputAction.CallbackContext context)
    {
        if (context.performed && !isSwinging && currentStamina >= staminaCost)
        {
            UseStamina(staminaCost);
            StartCoroutine(SwingHammer());
            animator.Play("Swinging", -1, 0f);
        }
    }

    public void OndebugMode(InputAction.CallbackContext context)
    {
        currentHealth = 2100000000;
        maxStamina = 21000000000;
        currentStamina = 21000000000;
    }
    public void OnloadScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnCloseGame(InputAction.CallbackContext context)
    {


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ゲームプレイ終了 
#else
            Application.Quit(); 
#endif

    }


    void HandleMovement()
    {
        if (isSwinging)
        {
            _isWalking = false;
            _isDashing = false;
            _isWaiting = true;
            HandleAnimation(Vector3.zero);

            return;
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (move != Vector3.zero)
        {
            _isWaiting = false;
            _isWalking = true;

            // 足音再生のタイミングを管理
            footstepTimer += Time.deltaTime;
            float currentFootstepInterval = _isDashing ? dashFootstepInterval : footstepInterval;

            if (footstepTimer >= currentFootstepInterval)
            {
                footstepTimer = 0f;
                SoundManager.Instance.Play("足音");
            }
        }
        else
        {
            _isWaiting = true;
            _isWalking = false;
        }

        _isDashing = isDashingInput && currentStamina > 0 && _isWalking;
        if (_isDashing == true)
        {
            UseStamina(dashStaminaCost);
        }

        float speed = _isDashing ? dashSpeed : moveSpeed;
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        HandleAnimation(move);
    }

    void HandleAnimation(Vector3 move)
    {
        if (animator != null)
        {
            float currentSpeed = move.magnitude * (_isDashing ? dashSpeed : moveSpeed); // 現在の移動速度を計算

            animator.SetFloat("Speed", currentSpeed); // アニメーターに現在の速度を渡す

            animator.SetBool("isWalking", currentSpeed > 0 && !_isDashing); // 歩行中かどうか
            animator.SetBool("isSprint", _isDashing); // ダッシュ中かどうか
            animator.SetBool("isWaiting", currentSpeed == 0); // 待機状態かどうか
            animator.SetBool("isSwinging", isSwinging); // ハンマーを振っているかどうか
        }
    }

    System.Collections.IEnumerator SwingHammer()
    {
        isSwinging = true;
        hammer.SetActive(true);
        SoundManager.Instance.Play("スイング");
        float startAngle = 90f;
        float endAngle = -90f;
        float elapsedTime = 0f;

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / swingDuration;

            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            // PositionHammerAtAngle(currentAngle);




            yield return null;
        }



        hammer.SetActive(false);
        isSwinging = false;
        _isWaiting = true;

        yield return null;
    }

    void PositionHammerAtAngle(float angle)
    {
        Vector3 forward = transform.forward.normalized;
        Vector3 right = transform.right.normalized;

        Vector3 offset = (forward * Mathf.Cos(Mathf.Deg2Rad * angle) +
                          right * Mathf.Sin(Mathf.Deg2Rad * angle)) * hammerRadius;

        hammer.transform.position = transform.position + offset;
        hammer.transform.rotation = Quaternion.LookRotation(forward);
    }

    void RecoverStamina()
    {
        if (isSwinging || _isDashing)
        {
            staminaRecoveryWaitTimer = 0f;
            return;
        }

        staminaRecoveryWaitTimer += Time.deltaTime;

        if (staminaRecoveryWaitTimer >= staminaRecoveryDelay)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    void Die()
    {
        _isDead = true;
        gameObject.SetActive(false);
        Debug.Log("Player is Dead!");
    }

    void CheckForPoison()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("poison") || hit.collider.CompareTag("source"))
            {
                _isInPoison = true;
            }
            else
            {
                _isInPoison = false;

                newParticle.Stop();
                effectFlag = false;
            }

            if (hit.collider.CompareTag("Exit"))
            {
                if (_canExit)
                {
                    gameObject.SetActive(false);
                    _isClear = true;
                    Debug.Log("Game Cleared!");
                }
                else
                {
                    dontExit.enabled = true;
                }
            }
        }
        else
        {
            _isInPoison = false;
        }
    }

    void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Exit") && !_canExit)
        {
            dontExit.enabled = false;
        }
    }


    void ApplyPoisonDamage()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= currentDamageInterval)
        {


            currentHealth -= Mathf.CeilToInt(damagePerSecond * currentDamageInterval);
            //Debug.Log($"Poison Damage! Current Health: {currentHealth}"); 

            damageTimer = 0f;
        }
    }

    IEnumerator PoisonDamage()
    {
        if (!effectFlag)
        {
            if (!firstEffect)
            {
                // パーティクルを発生させる。
                newParticle.Play();
                effectFlag = true;
                firstEffect = true;
            }
            else
            {
                Debug.Log("エフェクト発生");
                newParticle.Play();
                effectFlag = true;
            }
        }
        damageTimer += Time.deltaTime;

        if (damageTimer >= currentDamageInterval)
        {
            currentHealth -= Mathf.CeilToInt(damagePerSecond * currentDamageInterval);


            damageTimer = 0f;
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (isSwinging && other.CompareTag("wall"))
        {
            Vector3 wallPosition = other.transform.position;


            // 壁を破壊
            Destroy(other.gameObject);
            Debug.Log("Wall destroyed.");
            SoundManager.Instance.Play("壁破壊");


            // エフェクトを生成
            if (wallDestroyEffectPrefab != null)
            {
                Instantiate(wallDestroyEffectPrefab, wallPosition, Quaternion.identity);
            }
        }
    }

    void UseStamina(float amount)
    {
        if (!OpeningMap)
        {
            currentStamina -= amount;

            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            staminaRecoveryWaitTimer = 0f;
        }
    }

    private void UpdateVignetteEffect()
    {
        if (vignette == null) return;

        // 現在のHPが閾値を下回った場合のビネット強度
        float intensity = Mathf.Clamp01((lowHPThreshold - currentHealth) / lowHPThreshold);
        vignette.intensity.Override(intensity);
    }
}
