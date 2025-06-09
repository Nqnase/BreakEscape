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
            Debug.Log("�����ɓ�������");
            Damage();
        }
    }

    public async void Damage()
    {
        // ���G���͏������Ȃ�
        if (isInvincible)
        {
            return;
        }

        isInvincible = true;

        // �m�b�N�o�b�N����
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(-transform.forward * powoer * multiple, ForceMode.VelocityChange);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        isInvincible = false;
    }
}
