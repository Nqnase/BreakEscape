using UnityEngine;

public class Blind : MonoBehaviour
{
    [Header("囲んでる壊せる壁入れるところ")]
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