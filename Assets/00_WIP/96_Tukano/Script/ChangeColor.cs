using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    //void OnCollisionEnter(Collision collision)
    //{
    // �Փ˂��������Player�^�O���t���Ă���Ƃ�
    //if (collision.gameObject.tag == "Player")
    //{
    //    // �������g�̐F��Ԃɕς���
    //    GetComponent<Renderer>().material.color = Color.red;
    //}

    // }
    


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �������g�̐F��Ԃɕς���
            //GetComponent<Renderer>().material.color = Color.red;




            GetComponent<Renderer>().material.color = new Color32(255, 230, 50, 1);
        }

    }
}
