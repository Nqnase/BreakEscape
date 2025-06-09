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
            Debug.Log("ダミーだった!!");

            
            

        }
        else if (collision.gameObject.tag == "Pitfall" && objColor == Color.red)
        {
            Debug.Log("落とし穴だった!!");

            // 現在の座標を取得
            Vector3 currentPosition = transform.position;

            if (Input.GetKey(KeyCode.W) == true)
            {
                // 新しいZ座標を設定
                float newZPosition = 5.0f;
                currentPosition.z -= newZPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.A) == true)
            {
                // 新しいX座標を設定
                float newXPosition = 5.0f;
                currentPosition.x += newXPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.S) == true)
            {
                // 新しいZ座標を設定
                float newZPosition = 5.0f;
                currentPosition.z += newZPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.D) == true)
            {
                // 新しいX座標を設定
                float newXPosition = 5.0f;
                currentPosition.x -= newXPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }

        }



    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Pitfall" && objColor == Color.red)
        {
            // 現在の座標を取得
            Vector3 currentPosition = transform.position;

            if (Input.GetKey(KeyCode.W) == true)
            {
                // 新しいZ座標を設定
                float newZPosition = 5.0f;
                currentPosition.z -= newZPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.A) == true)
            {
                // 新しいX座標を設定
                float newXPosition = 5.0f;
                currentPosition.x += newXPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.S) == true)
            {
                // 新しいZ座標を設定
                float newZPosition = 5.0f;
                currentPosition.z += newZPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }
            else if (Input.GetKey(KeyCode.D) == true)
            {
                // 新しいX座標を設定
                float newXPosition = 5.0f;
                currentPosition.x -= newXPosition;

                // 更新された座標を適用
                transform.position = currentPosition;
            }


        }
    }

    //private void OnCollisionStay(UnityEngine.Collision collision)
    //{
    //    Debug.Log("OnCollisionStay()");
    //}




}
