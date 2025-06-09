using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    //　この書き方はgameManegerにアタッチされているUIManagerスクリプトを参考にしました


    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] public Slider _HpSlider;

    private DamageDummy Damage;


    //　DamageDummyを探して取得しているらしい
    void Start()
    {
        Damage = FindAnyObjectByType<DamageDummy>();
    }

    void Update()
    {
        //if (player != null)
        //{
            // プレイヤーの HP を表示
            HPText(Damage.currentHealth);

       // }





    }

    void HPText(int currentHealth)
    {
        //　スライダーのvalueをcurrentHealthに
        _HpSlider.value = currentHealth;
        hpText.text = $" {currentHealth}/200";
    }


}
