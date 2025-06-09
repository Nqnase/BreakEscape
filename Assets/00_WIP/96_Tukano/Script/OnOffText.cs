using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnOffText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpComponent;

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        tmpComponent.enabled = true; // TMP��L����
    //    }
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        tmpComponent.enabled = false; // TMP�𖳌���
    //    }
    //}


    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tmpComponent.enabled = true;
        }

    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tmpComponent.enabled = false;
        }
    }




}
