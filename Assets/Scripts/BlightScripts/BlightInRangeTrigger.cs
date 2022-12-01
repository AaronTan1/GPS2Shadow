using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BlightInRangeTrigger : MonoBehaviour
{
    [SerializeField] List<Animator> animControllers = new List<Animator>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {

                foreach (Animator ac in animControllers)
                {
                    ac.SetBool("inRange", true);
                }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(Animator ac in animControllers)
            {
                ac.SetBool("inRange", true);
            }
        }
    }
}
