using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSpread : MonoBehaviour
{
    public float interval = 1f; // 毒の拡散間隔
    public Color poisonColor = Color.green; // 毒の色
    public Color floorColor = Color.gray;  // 床の色
    [Header("毒ガスエフェクト")]
    [SerializeField] private GameObject _poisonEffect;

    // 隣接する方向
    private Vector3[] directions = {
        new Vector3(1, 0, 0),  // 上
        new Vector3(-1, 0, 0), // 下
        new Vector3(0, 0, 1),  // 右
        new Vector3(0, 0, -1)  // 左
    };

    void Start()
    {
        // コルーチンを開始
        StartCoroutine(SpreadPoison());
    }

    IEnumerator SpreadPoison()
    {
        while (true)
        {
            // sourceTag を持つ全てのオブジェクトから拡散
            GameObject[] sourceObjects = GameObject.FindGameObjectsWithTag("source");
            foreach (GameObject source in sourceObjects)
            {
                SpreadFromSource(source);
            }

            // poison タグを持つオブジェクトからさらに拡散
            GameObject[] poisonObjects = GameObject.FindGameObjectsWithTag("poison");
            foreach (GameObject poison in poisonObjects)
            {
                SpreadFromPoison(poison);
            }

            // 指定した間隔待機
            yield return new WaitForSeconds(interval);
        }
    }

    // source オブジェクトから毒を拡散
    void SpreadFromSource(GameObject source)
    {
        foreach (Vector3 direction in directions)
        {
            CheckAndChangeTag(source, direction);
        }
    }

    // poison オブジェクトから毒を拡散
    void SpreadFromPoison(GameObject poison)
    {
        foreach (Vector3 direction in directions)
        {
            CheckAndChangeTag(poison, direction);
        }
    }

    // 指定した方向に隣接するオブジェクトをチェックし、条件に応じてタグを変更
    void CheckAndChangeTag(GameObject obj, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 origin = obj.transform.position;

        // Ray を飛ばして隣接オブジェクトを確認
        if (Physics.Raycast(origin, direction, out hit, 1f))
        {
            GameObject neighbor = hit.collider.gameObject;

            // 壁がある場合、その方向の処理を中断
            if (neighbor.CompareTag("wall"))
            {
                //Debug.Log($"Spread blocked by wall at {neighbor.transform.position}");
                return;
            }

            // 床なら毒に変える
            if (neighbor.CompareTag("floor"))
            {
                // タグを poison に変更
                neighbor.tag = "poison";

                // 色を変更
                neighbor.GetComponent<Renderer>().material.color = poisonColor;

                // 毒エフェクトを生成
                if (_poisonEffect != null)
                {
                    Instantiate(_poisonEffect, neighbor.transform.position, Quaternion.identity);
                }
                else
                {
                    //Debug.LogWarning("Poison effect prefab is not assigned!");
                }

                //Debug.Log($"Spread poison to {neighbor.transform.position}");
            }
        }
    }
}
