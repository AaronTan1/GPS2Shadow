using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerShadow; // all shadows gameObj
    public static bool switchMode; //true = shadowRealm, false = 3d
    private joystickManager joystickManger;
    private Vector3 dir;
    private float inputX, inputZ;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        switchMode = false;
        moveSpeed = 15.0f;
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void ToggleSwitch()
    {
        if(playerCandleScript.restrictMode == false)
        if(switchMode == false)
        {
            switchMode = true;
        }
        else if (switchMode)
        {
            switchMode = false;
        }
    }

    void FixedUpdate()
    {

        //player orientation (not fully functional) [testing]
        /*Player.transform.Rotate(Vector3.up * inputX * (100f * Time.deltaTime));*/

        /*Player.transform.position += new Vector3(inputX * moveSpeed, 0, inputZ * moveSpeed);*/

        inputX = joystickManger.inputHorizontal();
        inputZ = joystickManger.inputVertical();

        if (switchMode == false)
        {
            dir = new Vector3(inputX, 0, inputZ).normalized;
            Player.GetComponent<Rigidbody>().AddForce(dir * moveSpeed);
        }
        else if (switchMode)
        {
            dir = new Vector3(inputX, 0, 0).normalized;
            PlayerShadow.GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed / 2.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(switchMode);
    }
}
