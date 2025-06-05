using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform goal; // ゴール地点のTransform
    public Transform player; // プレイヤーのTransform
    public float heightOffset = 2f; // 矢印の高さオフセット

    void Update()
    {
        if (goal == null || player == null) return;

        // プレイヤーとゴールの位置差を計算
        Vector3 direction = (goal.position - player.position).normalized;

        // 矢印をゴールの方向に向ける
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

        // 矢印をプレイヤーの上に配置
        transform.position = player.position + Vector3.up * heightOffset;
    }
}
