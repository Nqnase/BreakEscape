using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
//using UnityEngine.InputSystem.iOS;

public class Dummy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dummyText;

    [SerializeField] private int damage = 10;  //　食らうダメージ

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // プレイヤーと衝突したかを確認し色の変更とテキストの表示
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("これはダミーだ");
            Damage();
            GetComponent<Renderer>().material.color = Color.red;
            dummyText.enabled = true;
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

    // ダメージ処理
    private void Damage()
    {
        //playerController.currentHealth -= damage;
        //if (playerController.currentHealth < playerController.maxHealth)
        //    playerController.currentHealth = playerController.maxHealth;
        playerController.currentHealth -= damage;
    }
}
