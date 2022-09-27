using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeShadow : MonoBehaviour
{
    [SerializeField] Transform shadowTR;
    [SerializeField] Transform wallTR;
    [SerializeField] Transform lightTR;
    [SerializeField] float speed;
    [SerializeField] float angledShadowOffset;
    public Rigidbody rb;
    public Rigidbody shadowRB;
    Vector3 originalScale;//Original shadow scale
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        originalScale = shadowTR.localScale;
        CastFakeShadow();//Set initial shadow
        //StartCoroutine(CastShadow());
    }

    void Update()
    {
        //--Testing only, should be pushed in by player

        float xMove = 0;
        float zMove = 0;
        if (Input.GetKey(KeyCode.J))
        {
            xMove = -1;
        }
        else if(Input.GetKey(KeyCode.L))
        {
            xMove = 1;
        }
        else if(Input.GetKey(KeyCode.I))
        {
            zMove = 1;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            zMove = -1;
        }
        else
        {
            xMove = 0;
            zMove = 0;
        }
        
        rb.velocity = new Vector3(xMove * speed, rb.velocity.y, zMove * speed);
        //----------------------------------------


        //shadowRB.velocity = new Vector3(xMove, rb.velocity.y, zMove) * speed;
        if (rb.velocity != Vector3.zero)
        {
            CastFakeShadow();
        }
        

    }

    public void CastFakeShadow()
    {
        float wallZ = wallTR.position.z;


        float distanceToWall = wallZ - transform.position.z;
        float distanceToLight = transform.position.z - lightTR.position.z;

        if (distanceToWall > distanceToLight)
        {
            shadowTR.localScale = originalScale * (distanceToWall / distanceToLight);
        }
        else
        {
            shadowTR.localScale = originalScale;
        }

        shadowTR.position = new Vector3(transform.position.x * angledShadowOffset, shadowTR.position.y, wallZ - 0.01f);

        //Debug.Log("Shadow cast");
    }

}
