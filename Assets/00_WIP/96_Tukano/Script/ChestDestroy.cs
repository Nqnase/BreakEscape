using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDestroy : MonoBehaviour
{
    // �������Ɣ��肷��b�����C���X�y�N�^�[����G���悤��
    [SerializeField]
    private int holdtime = 3;

    [Header("��������A�C�e���̃I�u�W�F�N�g")]
    [SerializeField] private GameObject item;
    // E�L�[�������Ă���b�����L�^
    private float time = 0;

    //�@�g���K�[�ɓ������Ă���Ԏ��Ԃ𑝉�������
    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        //�����葱���Ă���Ƃ�
        if (collision.gameObject.tag == "Player")
        {
            //�L�[�������Ă��邩�𔻒�
                    if (Input.GetKey(KeyCode.E))
            {
                // �����Ă���b�������Z
                time += Time.deltaTime;
                Debug.Log(time);
            }

            // ���ꂽ��
            else
            {
                time = 0;
            }

            // �w�肳��Ă��鎞�ԕ����������ꂽ��
            if (time >= holdtime)
            {
                Destroy(gameObject.transform.root.gameObject);
                Instantiate(item);
            }
        }
    }

    //private void OnCollisionStay(UnityEngine.Collision collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        // �L�[�������Ă��邩�𔻒�
    //        if (Input.GetKey(KeyCode.E))
    //        {
    //            // �����Ă���b�������Z
    //            time += Time.deltaTime;
    //            Debug.Log(time);
    //        }
    //        else
    //        {
    //            //time = 0;
    //        }

    //        // �w�肳��Ă��鎞�ԕ����������ꂽ��
    //        if (time >= holdtime)
    //        {
    //            Destroy(gameObject.transform.root.gameObject);
    //            Instantiate(item);
    //        }
    //    }
    //}

    ////�@�g���K�[���痣�ꂽ��
    //private void OnTriggerExit(Collider other)
    //{
    //    time = 0;
    //}
}
