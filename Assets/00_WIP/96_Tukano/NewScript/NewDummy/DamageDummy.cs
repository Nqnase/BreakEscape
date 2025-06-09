using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
//using UnityEngine.InputSystem.iOS;

public class DamageDummy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dummyText;


    //�@PlayerController���Q�l�ɒǉ����Ă��܂�
    //�@Dummy����̒ǉ��R�[�h�@1�@//
    [Header("�v���C���[HP")]  // �v���C���[�� HP �Ǘ�
    public int maxHealth = 200;  //�@�ő�HP
    public int currentHealth;
    public int damage = 10;  //�@�H�炤�_���[�W

    //�@�����܂Ł@//



    //�@Dummy����̒ǉ��R�[�h�@2�@//
    private void Start()
    {
        currentHealth = maxHealth;
    }
    //�@�����܂Ł@//


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // �v���C���[�ƏՓ˂��������m�F���F�̕ύX�ƃe�L�X�g�̕\��
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("����̓_�~�[��");
            GetComponent<Renderer>().material.color = Color.red;
            dummyText.enabled = true;

            //�@Dummy����̒ǉ��R�[�h�@3�@//
            currentHealth -= damage;
            //�@�����܂Ł@//
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
}
