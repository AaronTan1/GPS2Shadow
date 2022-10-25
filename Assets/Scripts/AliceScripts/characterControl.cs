using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerShadow; // all shadows gameObj
    private static bool switchMode; //true = shadowRealm, false = 3d
    private Vector3 jump;
    private float jumpForce = 1.7f;
    private bool jumpDelay;
    private joystickManager joystickManger;
    private Vector3 dir;
    private float inputX, inputY;
    private float moveSpeed;
    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        switchMode = false;
        moveSpeed = 14.5f;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        cam = Camera.main;
    }

    public void ToggleSwitch()
    {
        if (playerCandleScript.restrictMode) return;
        if (PuzzleHandler_Cabinet.inRange) return;

        switchMode = switchMode switch
        {
            false => true,
            true => false
        };
    }

    public void ToggleJump()
    {
        if(playerCandleScript.restrictMode == false && switchMode && jumpDelay == false)
        {
            jumpDelay = true;
            PlayerShadow.GetComponent<Rigidbody2D>().AddForce(jump * jumpForce, (ForceMode2D)ForceMode.Impulse);
            StartCoroutine(jumpCdr());
        }
    }

    IEnumerator jumpCdr()
    {
        yield return new WaitForSeconds(1.5f);
        jumpDelay = false;

    }

    void FixedUpdate()
    {
        
        Transform camTransform = cam.transform;
        
        inputX = joystickManger.InputHorizontal();
        inputY = joystickManger.InputVertical();
        
        if (PuzzleHandler_Cabinet.inSelection) return;

        if (switchMode == false)
        {
            Vector3 faceVector = (camTransform.forward * inputY + camTransform.right * inputX).normalized;
            faceVector.y = 0;
            Vector3 rotation = Vector3.RotateTowards(Player.transform.forward, faceVector,10 * Time.deltaTime,0f);
            Player.transform.rotation = Quaternion.LookRotation(rotation);

            dir = new Vector3(inputX, 0, inputY).normalized;
            Player.GetComponent<Rigidbody>().AddForce(dir * moveSpeed);
        }
        else if (switchMode)
        {
            dir = new Vector3(inputX, 0, 0).normalized;
            PlayerShadow.GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed / 2.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(switchMode);*/
    }
}
