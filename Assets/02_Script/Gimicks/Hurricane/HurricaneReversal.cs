using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneReversal : MonoBehaviour
{
    // 長押しと判定する秒数をインスペクターから触れるように
    [SerializeField]
    public int holdtime = 3;
    // Eキーを押している秒数を記録
    public float time;

    private Patorol patrol;  // Patorolへの参照
    void Start()
    {
        patrol = GetComponent<Patorol>();  // Patorolコンポーネントを取得
        time = 0;
    }

    //　トリガーに当たっている間
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(time);

        if (other.CompareTag("Player"))
        {

            Debug.Log("buttonがプレイヤーと接触しています！");

            // キーを押しているかを判定
            if (Input.GetKey(KeyCode.E))
            {
                // 押している秒数を加算
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }

            // 長押しされたら
            if (time >= holdtime)
            {

                //　rootはスクリプトがアタッチされているオブジェクトの親をたどる
                //Destroy(gameObject.transform.root.gameObject);

                Debug.Log($"{holdtime}秒間押されました");

                //オブジェクトの色を白に変更する
                GetComponent<Renderer>().material.color = Color.white;

                Patorol[] patrols = FindObjectsOfType<Patorol>();  // 全ての巡回オブジェクトを取得
                foreach (Patorol patrol in patrols)
                {
                    patrol.ReverseDirection();
                }
            }


        }
    }

    //　トリガーから離れたら
    private void OnTriggerExit(Collider other)
    {
        time = 0;
    }
}
