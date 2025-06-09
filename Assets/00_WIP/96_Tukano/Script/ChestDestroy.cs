using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDestroy : MonoBehaviour
{
    // 長押しと判定する秒数をインスペクターから触れるように
    [SerializeField]
    private int holdtime = 3;

    [Header("生成するアイテムのオブジェクト")]
    [SerializeField] private GameObject item;
    // Eキーを押している秒数を記録
    private float time = 0;

    //　トリガーに当たっている間時間を増加させる
    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        //当たり続けているとき
        if (collision.gameObject.tag == "Player")
        {
            //キーを押しているかを判定
                    if (Input.GetKey(KeyCode.E))
            {
                // 押している秒数を加算
                time += Time.deltaTime;
                Debug.Log(time);
            }

            // 離れた時
            else
            {
                time = 0;
            }

            // 指定されている時間分長押しされたら
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
    //        // キーを押しているかを判定
    //        if (Input.GetKey(KeyCode.E))
    //        {
    //            // 押している秒数を加算
    //            time += Time.deltaTime;
    //            Debug.Log(time);
    //        }
    //        else
    //        {
    //            //time = 0;
    //        }

    //        // 指定されている時間分長押しされたら
    //        if (time >= holdtime)
    //        {
    //            Destroy(gameObject.transform.root.gameObject);
    //            Instantiate(item);
    //        }
    //    }
    //}

    ////　トリガーから離れたら
    //private void OnTriggerExit(Collider other)
    //{
    //    time = 0;
    //}
}
