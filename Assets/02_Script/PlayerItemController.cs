using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Collections;
using static UnityEngine.ParticleSystem;

public class PlayerItemController : MonoBehaviour
{
    [Header("�A�C�e���X���b�g")]
    public int maxSlots = 4;
    private List<GameObject> itemSlots = new List<GameObject>(); // �A�C�e���X���b�g

    [Header("UI")]
    public List<Image> slotImages; // �X���b�g���Ƃ�UI�摜
    public Sprite emptySlotSprite; // ��̃X���b�g�̉摜

    public Sprite slot1EmptyImage; // �X���b�g1�p�̋�摜
    public Sprite slot2EmptyImage; // �X���b�g2�p�̋�摜
    public Sprite slot3EmptyImage; // �X���b�g3�p�̋�摜
    public Sprite slot4EmptyImage; // �X���b�g4�p�̋�摜

    public Sprite potionSprite; // �|�[�V�����̉摜
    public Sprite gasMaskSprite; // �K�X�}�X�N�̉摜
    public Sprite cleaningSprite; // �򉻃A�C�e���̉摜
    public Sprite staminaSprite;  // �X�^�~�i�|�[�V�����̉摜

    [Header("�A�C�e���G�t�F�N�g")]
    [SerializeField] private ParticleSystem potionEffect;    // �|�[�V�����̃G�t�F�N�g
    [SerializeField] private ParticleSystem gasMaskEffect;   // �K�X�}�X�N�̃G�t�F�N�g
    [SerializeField] private ParticleSystem cleaningEffect;  // �򉻃A�C�e���̃G�t�F�N�g
    [SerializeField] private ParticleSystem staminaEffect;   // �X�^�~�i�|�[�V�����̃G�t�F�N�g

    [Header("�򉻃A�C�e���֘A")]
    public bool isClean = false;
    public float cleanRange = 1.0f;
    public Color floorColor = Color.white;  // ���̐F
    [SerializeField]
    private float loopTime;
    private PlayerController playerController;

    private bool isGasMaskActive = false;
    void Start()
    {
        // �X���b�g��������
        for (int i = 0; i < maxSlots; i++)
        {
            itemSlots.Add(null); // ��̃X���b�g���쐬
        }

        playerController = FindAnyObjectByType<PlayerController>();

        // �X���b�gUI��������
        UpdateSlotUI();
    }
    void OnTriggerEnter(Collider other)
    {
        // �A�C�e�����E������
        if (other.CompareTag("Potion") || other.CompareTag("gasMask") || other.CompareTag("Cleaning")
            || (other.CompareTag("StaminaPotion")))
        {
            SoundManager.Instance.Play("�A�C�e������");
            AddItemToSlot(other.gameObject);
            UpdateSlotUI(); // UI���X�V
        }
    }

    void AddItemToSlot(GameObject item)
    {
        // �󂫃X���b�g��T��
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i] == null)
            {
                itemSlots[i] = item; // �A�C�e����ۑ�
                item.SetActive(false); // �E�����A�C�e�����\���ɂ���
                Debug.Log($"Item added to slot {i + 1}: {item.name} ({item.tag})");
                UpdateSlotUI(); // UI���X�V
                return;
            }
        }

        // �X���b�g�����t�̏ꍇ
        Debug.Log("No empty slot available!");
    }



    void UpdateSlotUI()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (slotImages.Count > i && slotImages[i] != null)
            {
                if (itemSlots[i] == null)
                {
                    // �X���b�g���ƂɈقȂ��摜��ݒ�
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
                    // �A�C�e�����ƂɑΉ�����摜��ݒ�
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
                            slotImages[i].sprite = emptySlotSprite; // �f�t�H���g�͋�X���b�g
                            break;
                    }
                }
            }
        }
    }

void ActivateItemEffectByTag(string tag)
    {
        // �^�O�ɂ��A�C�e�����ʂ𔭓�
        switch (tag)
        {
            case "Potion":
                //HealPlayer(25); // �|�[�V�����ŉ�
                StartCoroutine(Heal(50));
                break;

            case "gasMask":
                ActivateGasMask(); // �K�X�}�X�N�œŌ��ʂ��y��
                break;

            case "Cleaning":
                //CleanArea();
                StartCoroutine(CleanLoop());
                break;

            case "StaminaPotion":
                //StaminaPotion();    // �X�^�~�i�|�[�V�����ŃX�^�~�i���t����
                StartCoroutine(Stamina());
                break;

            default:
                Debug.LogWarning($"Unknown item tag: {tag}. No effect applied.");
                break;
        }
    }
    void UseItem(int slotIndex)
    {
        // �w��X���b�g�̃A�C�e�����g�p
        if (slotIndex >= 0 && slotIndex < itemSlots.Count && itemSlots[slotIndex] != null)
        {
            GameObject item = itemSlots[slotIndex];
            Debug.Log($"Using item from slot {slotIndex + 1}: {item.name} ({item.tag})");

            // �A�C�e�����ʂ𔭓�
            ActivateItemEffectByTag(item.tag);

            // �X���b�g����ɂ���
            itemSlots[slotIndex] = null;
            UpdateSlotUI(); // UI���X�V
        }
        else
        {
            Debug.Log($"Slot {slotIndex + 1} is empty!");
        }
    }
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed && !playerController.OpeningMap)
        {
            // ���͂��ꂽ�L�[�܂��̓{�^���ɑΉ�����X���b�g�ԍ����擾 
            string input = context.control.name; // �����ꂽ�L�[�܂��̓{�^���̖��O���擾 
            int slotIndex = -1;

            // ���͂ɑΉ�����X���b�g�ԍ��𔻒� 
            switch (input)
            {
                case "1": // �L�[�{�[�h: �X���b�g1 
                case "buttonNorth": // �R���g���[���[: �X���b�g1 (��: Y�{�^��) 
                    slotIndex = 0;
                    break;

                case "2": // �L�[�{�[�h: �X���b�g2 
                case "buttonEast": // �R���g���[���[: �X���b�g2 (��: B�{�^��) 
                    slotIndex = 1;
                    break;

                case "3": // �L�[�{�[�h: �X���b�g3 
                case "buttonSouth": // �R���g���[���[: �X���b�g3 (��: A�{�^��) 
                    slotIndex = 2;
                    break;

                case "4": // �L�[�{�[�h: �X���b�g4 
                case "buttonWest": // �R���g���[���[: �X���b�g4 (��: X�{�^��) 
                    slotIndex = 3;
                    break;

                default:
                    Debug.LogWarning($"Invalid input for slot: {input}");
                    break;
            }

            // �L���ȃX���b�g�ԍ��Ȃ�A�C�e�����g�p 
            if (slotIndex >= 0 && slotIndex < maxSlots)
            {
                UseItem(slotIndex);
            }
            else if (slotIndex != -1) // �����ȃX���b�g�ԍ� 
            {
                Debug.Log($"Invalid slot number: {slotIndex + 1}");
            }
        }
    }

    ////////////
    ///�A�C�e������
    ////////////


    private void HealPlayer(int healamount)
    {
        SoundManager.Instance.Play("�|�[�V����");

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(potionEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
        newParticle.transform.parent = this.transform;
        // �p�[�e�B�N���𔭐�������B
        newParticle.Play();

        playerController.currentHealth += healamount;
        if (playerController.currentHealth > playerController.maxHealth)
            playerController.currentHealth = playerController.maxHealth;
    }

    IEnumerator Heal(int healAmount)
    {
        SoundManager.Instance.Play("�|�[�V����");

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(potionEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
        newParticle.transform.parent = this.transform;
        // �p�[�e�B�N���𔭐�������B
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

    private void CleanArea()
    {
        SoundManager.Instance.Play("�򉻃A�C�e��");
        RaycastHit[] hits = Physics.SphereCastAll(
            this.gameObject.transform.position, cleanRange,
            Vector3.up);

        Debug.Log($"���o���ꂽ�R���C�_�[�̐�: {hits.Length}");

        foreach (var hit in hits)
        {
            Debug.Log($"���o���ꂽ�I�u�W�F�N�g {hit.collider.name}");
            if (hit.collider.CompareTag("poison"))
            {
                // �^�O�� poison �ɕύX
                hit.collider.tag = "floor";

                // �F��ύX
                hit.collider.GetComponent<Renderer>().material.color = floorColor;
            }
            if (hit.collider.CompareTag("PoisonGas"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    IEnumerator CleanLoop()
    {
        SoundManager.Instance.Play("�򉻃A�C�e��");

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(cleaningEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
        newParticle.transform.parent = this.transform;
        // �p�[�e�B�N���𔭐�������B
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

            Debug.Log($"���o���ꂽ�R���C�_�[�̐�: {hits.Length}");

            foreach (var hit in hits)
            {
                Debug.Log($"���o���ꂽ�I�u�W�F�N�g {hit.collider.name}");
                if (hit.collider.CompareTag("poison"))
                {
                    // �^�O�� poison �ɕύX
                    hit.collider.tag = "floor";

                    // �F��ύX
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



    private void StaminaPotion()
    {
        SoundManager.Instance.Play("�X�^�~�i�|�[�V����");

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(staminaEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
        newParticle.transform.parent = this.transform;
        // �p�[�e�B�N���𔭐�������B
        newParticle.Play();

        playerController.currentStamina = playerController.maxStamina;
    }

    IEnumerator Stamina()
    {
        SoundManager.Instance.Play("�|�[�V����");

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(staminaEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
        newParticle.transform.parent = this.transform;
        // �p�[�e�B�N���𔭐�������B
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

    private void ActivateGasMask()
    {
        SoundManager.Instance.Play("�K�X�}�X�N");

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(gasMaskEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = this.transform.position;
        // �p�[�e�B�N�����v���C���[�̎q�I�u�W�F�N�g�ɂ���
        newParticle.transform.parent = this.transform;
        // �p�[�e�B�N���𔭐�������B
        newParticle.Play();

        if (isGasMaskActive) return;


        playerController.currentDamageInterval = 0;

        Invoke(nameof(DeactiveGasMask), 15);
    }

    private void DeactiveGasMask()
    {
        playerController.currentDamageInterval = 0.2f;
        isGasMaskActive = false;

    }

}
