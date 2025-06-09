using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody m_rigidbody = null;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Å@ÉJÉÅÉâÇÃí«îˆ
        Camera.main.transform.position = this.transform.position + (Vector3.back * 7f);

        //Å@ì¸óÕ

        if(Input.GetKey(KeyCode.W) == true)
        {
            m_rigidbody.AddForce(0f, 0f, 30f);
        }
        if (Input.GetKey(KeyCode.S) == true)
        {
            m_rigidbody.AddForce(0f, 0f, -30f);
        }
        if (Input.GetKey(KeyCode.A) == true)
        {
            m_rigidbody.AddForce(-30f, 0f, 0f);
        }
        if ( Input.GetKey(KeyCode.D) == true)
        {
            m_rigidbody.AddForce(30f, 0f, 0f);
        }
    }
}
