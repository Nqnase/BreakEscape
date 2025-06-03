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
            //// 自分自身の色を黒に変える
            //GetComponent<Renderer>().material.color = Color.red;
            //}
            count++;

            if (GetComponent<Renderer>().material.color == Color.black)
            {
                // 自分自身の色を黄に変える
                GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (GetComponent<Renderer>().material.color == Color.yellow)
            {
                // 自分自身の色を赤に変える
                GetComponent<Renderer>().material.color = Color.red;
            }

            if (count > 2)
            {
                Debug.Log("落とし穴だった!!");

                // 現在の座標を取得
                Vector3 currentPosition = collision.transform.position;

                if (Input.GetKey(KeyCode.W) == true)
                {
                    // 新しいZ座標を設定
                    float newZPosition = upRespawnPosition;
                    currentPosition.z -= newZPosition;

                    // 更新された座標を適用
                    collision.transform.position = currentPosition;
                }
                else if (Input.GetKey(KeyCode.A) == true)
                {
                    // 新しいX座標を設定
                    float newXPosition = leftRespawnPosition;
                    currentPosition.x += newXPosition;

                    // 更新された座標を適用
                    collision.transform.position = currentPosition;
                }
                else if (Input.GetKey(KeyCode.S) == true)
                {
                    // 新しいZ座標を設定
                    float newZPosition = downRespawnPosition;
                    currentPosition.z += newZPosition;

                    // 更新された座標を適用
                    collision.transform.position = currentPosition;
                }
                else if (Input.GetKey(KeyCode.D) == true)
                {
                    // 新しいX座標を設定
                    float newXPosition = rightRespawnPosition;
                    currentPosition.x -= newXPosition;

                    // 更新された座標を適用
                    collision.transform.position = currentPosition;
                }
            }

            //GetComponent<Renderer>().material.color = new Color32(255, 230, 50, 1);
        }

    }
}
