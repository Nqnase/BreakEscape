using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patorol : MonoBehaviour
{
    public Transform[] patrolPoints; // 巡回ポイントを格納する配列
    public float speed; // 移動速度
    public int currentPointIndex = 0; // 現在の巡回ポイントのインデックス
    private bool reverse = false;  // 逆方向フラグ

    void Update()
    {
        // 巡回ポイントが設定されていない場合は何もしない
        if (patrolPoints.Length == 0) return;

        // 現在のポイントへ向かう
        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = targetPoint.position - transform.position;

        // 移動
        transform.position += direction.normalized * speed * Time.deltaTime;

        // ポイントに到達したか確認
        if (direction.magnitude < 0.5f) // 距離が小さいとき次のポイントへ
        {
            if (reverse)
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = patrolPoints.Length - 1;  // 最後のポイントに戻る
                }
            }
            else
            {
                currentPointIndex++;
                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = 0;  // 最初のポイントに戻る
                }
            }
        }


    }

    // 逆方向に巡回する処理を追加
    public void ReverseDirection()
    {
        reverse = true;  // フラグの切り替え
        Debug.Log("移動方向を反転！");
    }


    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // 衝突した相手にPlayerタグが付いているとき
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤーに当たりました。");
        }

        if (collision.gameObject.tag == "wall")
        {
            //消える
            Destroy(gameObject);
            Debug.Log("壁に当たりました。");


        }
    }
}