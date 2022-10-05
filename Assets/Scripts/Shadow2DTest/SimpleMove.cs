using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{

    //-----Simple left right for shadow player
    //public Rigidbody rb = null;
    public Rigidbody2D rb2d;
    [SerializeField] float speed;
    [SerializeField] float jumpStr;
    float moveHorizontal;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");


        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.AddForce(Vector2.up * jumpStr, ForceMode2D.Impulse);
        }
        // Try out this delta time method??
        //rb2d.transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
    }
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
    }
}
