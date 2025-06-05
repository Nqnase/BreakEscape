using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Input System �p
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[")]
    public float moveSpeed = 5f; // �ʏ�̈ړ����x
    public float dashSpeed = 10f; // �_�b�V�����̈ړ����x
    public float rotationSpeed = 720f;

    // �v���C���[�� HP �Ǘ�
    public int maxHealth = 100;
    public int currentHealth;
    public float damagePerSecond = 10f;

    [Header("�_���[�W����")]
    public float damageInterval = 0.5f;
    public float currentDamageInterval;
    private float damageTimer = 0f;

    [Header("�n���}�[�֌W")]
    public GameObject hammer;
    public float swingDuration = 0.5f;
    public float hammerRadius = 1.5f;
    public bool isSwinging = false;
    [Header("�G�t�F�N�g")]
    public GameObject wallDestroyEffectPrefab; // �G�t�F�N�g�̃v���n�u
    [SerializeField] private ParticleSystem poisonEffect;
    private ParticleSystem newParticle;
    private bool effectFlag = false;
    private bool firstEffect = false;

    [Header("�X�^�~�i")]
    public float maxStamina = 100;
    public float currentStamina;
    public float staminaCost = 30f;
    public float dashStaminaCost = 1f;
    public float staminaRecoveryRate = 1f;
    private float staminaRecoveryTimer = 0f;
    public float staminaRecoveryDelay = 3f;
    private float staminaRecoveryWaitTimer = 0f;

    [Header("�E�o�ł��Ȃ���")]
    [SerializeField] private Image dontExit;

    public Slider staminaSlider;

    public float rayDistance = 1.1f;

    [Header("�t���O")]
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

    [Header("�A�j���[�V����")]
    public Animator animator;

    [Header("���E�G�t�F�N�g")]
    [SerializeField] private Volume postProcessVolume; // Post-process Volume
    private Vignette vignette;
    public float lowHPThreshold = 30f; // HP�����Ȃ��Ƃ݂Ȃ�臒l


    private Vector2 moveInput; // ���̓x�N�g��
    private bool isDashingInput; // �_�b�V������


    public float footstepInterval = 0.5f; // �ʏ펞�̑����Ԋu
    public float dashFootstepInterval = 0.3f; // �_�b�V�����̑����Ԋu
    private float footstepTimer = 0f; // �����p�̃^�C�}�[

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

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        newParticle = Instantiate(poisonEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
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
        // ���̓x�N�g�����擾
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        // �_�b�V�����͂��擾
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
            Time.timeScale = 0f; // �Q�[���̎��Ԃ��~
        }
        else
        {
            OpeningMap = false;
            MAP.SetActive(false);
            Time.timeScale = 1f; // �Q�[���̎��Ԃ��ĊJ
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
        UnityEditor.EditorApplication.isPlaying = false; // �Q�[���v���C�I�� 
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

            // �����Đ��̃^�C�~���O���Ǘ�
            footstepTimer += Time.deltaTime;
            float currentFootstepInterval = _isDashing ? dashFootstepInterval : footstepInterval;

            if (footstepTimer >= currentFootstepInterval)
            {
                footstepTimer = 0f;
                SoundManager.Instance.Play("����");
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
            float currentSpeed = move.magnitude * (_isDashing ? dashSpeed : moveSpeed); // ���݂̈ړ����x���v�Z

            animator.SetFloat("Speed", currentSpeed); // �A�j���[�^�[�Ɍ��݂̑��x��n��

            animator.SetBool("isWalking", currentSpeed > 0 && !_isDashing); // ���s�����ǂ���
            animator.SetBool("isSprint", _isDashing); // �_�b�V�������ǂ���
            animator.SetBool("isWaiting", currentSpeed == 0); // �ҋ@��Ԃ��ǂ���
            animator.SetBool("isSwinging", isSwinging); // �n���}�[��U���Ă��邩�ǂ���
        }
    }

    System.Collections.IEnumerator SwingHammer()
    {
        isSwinging = true;
        hammer.SetActive(true);
        SoundManager.Instance.Play("�X�C���O");
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
                // �p�[�e�B�N���𔭐�������B
                newParticle.Play();
                effectFlag = true;
                firstEffect = true;
            }
            else
            {
                Debug.Log("�G�t�F�N�g����");
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


            // �ǂ�j��
            Destroy(other.gameObject);
            Debug.Log("Wall destroyed.");
            SoundManager.Instance.Play("�ǔj��");


            // �G�t�F�N�g�𐶐�
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

        // ���݂�HP��臒l����������ꍇ�̃r�l�b�g���x
        float intensity = Mathf.Clamp01((lowHPThreshold - currentHealth) / lowHPThreshold);
        vignette.intensity.Override(intensity);
    }
}
