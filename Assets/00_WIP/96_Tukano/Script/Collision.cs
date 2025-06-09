using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public GameObject obj;
    public Color objColor;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {

        obj = collision.gameObject;
        objColor = obj.GetComponent<Renderer>().material.color;

        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Dummy")
        {
            Debug.Log("�_�~�[������!!");

            
            

        }
        else if (collision.gameObject.tag == "Pitfall" && objColor == Color.red)
        {
            Debug.Log("���Ƃ���������!!");

            // ���݂̍��W���擾
            Vector3 currentPosition = transform.position;

            if (Input.GetKey(KeyCode.W) == true)
            {
                // �V����Z���W��ݒ�
                float newZPosition = 5.0f;
                currentPosition.z -= newZPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.A) == true)
            {
                // �V����X���W��ݒ�
                float newXPosition = 5.0f;
                currentPosition.x += newXPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.S) == true)
            {
                // �V����Z���W��ݒ�
                float newZPosition = 5.0f;
                currentPosition.z += newZPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.D) == true)
            {
                // �V����X���W��ݒ�
                float newXPosition = 5.0f;
                currentPosition.x -= newXPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }

        }



    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Pitfall" && objColor == Color.red)
        {
            // ���݂̍��W���擾
            Vector3 currentPosition = transform.position;

            if (Input.GetKey(KeyCode.W) == true)
            {
                // �V����Z���W��ݒ�
                float newZPosition = 5.0f;
                currentPosition.z -= newZPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.A) == true)
            {
                // �V����X���W��ݒ�
                float newXPosition = 5.0f;
                currentPosition.x += newXPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.S) == true)
            {
                // �V����Z���W��ݒ�
                float newZPosition = 5.0f;
                currentPosition.z += newZPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.D) == true)
            {
                // �V����X���W��ݒ�
                float newXPosition = 5.0f;
                currentPosition.x -= newXPosition;

                // �X�V���ꂽ���W��K�p
                transform.position = currentPosition;
            }


        }
    }

    //private void OnCollisionStay(UnityEngine.Collision collision)
    //{
    //    Debug.Log("OnCollisionStay()");
    //}




}
