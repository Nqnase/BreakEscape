using UnityEngine;

public class Blind : MonoBehaviour
{
    [Header("ˆÍ‚ñ‚Å‚é‰ó‚¹‚é•Ç“ü‚ê‚é‚Æ‚±‚ë")]
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