using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target; // 追従するオブジェクト
    [SerializeField] private float speed = 5f; // 追従速度
    [SerializeField] private Vector3 offset;  // ターゲットとの相対位置

    void Update()
    {
        if (target != null)
        {
            // ターゲットの位置に向かって補間移動
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    // 外部からターゲットを設定するメソッド
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
