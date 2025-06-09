using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    //void OnCollisionEnter(Collision collision)
    //{
    // 衝突した相手にPlayerタグが付いているとき
    //if (collision.gameObject.tag == "Player")
    //{
    //    // 自分自身の色を赤に変える
    //    GetComponent<Renderer>().material.color = Color.red;
    //}

    // }
    


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 自分自身の色を赤に変える
            //GetComponent<Renderer>().material.color = Color.red;




            GetComponent<Renderer>().material.color = new Color32(255, 230, 50, 1);
        }

    }
}
