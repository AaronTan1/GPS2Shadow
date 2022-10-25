using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public GameObject shadowAlice;
    public float proceedDelay; //time before entering new Section

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(NewSection());
        }
    }

    IEnumerator NewSection()
    {
        yield return new WaitForSeconds(proceedDelay);
        shadowAlice.transform.position = new Vector3(6.0f, 1.7f, 2.5f);
    }
}
