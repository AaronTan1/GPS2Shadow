using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        characterControl.isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
