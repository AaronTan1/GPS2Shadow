using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControl : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private joystickManager joystickManger;
    private float inputX, inputZ;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0.1f;
        joystickManger = GameObject.Find("joystick_imgBg").GetComponent<joystickManager>();
        
    }

    private void FixedUpdate()
    {
        Player.transform.position += new Vector3(inputX * moveSpeed, 0, inputZ * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        inputX = joystickManger.inputHorizontal();
        inputZ = joystickManger.inputVertical();
    }
}
