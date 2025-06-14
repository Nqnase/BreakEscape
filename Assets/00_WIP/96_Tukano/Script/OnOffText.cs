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
    //        tmpComponent.enabled = true; // TMPを有効化
    //    }
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        tmpComponent.enabled = false; // TMPを無効化
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
