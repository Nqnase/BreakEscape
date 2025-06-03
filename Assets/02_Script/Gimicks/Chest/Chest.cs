using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public int itemCount; //�I�u�W�F�N�g�̐�
    public GameObject[] objectToSpawn; // ��������I�u�W�F�N�g
    //public KeyCode spawnKey = KeyCode.Space; // ����������L�[
    public float holdTime = 1f; // ����������K�v�����鎞��

    private float keyHoldTimer = 0f;
    private bool isPlayerInRange = false;

    [Header("UI")]
    [SerializeField] private ParticleSystem chestUI;
    [SerializeField] private ParticleSystem itemUI;
    private ParticleSystem newParticle;
    private Vector3 chestPos;

    [SerializeField]
    private Slider progressBar; // ���L�X���C�_�[
    [SerializeField]
    private Canvas canvas; // ���L�X���C�_�[�̃L�����o�X

    void Start()
    {
        //if (progressBar == null)
        //{
        //    // �V�[�����̃X���C�_�[�ƃL�����o�X������
        //    progressBar = FindObjectOfType<Slider>();
        //    if (progressBar != null)
        //    {
        //        canvas = progressBar.GetComponentInParent<Canvas>();
        //        canvas.gameObject.SetActive(false); // �ŏ��͔�\��
        //        progressBar.value = 0f; // �����l
        //    }
        //    else
        //    {
        //        Debug.LogError("Slider���V�[�����ɑ��݂��܂���");
        //    }
        //}

        chestPos = this.transform.position;

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        newParticle = Instantiate(chestUI);
        newParticle.transform.position = new Vector3
            (chestPos.x, chestPos.y - 0.5f, chestPos.z);
        // �p�[�e�B�N���𔭐�������B
        newParticle.Play();

        canvas.gameObject.SetActive(false); // �ŏ��͔�\��
        progressBar.value = 0f; // �����l
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (canvas != null)
            {
                canvas.gameObject.SetActive(true);
                canvas.transform.position = this.transform.position + Vector3.up; // �󔠂̏�ɕ\��
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            keyHoldTimer = 0f; // �v���C���[���͈͊O�ɏo����^�C�}�[���Z�b�g
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false); // �͈͊O�Ȃ�Q�[�W���\��
            }
            if (progressBar != null)
            {
                progressBar.value = 0f; // �Q�[�W���Z�b�g
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
                progressBar.value = keyHoldTimer / holdTime; // �Q�[�W�X�V
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
                    canvas.gameObject.SetActive(false); // �Q�[�W��\��
                }
            }
        }
        else
        {
            keyHoldTimer = 0f; // �L�[�𗣂�����i�����Z�b�g
            if (progressBar != null)
            {
                progressBar.value = 0f; // �Q�[�W���Z�b�g
            }
        }
    }

    void SpawnObject()
    {
        List<GameObject> availablePoints = new List<GameObject>(objectToSpawn);
        // �󔠂��J���T�E���h���Đ�
        SoundManager.Instance.Play("��");
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
