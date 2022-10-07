using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private joystickManager joystickManger;
    private Vector3 dir;
    private float inputX, inputZ;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 20.0f;
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {

        //player orientation (not fully functional)
        Player.transform.Rotate(Vector3.up * inputX * (1000f * Time.deltaTime));



        /*Player.transform.position += new Vector3(inputX * moveSpeed, 0, inputZ * moveSpeed);*/
        Player.GetComponent<Rigidbody>().AddForce(dir * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        inputX = joystickManger.inputHorizontal();
        inputZ = joystickManger.inputVertical();

        dir = new Vector3(inputX, 0, inputZ).normalized;


        

        
    }
}
