using UnityEngine;
using System.Collections.Generic;

public class RandomInstantiate : MonoBehaviour
{
    [SerializeField] private GameObject key;

    [SerializeField] private GameObject[] spawnPoints;

    [SerializeField] private int keyCount = 1; // ��������key�̐�

    // Start is called before the first frame update
    void Start()
    {
        // �X�|�[���|�C���g�����X�g�ɕϊ����ĊǗ�
        List<GameObject> availablePoints = new List<GameObject>(spawnPoints);

        for (int i = 0; i < keyCount; i++)
        {
            if (availablePoints.Count == 0)
            {
                Debug.LogWarning("�X�|�[���|�C���g���s�����Ă��܂��B�����ł��錮�̐����X�|�[���|�C���g�̐��𒴂��Ă��܂��B");
                break;
            }

            // �����_���ȃX�|�[���|�C���g��I��
            int randomIndex = Random.Range(0, availablePoints.Count);
            GameObject randomPoint = availablePoints[randomIndex];

            // key�𐶐�
            Instantiate(key, randomPoint.transform.position, Quaternion.identity);

            // �g�p�ς݂̃X�|�[���|�C���g�����X�g����폜
            availablePoints.RemoveAt(randomIndex);
        }
    }
}