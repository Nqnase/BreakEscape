using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float powoer = 5f;
    [SerializeField] private float multiple = 2f;

    private bool isInvincible;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("竜巻に当たった");
            Damage();
        }
    }

    public async void Damage()
    {
        // 無敵中は処理しない
        if (isInvincible)
        {
            return;
        }

        isInvincible = true;

        // ノックバック処理
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(-transform.forward * powoer * multiple, ForceMode.VelocityChange);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        isInvincible = false;
    }
}
