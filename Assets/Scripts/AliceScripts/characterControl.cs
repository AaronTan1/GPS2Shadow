using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerShadow; // all shadows gameObj except drawerShadow
    public static bool switchMode; //true = shadowRealm, false = 3d
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public static bool isGrounded;
    public bool jumpDelay;
    private joystickManager joystickManger;
    private Vector3 dir;
    private float inputX, inputZ;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        switchMode = false;
        moveSpeed = 15.0f;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        jumpDelay = false;
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void ToggleSwitch()
    {
        if(playerCandleScript.restrictMode == false)
        {
            if (switchMode == false)
            {
                switchMode = true;
            }
            else if (switchMode)
            {
                switchMode = false;
            }
        }
    }

    public void ToggleJump()
    {
        if(playerCandleScript.restrictMode == false && switchMode && isGrounded && jumpDelay == false)
        {
            jumpDelay = true;
            StartCoroutine(jumpCdr());
            PlayerShadow.GetComponent<Rigidbody2D>().AddForce(jump * jumpForce, (ForceMode2D)ForceMode.Impulse);

        }
    }

    IEnumerator jumpCdr()
    {
        yield return new WaitForSeconds(1.0f);
        jumpDelay = false;
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
