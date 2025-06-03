using UnityEngine;
using System.Collections.Generic;

public class RandomInstantiate : MonoBehaviour
{
    [SerializeField] private GameObject key;

    [SerializeField] private GameObject[] spawnPoints;

    [SerializeField] private int keyCount = 1; // 生成するkeyの数

    // Start is called before the first frame update
    void Start()
    {
        // スポーンポイントをリストに変換して管理
        List<GameObject> availablePoints = new List<GameObject>(spawnPoints);

        for (int i = 0; i < keyCount; i++)
        {
            if (availablePoints.Count == 0)
            {
                Debug.LogWarning("スポーンポイントが不足しています。生成できる鍵の数がスポーンポイントの数を超えています。");
                break;
            }

            // ランダムなスポーンポイントを選択
            int randomIndex = Random.Range(0, availablePoints.Count);
            GameObject randomPoint = availablePoints[randomIndex];

            // keyを生成
            Instantiate(key, randomPoint.transform.position, Quaternion.identity);

            // 使用済みのスポーンポイントをリストから削除
            availablePoints.RemoveAt(randomIndex);
        }
    }
}