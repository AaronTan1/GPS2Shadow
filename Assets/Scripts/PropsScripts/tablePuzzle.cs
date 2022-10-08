using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablePuzzle : MonoBehaviour
{
    [SerializeField] GameObject tableShadow; //shadow of the table
    private bool interact, range;

    // Start is called before the first frame update
    void Start()
    {
        interact = false;
        range = false;
    }

    public void ToggleInteract()
    {
        if(interact == false && range)
        {
            interact = true;
        }
        else if (interact && range)
        {
            interact = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && range == false)
        {
            range = true;
        }

        if (other.gameObject.tag == "Player" && interact && range) 
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (interact == false)
        {
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (interact && range)
        {

        }
    }
}
