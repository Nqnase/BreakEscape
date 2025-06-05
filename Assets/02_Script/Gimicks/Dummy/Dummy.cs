using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
//using UnityEngine.InputSystem.iOS;

public class Dummy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dummyText;

    [SerializeField] private int damage = 10;  //�@�H�炤�_���[�W

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // �v���C���[�ƏՓ˂��������m�F���F�̕ύX�ƃe�L�X�g�̕\��
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("����̓_�~�[��");
            Damage();
            GetComponent<Renderer>().material.color = Color.red;
            dummyText.enabled = true;
        }
    }

    // �v���C���[�����ꂽ���Ƀe�L�X�g������
    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            dummyText.enabled = false;
        }
    }

    // �_���[�W����
    private void Damage()
    {
        //playerController.currentHealth -= damage;
        //if (playerController.currentHealth < playerController.maxHealth)
        //    playerController.currentHealth = playerController.maxHealth;
        playerController.currentHealth -= damage;
    }
}
