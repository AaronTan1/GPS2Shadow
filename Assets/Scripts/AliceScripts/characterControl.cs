using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerShadow; // all shadows gameObj
    [SerializeField] Image YellowIcon; //RS lower left
    [SerializeField] Image PurpleIcon; //RS higher right
    [SerializeField] Sprite[] UIIcons;
    [SerializeField] Animator PlayerShadowAnimator;
    [SerializeField] Animator PlayerAnimator;

    public static bool holdCandle;
    public static int callOnce = 0;
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
    //2D Shadow Alice
    const string SHADOWALICE_IDLE_RIGHT = "ShadowAliceIdleRight";
    const string SHADOWALICE_IDLE_LEFT = "ShadowAliceIdleLeft";
    const string SHADOWALICE_WALK_RIGHT = "ShadowAliceWalkRight";
    const string SHADOWALICE_WALK_LEFT = "ShadowAliceWalkLeft";
    const string SHADOWALICE_JUMP_LEFT = "ShadowAliceJumpLeft";
    const string SHADOWALICE_JUMP_RIGHT = "ShadowAliceJumpRight";
    const string SHADOWALICE_DEATH = "ShadowAliceDeath"; //temporary used as transition

    //3D Real Alice
    const string ALICE_IDLE = "AliceIdle";
    const string ALICE_IDLE_CANDLE = "AliceIdleCandle";
    const string ALICE_WALK = "AliceWalk";
    const string ALICE_WALK_CANDLE = "AliceWalkCandle";
    const string ALICE_PICK_UP = "AlicePickUp";
    const string ALICE_PUSH = "AlicePush";
    const string ALICE_PULL = "AlicePull";

    // Start is called before the first frame update
    void Start()
    {
        switchMode = false;
        moveSpeed = 13f;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        facingRight = true;
        holdCandle = false;
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        cam = Camera.main;
        callOnce = 0;

        YellowIcon.sprite = UIIcons[0];
        PurpleIcon.sprite = UIIcons[1];
    }

    public void ToggleSwitch()
    {
        if (callOnce == 0) // called once for sec a tuto blight anim only
        {
            callOnce++;
        }

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
            if (facingRight == false)
            {
                ChangeAnimationState(SHADOWALICE_JUMP_LEFT);
            }
            else
            {
                ChangeAnimationState(SHADOWALICE_JUMP_RIGHT);
            }
            
            StartCoroutine(jumpCdr());
        }
    }

    IEnumerator jumpCdr()
    {
        yield return new WaitForSeconds(1.0f);
        jumpDelay = false;

    }

    void ChangeAnimationStateAlice(string newState) //for Alice 3D
    {
        //stop same anim from interrupting itself
        if (currentState == newState) return;

        //play the animation
        PlayerAnimator.Play(newState);

        //reassign current state
        currentState = newState;
    }

    void ChangeAnimationState(string newState) //for Alice 2D
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

            if (playerCandleScript.playAlicePick == false)
            {
                dir = new Vector3(inputX, 0, inputY).normalized;
                Player.GetComponent<Rigidbody>().AddForce(dir * moveSpeed);


                if (PuzzleHandler_Table.playAliceTableAnim == false)
                {
                    if (Player.GetComponent<Rigidbody>().velocity == Vector3.zero && holdCandle == false)
                    {
                        ChangeAnimationStateAlice(ALICE_IDLE);
                    }
                    else if (Player.GetComponent<Rigidbody>().velocity != Vector3.zero && holdCandle == false)
                    {
                        ChangeAnimationStateAlice(ALICE_WALK);
                    }

                    if (Player.GetComponent<Rigidbody>().velocity == Vector3.zero && holdCandle)
                    {
                        ChangeAnimationStateAlice(ALICE_IDLE_CANDLE);
                    }
                    else if (Player.GetComponent<Rigidbody>().velocity != Vector3.zero && holdCandle)
                    {
                        ChangeAnimationStateAlice(ALICE_WALK_CANDLE);
                    }
                }
                else if(PuzzleHandler_Table.playAliceTableAnim)
                {
                    ChangeAnimationStateAlice(ALICE_PUSH); //not working but boolean works
                }

            }
            else if(playerCandleScript.playAlicePick)
            {
                ChangeAnimationStateAlice(ALICE_PICK_UP);
                playerCandleScript.playAlicePick = false;
                holdCandle = true;

            }

        }
        else if (switchMode)
        {
            if(checkpointScript.shadowTransitionAnim == false)
            {
                dir = new Vector3(inputX, 0, 0).normalized;
                PlayerShadow.GetComponent<Rigidbody2D>().AddForce(dir * moveSpeed / 2.5f);
            }


            if (PlayerShadow.GetComponent<Rigidbody2D>().velocity != Vector2.zero && checkpointScript.shadowTransitionAnim == false)
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
            else if(PlayerShadow.GetComponent<Rigidbody2D>().velocity == Vector2.zero && checkpointScript.shadowTransitionAnim == false)
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

            if (checkpointScript.shadowTransitionAnim)
            {
                ChangeAnimationState(SHADOWALICE_DEATH);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(switchMode);*/

        /*Debug.Log(PlayerShadow.GetComponent<Rigidbody2D>().velocity);*/
        /*Debug.Log(PuzzleHandler_Table.playAliceTableAnim);*/

        if (playerCandleScript.restrictMode)
        {
            if(switchMode == false)
            {
                if (holdCandle == false)
                {
                    YellowIcon.sprite = UIIcons[2];
                    PurpleIcon.sprite = UIIcons[1];
                }
                else if (holdCandle == true)
                {
                    YellowIcon.sprite = UIIcons[0];
                }
            }
        }
        else if(playerCandleScript.restrictMode == false)
        {
            if (switchMode)
            {
                YellowIcon.sprite = UIIcons[4];
                PurpleIcon.sprite = UIIcons[3];
            }
            else if(switchMode == false)
            {
                if (PuzzleHandler_Cabinet.inRange) // for cabinet
                {
                    YellowIcon.sprite = UIIcons[6];
                    PurpleIcon.sprite = UIIcons[5];
                }
                else if (PuzzleHandler_Table.inRangeT)
                {
                    if(PuzzleHandler_Table.inSelection)
                    {
                        YellowIcon.sprite = UIIcons[8];
                        PurpleIcon.sprite = UIIcons[9];
                    }
                    else 
                    {
                        YellowIcon.sprite = UIIcons[7];
                        PurpleIcon.sprite = UIIcons[9];
                    }                  
                }
                else
                {
                    YellowIcon.sprite = UIIcons[2];
                    PurpleIcon.sprite = UIIcons[1];
                }
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blight"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
