using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    //�@���̏�������gameManeger�ɃA�^�b�`����Ă���UIManager�X�N���v�g���Q�l�ɂ��܂���


    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] public Slider _HpSlider;

    private DamageDummy Damage;


    //�@DamageDummy��T���Ď擾���Ă���炵��
    void Start()
    {
        Damage = FindAnyObjectByType<DamageDummy>();
    }

    void Update()
    {
        //if (player != null)
        //{
            // �v���C���[�� HP ��\��
            HPText(Damage.currentHealth);

       // }





    }

    void HPText(int currentHealth)
    {
        //�@�X���C�_�[��value��currentHealth��
        _HpSlider.value = currentHealth;
        hpText.text = $" {currentHealth}/200";
    }


}
