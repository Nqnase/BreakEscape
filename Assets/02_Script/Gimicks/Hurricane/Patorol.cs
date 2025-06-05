using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patorol : MonoBehaviour
{
    public Transform[] patrolPoints; // ����|�C���g���i�[����z��
    public float speed; // �ړ����x
    public int currentPointIndex = 0; // ���݂̏���|�C���g�̃C���f�b�N�X
    private bool reverse = false;  // �t�����t���O

    void Update()
    {
        // ����|�C���g���ݒ肳��Ă��Ȃ��ꍇ�͉������Ȃ�
        if (patrolPoints.Length == 0) return;

        // ���݂̃|�C���g�֌�����
        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = targetPoint.position - transform.position;

        // �ړ�
        transform.position += direction.normalized * speed * Time.deltaTime;

        // �|�C���g�ɓ��B�������m�F
        if (direction.magnitude < 0.5f) // �������������Ƃ����̃|�C���g��
        {
            if (reverse)
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = patrolPoints.Length - 1;  // �Ō�̃|�C���g�ɖ߂�
                }
            }
            else
            {
                currentPointIndex++;
                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = 0;  // �ŏ��̃|�C���g�ɖ߂�
                }
            }
        }


    }

    // �t�����ɏ��񂷂鏈����ǉ�
    public void ReverseDirection()
    {
        reverse = true;  // �t���O�̐؂�ւ�
        Debug.Log("�ړ������𔽓]�I");
    }


    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // �Փ˂��������Player�^�O���t���Ă���Ƃ�
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("�v���C���[�ɓ�����܂����B");
        }

        if (collision.gameObject.tag == "wall")
        {
            //������
            Destroy(gameObject);
            Debug.Log("�ǂɓ�����܂����B");


        }
    }
}