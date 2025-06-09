using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PitTrap : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pitText;

    private int count = 0;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            count++;
            if (count > 0)
            {
                GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("°‚É‚Ð‚Ñ‚ª“ü‚Á‚½");
            }
            if(count>1)
            {
                GetComponent<Renderer>().material.color= Color.black;
                Debug.Log("—Ž‚Æ‚µŒŠ‚¾!");
            }
        }
    }
}
