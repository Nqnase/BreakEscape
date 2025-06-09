using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
//using UnityEngine.InputSystem.iOS;

public class DamageDummy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dummyText;


    //　PlayerControllerを参考に追加しています
    //　Dummyからの追加コード　1　//
    [Header("プレイヤーHP")]  // プレイヤーの HP 管理
    public int maxHealth = 200;  //　最大HP
    public int currentHealth;
    public int damage = 10;  //　食らうダメージ

    //　ここまで　//



    //　Dummyからの追加コード　2　//
    private void Start()
    {
        currentHealth = maxHealth;
    }
    //　ここまで　//


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // プレイヤーと衝突したかを確認し色の変更とテキストの表示
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("これはダミーだ");
            GetComponent<Renderer>().material.color = Color.red;
            dummyText.enabled = true;

            //　Dummyからの追加コード　3　//
            currentHealth -= damage;
            //　ここまで　//
        }
    }

    // プレイヤーが離れた時にテキストを消す
    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            dummyText.enabled = false;
        }
    }
}
