using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBlightBehaviour : MonoBehaviour
{
    [SerializeField] Animator blightAnimator;
    private string currentAnim;

    private const string ATTACK_READY_IDLE = "AttackReadyIdle";
    private const string ATTACK_SLAM = "AttackSlam";
    private const string ATTACK_RETURN_TO_IDLE = "AttackReturnToReadyIdle";
    private const string PLAY_WITH_SCALE = "PlayWithScale";
    private const string PLAY_TO_ATTACK_READY = "PlayToAttackReady";

    void OnEnable()
    {
        SwitchToAnimation(PLAY_WITH_SCALE);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player Dies");
        }
    }
    public void PlayerInRange()
    {
        SwitchToAnimation(PLAY_TO_ATTACK_READY);
        Invoke("PrepareToAttack", 1f);
    }
    public void PrepareToAttack()
    {
        SwitchToAnimation(ATTACK_READY_IDLE);
    }

    public void SlamOnScale()//Externally called
    {
        SwitchToAnimation(ATTACK_SLAM);
        StartCoroutine(ReturnToAttackIdle());
    }
    public void SwitchToAnimation(string anim)
    {
        if (currentAnim == anim) return;

        blightAnimator.Play(anim);
        currentAnim = anim;
    }

    IEnumerator ReturnToAttackIdle()
    {
        yield return new WaitForSeconds(3f);
        SwitchToAnimation(ATTACK_RETURN_TO_IDLE);
        yield return new WaitForSeconds(1f);
        SwitchToAnimation(ATTACK_READY_IDLE);
    }
}
