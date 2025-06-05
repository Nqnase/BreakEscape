using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePlayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("プレイヤーを跳ね返すときの速さ")]
    private float banceSpeed = 30.0f;

    [SerializeField]
    [Tooltip("跳ね返す単位ベクトルにかける倍数")]
    private float bounceVectorMultiple = 2f;

    [SerializeField]
    [Tooltip("待つ時間")]
    private float waitTime = 1f;

    [Header("浄化システム")]
    public float cleanRange = 1.0f;
    public Color floorColor = Color.white;  // 床の色

    private bool isInvincible;

    public void Update()
    {
        CleanArea();
    }

    //衝突したとき
    private async void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //当たった相手にプレイヤータグがあるとき
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isInvincible)
            {
                return;
            }
            float speed = collision.gameObject.GetComponent<PlayerController>().moveSpeed;
            collision.gameObject.GetComponent<PlayerController>().moveSpeed = 0;
            //collision.gameObject.GetComponent<PlayerController>().

            isInvincible = true;
            Debug.Log("無敵中");

            //衝突した面の、接触した点におけるベクトルを取得
            Vector3 normal = collision.contacts[0].normal;

            // 衝突した速度ベクトルを単位ベクトルにする
            Vector3 velocity = collision.rigidbody.velocity.normalized;

            //x,z方向に対して逆向きのベクトルを取得
            velocity += new Vector3(-normal.x * bounceVectorMultiple, 0f, -normal.z * bounceVectorMultiple);

            //取得した方向に跳ね返す
            collision.rigidbody.AddForce(velocity * banceSpeed * Time.deltaTime, ForceMode.Impulse);

            // 一定時間待つ
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            collision.gameObject.GetComponent<PlayerController>().moveSpeed = speed;
            collision.rigidbody.isKinematic = true;
            collision.rigidbody.isKinematic = false;
            isInvincible = false;
            Debug.Log("無敵解除");
        }
    }

    void CleanArea()
    {
        RaycastHit[] hits = Physics.SphereCastAll(
            this.gameObject.transform.position, cleanRange,
            Vector3.up);

        //Debug.Log($"検出されたコライダーの数: {hits.Length}");

        foreach (var hit in hits)
        {
            //Debug.Log($"検出されたオブジェクト {hit.collider.name}");
            if (hit.collider.CompareTag("poison"))
            {
                // タグを poison に変更
                hit.collider.tag = "floor";

                // 色を変更
                hit.collider.GetComponent<Renderer>().material.color = floorColor;
            }
            if (hit.collider.CompareTag("PoisonGas"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

}
