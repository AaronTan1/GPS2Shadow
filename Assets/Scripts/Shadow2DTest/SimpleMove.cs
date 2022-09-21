using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{

    //-----Simple left right for shadow player
    public Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float jumpStr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = 0;
        float zMove = 0;
        if(Input.GetKey(KeyCode.A))
        {
            xMove = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xMove = 1;
        }
        else
        {
            xMove = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpStr, ForceMode.VelocityChange);
        }

        rb.velocity = new Vector3(xMove * speed, rb.velocity.y, zMove * speed) ;
    }
}
