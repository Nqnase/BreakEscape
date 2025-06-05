using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target; // �Ǐ]����I�u�W�F�N�g
    [SerializeField] private float speed = 5f; // �Ǐ]���x
    [SerializeField] private Vector3 offset;  // �^�[�Q�b�g�Ƃ̑��Έʒu

    void Update()
    {
        if (target != null)
        {
            // �^�[�Q�b�g�̈ʒu�Ɍ������ĕ�Ԉړ�
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    // �O������^�[�Q�b�g��ݒ肷�郁�\�b�h
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
