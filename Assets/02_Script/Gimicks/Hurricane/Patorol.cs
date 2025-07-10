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
        if (direction.magnitude < 0.5f)
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

    /// <summary>
    /// 逆方向に巡回する処理を追加
    /// </summary>
    public void ReverseDirection()
    {
        reverse = true;
        //Debug.Log("移動方向を反転！");
    }

    /// <summary>
    /// 壁に衝突した時
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
            Debug.Log("壁に当たりました。");
        }
    }
}