using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    [SerializeField] private float leftRespawnPosition = 1f;
    [SerializeField] private float rightRespawnPosition = 1f;
    [SerializeField] private float downRespawnPosition = 1f;
    [SerializeField] private float upRespawnPosition = 1f;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.black;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //if (GetComponent<Renderer>().material.color == Color.black)
            //{
            //// �������g�̐F�����ɕς���
            //GetComponent<Renderer>().material.color = Color.red;
            //}
            count++;

            if (GetComponent<Renderer>().material.color == Color.black)
            {
                // �������g�̐F�����ɕς���
                GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (GetComponent<Renderer>().material.color == Color.yellow)
            {
                // �������g�̐F��Ԃɕς���
                GetComponent<Renderer>().material.color = Color.red;
            }

            if (count > 2)
            {
                Debug.Log("���Ƃ���������!!");

                // ���݂̍��W���擾
                Vector3 currentPosition = collision.transform.position;

                if (Input.GetKey(KeyCode.W) == true)
                {
                    // �V����Z���W��ݒ�
                    float newZPosition = upRespawnPosition;
                    currentPosition.z -= newZPosition;

                    // �X�V���ꂽ���W��K�p
                    collision.transform.position = currentPosition;
                }
                else if (Input.GetKey(KeyCode.A) == true)
                {
                    // �V����X���W��ݒ�
                    float newXPosition = leftRespawnPosition;
                    currentPosition.x += newXPosition;

                    // �X�V���ꂽ���W��K�p
                    collision.transform.position = currentPosition;
                }
                else if (Input.GetKey(KeyCode.S) == true)
                {
                    // �V����Z���W��ݒ�
                    float newZPosition = downRespawnPosition;
                    currentPosition.z += newZPosition;

                    // �X�V���ꂽ���W��K�p
                    collision.transform.position = currentPosition;
                }
                else if (Input.GetKey(KeyCode.D) == true)
                {
                    // �V����X���W��ݒ�
                    float newXPosition = rightRespawnPosition;
                    currentPosition.x -= newXPosition;

                    // �X�V���ꂽ���W��K�p
                    collision.transform.position = currentPosition;
                }
            }

            //GetComponent<Renderer>().material.color = new Color32(255, 230, 50, 1);
        }

    }
}
