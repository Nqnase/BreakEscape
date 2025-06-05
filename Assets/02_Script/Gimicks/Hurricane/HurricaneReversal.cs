using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneReversal : MonoBehaviour
{
    // �������Ɣ��肷��b�����C���X�y�N�^�[����G���悤��
    [SerializeField]
    public int holdtime = 3;
    // E�L�[�������Ă���b�����L�^
    public float time;

    private Patorol patrol;  // Patorol�ւ̎Q��
    void Start()
    {
        patrol = GetComponent<Patorol>();  // Patorol�R���|�[�l���g���擾
        time = 0;
    }

    //�@�g���K�[�ɓ������Ă����
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(time);

        if (other.CompareTag("Player"))
        {

            Debug.Log("button���v���C���[�ƐڐG���Ă��܂��I");

            // �L�[�������Ă��邩�𔻒�
            if (Input.GetKey(KeyCode.E))
            {
                // �����Ă���b�������Z
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }

            // ���������ꂽ��
            if (time >= holdtime)
            {

                //�@root�̓X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̐e�����ǂ�
                //Destroy(gameObject.transform.root.gameObject);

                Debug.Log($"{holdtime}�b�ԉ�����܂���");

                //�I�u�W�F�N�g�̐F�𔒂ɕύX����
                GetComponent<Renderer>().material.color = Color.white;

                Patorol[] patrols = FindObjectsOfType<Patorol>();  // �S�Ă̏���I�u�W�F�N�g���擾
                foreach (Patorol patrol in patrols)
                {
                    patrol.ReverseDirection();
                }
            }


        }
    }

    //�@�g���K�[���痣�ꂽ��
    private void OnTriggerExit(Collider other)
    {
        time = 0;
    }
}
