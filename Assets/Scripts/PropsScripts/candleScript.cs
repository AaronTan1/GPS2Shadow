using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candleScript : MonoBehaviour
{
    [SerializeField] public GameObject candleObj;
    [SerializeField] public GameObject floorCandle;
    [SerializeField] public Light lightSource; //candle on hand

    private bool holding = false;
    private bool range = false;

    // Start is called before the first frame update
    void Start()
    {
        holding = false;
        range = false;
    }

    public void ToggleHold()
    {
        if(holding == false && range)
        {
            holding = true;
            candleOnHand();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (holding == false && other.gameObject.name == "Alice")
        {
            range = true;  
        }

    }

    void candleOnHand()
    {
        candleObj.SetActive(true);
        floorCandle.SetActive(false);
        lightSource.intensity = 0.0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (range)
        {
            range = false;
        }
    }

    void candleFlare()
    {
        lightSource.intensity = Mathf.Clamp(Mathf.Cos(Time.time), 0.6f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (candleObj.activeSelf)
        {
            int randNum = Random.Range(1, 3);

            if(randNum == 2)
            {
                Invoke("candleFlare", 0.0f);
            }
           
        }

        Debug.Log("Range" + range);
    }
}
