using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform goal; // �S�[���n�_��Transform
    public Transform player; // �v���C���[��Transform
    public float heightOffset = 2f; // ���̍����I�t�Z�b�g

    void Update()
    {
        if (goal == null || player == null) return;

        // �v���C���[�ƃS�[���̈ʒu�����v�Z
        Vector3 direction = (goal.position - player.position).normalized;

        // �����S�[���̕����Ɍ�����
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

        // �����v���C���[�̏�ɔz�u
        transform.position = player.position + Vector3.up * heightOffset;
    }
}
