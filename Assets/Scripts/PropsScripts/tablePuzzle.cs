using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablePuzzle : MonoBehaviour
{
    [SerializeField] GameObject tableShadow; //shadow of the table
    public FixedJoint connectedBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleInteract()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") //and button is pressed
        {
            /*connectedBody = gameObject.GetComponent<Rigidbody>();*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
