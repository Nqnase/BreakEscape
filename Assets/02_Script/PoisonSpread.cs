using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSpread : MonoBehaviour
{
    public float interval = 1f; // �ł̊g�U�Ԋu
    public Color poisonColor = Color.green; // �ł̐F
    public Color floorColor = Color.gray;  // ���̐F
    [Header("�ŃK�X�G�t�F�N�g")]
    [SerializeField] private GameObject _poisonEffect;

    // �אڂ������
    private Vector3[] directions = {
        new Vector3(1, 0, 0),  // ��
        new Vector3(-1, 0, 0), // ��
        new Vector3(0, 0, 1),  // �E
        new Vector3(0, 0, -1)  // ��
    };

    void Start()
    {
        // �R���[�`�����J�n
        StartCoroutine(SpreadPoison());
    }

    IEnumerator SpreadPoison()
    {
        while (true)
        {
            // sourceTag �����S�ẴI�u�W�F�N�g����g�U
            GameObject[] sourceObjects = GameObject.FindGameObjectsWithTag("source");
            foreach (GameObject source in sourceObjects)
            {
                SpreadFromSource(source);
            }

            // poison �^�O�����I�u�W�F�N�g���炳��Ɋg�U
            GameObject[] poisonObjects = GameObject.FindGameObjectsWithTag("poison");
            foreach (GameObject poison in poisonObjects)
            {
                SpreadFromPoison(poison);
            }

            // �w�肵���Ԋu�ҋ@
            yield return new WaitForSeconds(interval);
        }
    }

    // source �I�u�W�F�N�g����ł��g�U
    void SpreadFromSource(GameObject source)
    {
        foreach (Vector3 direction in directions)
        {
            CheckAndChangeTag(source, direction);
        }
    }

    // poison �I�u�W�F�N�g����ł��g�U
    void SpreadFromPoison(GameObject poison)
    {
        foreach (Vector3 direction in directions)
        {
            CheckAndChangeTag(poison, direction);
        }
    }

    // �w�肵�������ɗאڂ���I�u�W�F�N�g���`�F�b�N���A�����ɉ����ă^�O��ύX
    void CheckAndChangeTag(GameObject obj, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 origin = obj.transform.position;

        // Ray ���΂��ėאڃI�u�W�F�N�g���m�F
        if (Physics.Raycast(origin, direction, out hit, 1f))
        {
            GameObject neighbor = hit.collider.gameObject;

            // �ǂ�����ꍇ�A���̕����̏����𒆒f
            if (neighbor.CompareTag("wall"))
            {
                //Debug.Log($"Spread blocked by wall at {neighbor.transform.position}");
                return;
            }

            // ���Ȃ�łɕς���
            if (neighbor.CompareTag("floor"))
            {
                // �^�O�� poison �ɕύX
                neighbor.tag = "poison";

                // �F��ύX
                neighbor.GetComponent<Renderer>().material.color = poisonColor;

                // �ŃG�t�F�N�g�𐶐�
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
