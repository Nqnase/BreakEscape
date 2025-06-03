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
    [Tooltip("�v���C���[�𒵂˕Ԃ��Ƃ��̑���")]
    private float banceSpeed = 30.0f;

    [SerializeField]
    [Tooltip("���˕Ԃ��P�ʃx�N�g���ɂ�����{��")]
    private float bounceVectorMultiple = 2f;

    [SerializeField]
    [Tooltip("�҂���")]
    private float waitTime = 1f;

    [Header("�򉻃V�X�e��")]
    public float cleanRange = 1.0f;
    public Color floorColor = Color.white;  // ���̐F

    private bool isInvincible;

    public void Update()
    {
        CleanArea();
    }

    //�Փ˂����Ƃ�
    private async void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //������������Ƀv���C���[�^�O������Ƃ�
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
            Debug.Log("���G��");

            //�Փ˂����ʂ́A�ڐG�����_�ɂ�����x�N�g�����擾
            Vector3 normal = collision.contacts[0].normal;

            // �Փ˂������x�x�N�g����P�ʃx�N�g���ɂ���
            Vector3 velocity = collision.rigidbody.velocity.normalized;

            //x,z�����ɑ΂��ċt�����̃x�N�g�����擾
            velocity += new Vector3(-normal.x * bounceVectorMultiple, 0f, -normal.z * bounceVectorMultiple);

            //�擾���������ɒ��˕Ԃ�
            collision.rigidbody.AddForce(velocity * banceSpeed * Time.deltaTime, ForceMode.Impulse);

            // ��莞�ԑ҂�
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            collision.gameObject.GetComponent<PlayerController>().moveSpeed = speed;
            collision.rigidbody.isKinematic = true;
            collision.rigidbody.isKinematic = false;
            isInvincible = false;
            Debug.Log("���G����");
        }
    }

    void CleanArea()
    {
        RaycastHit[] hits = Physics.SphereCastAll(
            this.gameObject.transform.position, cleanRange,
            Vector3.up);

        //Debug.Log($"���o���ꂽ�R���C�_�[�̐�: {hits.Length}");

        foreach (var hit in hits)
        {
            //Debug.Log($"���o���ꂽ�I�u�W�F�N�g {hit.collider.name}");
            if (hit.collider.CompareTag("poison"))
            {
                // �^�O�� poison �ɕύX
                hit.collider.tag = "floor";

                // �F��ύX
                hit.collider.GetComponent<Renderer>().material.color = floorColor;
            }
            if (hit.collider.CompareTag("PoisonGas"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

}
