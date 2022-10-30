using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerShadow; // all shadows gameObj
    [SerializeField] Animator PlayerShadowAnimator;
    private static bool switchMode; //true = shadowRealm, false = 3d
    private joystickManager joystickManger;
    private string currentState;
    private Vector3 jump;
    private Vector3 dir;
    private float jumpForce = 1.7f;
    private float inputX, inputY;
    private float moveSpeed;
    private bool facingRight;
    private bool jumpDelay;
    Camera cam;

    //Animation States
    const string SHADOWALICE_IDLE_RIGHT = "ShadowAliceIdleRight";
    const string SHADOWALICE_IDLE_LEFT = "ShadowAliceIdleLeft";
    const string SHADOWALICE_WALK_RIGHT = "ShadowAliceWalkRight";
    const string SHADOWALICE_WALK_LEFT = "ShadowAliceWalkLeft";
    const string SHADOWALICE_DEATH = "ShadowAliceDeath";

    // Start is called before the first frame update
    void Start()
    {
        switchMode = false;
        moveSpeed = 14.5f;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        facingRight = true;
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        cam = Camera.main;
    }

    public void ToggleSwitch()
    {
        if (playerCandleScript.restrictMode) return;
        if (PuzzleManager.Instance.disableShadow) return;

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

    void ChangeAnimationState(string newState)
    {
        //stop same anim from interrupting itself
        if (currentState == newState) return;

        //play the animation
        PlayerShadowAnimator.Play(newState);

        //reassign current state
        currentState = newState;
    }

    void FixedUpdate()
    {
        
        Transform camTransform = cam.transform;
        
        inputX = joystickManger.InputHorizontal();
        inputY = joystickManger.InputVertical();
        
        if (PuzzleManager.Instance.disableMovement) return;

        if (switchMode == false)
        {
            Vector3 faceVector = (camTransform.forward * inputY + camTransform.right * inputX).normalized;
            faceVector.y = 0;
            Vector3 rotation = Vector3.RotateTowards(Player.transform.forward, faceVector, 10 * Time.deltaTime, 0f);
            Player.transform.rotation = Quaternion.LookRotation(rotation);

            dir = new Vector3(inputX, 0, inputY).normalized;
            Player.GetComponent<Rigidbody>().AddForce(dir * moveSpeed);
        }
        else if (switchMode)
        {

            dir = new Vector3(inputX, 0, 0).normalized;
            PlayerShadow.GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed / 2.5f);

            if (PlayerShadow.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                if (dir.x > 0)
                {
                    facingRight = true;
                    ChangeAnimationState(SHADOWALICE_WALK_RIGHT);
                }
                else if (dir.x < 0)
                {
                    facingRight = false;
                    ChangeAnimationState(SHADOWALICE_WALK_LEFT);
                }
            }
            else
            {
                if (facingRight)
                {
                    ChangeAnimationState(SHADOWALICE_IDLE_RIGHT);
                }
                else
                {
                    ChangeAnimationState(SHADOWALICE_IDLE_LEFT);
                }              
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(switchMode);*/

        /*Debug.Log(PlayerShadow.GetComponent<Rigidbody2D>().velocity);*/

    }
}
