using UnityEngine;

public class Blind : MonoBehaviour
{
    [Header("�͂�ł�󂹂�Ǔ����Ƃ���")]
    [SerializeField] GameObject[] breakObject;

    public void Update()
    {
        for (int i = 0; i < breakObject.Length; i++)
        {
            if (breakObject[i] == null)
            {
                Destroy(this.gameObject);
            }
        }
    }
}