using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointScript : MonoBehaviour
{
    public static bool shadowTransitionAnim;
    public GameObject shadowAlice;
    public float proceedDelay; //time before entering new Section
    public Animator doorAnim; //tutorial door

    [SerializeField] private Collider doorColDisable;

    private void Awake()
    {
        shadowTransitionAnim = false;
        doorAnim.enabled = false;
        doorColDisable.isTrigger = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(NewSection());
        }
    }

    IEnumerator NewSection()
    {
        shadowTransitionAnim = true;
        yield return new WaitForSeconds(proceedDelay);
        shadowAlice.transform.position = new Vector3(6.0f, 1.7f, 2.5f);
        doorAnim.enabled = true;
        doorColDisable.isTrigger = true;
        CameraBehaviour.SwapRooms("Room2");
        Invoke(nameof(UnlockTransition), 0.5f);
    }

    void UnlockTransition()
    {
        shadowTransitionAnim = false;
    }

}
