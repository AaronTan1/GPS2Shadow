using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCandleScript : MonoBehaviour
{
    [SerializeField] public GameObject[] floorCandle; //stationary candle
    [SerializeField] public GameObject[] placeCandle; //place candle after hold
    [SerializeField] public GameObject handCandle; //preset candle on hand
    [SerializeField] public GameObject[] shadowPropsA; //gameObject shadows
    [SerializeField] public GameObject[] shadowPropsB; //gameObject shadows
    [SerializeField] public Light lightSource; //candleLight on hand
    GameObject childOfPlace; //placeCandle's child
    GameObject childOfChild; //placeCandle's grandson
    Light lightOfChild; //child's reference to light
    public static bool restrictMode; // stops button function before light is placed
    public static bool playAlicePick; //enables anim for picking candle up
    private string stationName, placeName; //names of stationary and placed candle
    /*private bool childIlluminate = false;*/
    private bool hold = false;
    private bool range = false; // for picking up
    private bool rangePlace = false; // for placing

    void Start()
    {
        restrictMode = true; //REMEMBER TO MAKE IT TO TRUE
        hold = false;
        range = false;
        /*childIlluminate = false;*/
        playAlicePick = false;
        stationName = "";
        placeName = "";
    }

    public void ToggleHold()
    {
        if (hold == false && range)
        {
            hold = true;
            candleOnHand();
        }

        if(hold && rangePlace)
        {
            hold = false;
            candleToPlace();
        }

    }

    private void candleOnHand()
    {
        playAlicePick = true;
        Invoke(nameof(CandleInteract), 0.4f);
    
        for (int i = 0; i < floorCandle.Length; i++)
        {
            if (floorCandle[i].name == stationName)
            {
                floorCandle[i].SetActive(false);
            }
        }

        lightSource.intensity = 0.0f;
    }

    void CandleInteract()
    {
        handCandle.SetActive(true);
    }

    private void candleToPlace()
    {
        handCandle.SetActive(false);
        characterControl.holdCandle = false;

        /*        for(int i = 0; i < placeCandle.Length; i++)
                {
                    if(placeCandle[i].name == placeName)
                    {
                        childOfPlace = placeCandle[i].transform.GetChild(0).gameObject;
                        childOfPlace.SetActive(true);
                        childOfChild = placeCandle[i].transform.GetChild(0).gameObject;
                        lightOfChild = childOfChild.GetComponent(typeof(Light)) as Light;

                        if (placeName == "candlePlacePos")
                        {
                            foreach (var t in shadowPropsA)
                            {
                                if (t != null)
                                    t.SetActive(true);
                            }
                        }
                        else if(placeName== "candlePlacePos(1)")
                        {
                            foreach (var t in shadowPropsB)
                            {
                                if (t != null)
                                    t.SetActive(true);
                            }
                        }

                        restrictMode = false;              
                        *//*childIlluminate = true;*//*
                    }*/

        for (int i = 0; i < placeCandle.Length; i++)
        {
            if (placeCandle[i].name == placeName || placeCandle[i].name == "candlePlacePos(1)")
            {
                childOfPlace = placeCandle[i].transform.GetChild(0).gameObject;
                childOfPlace.SetActive(true);
                childOfChild = placeCandle[i].transform.GetChild(0).gameObject;
                lightOfChild = childOfChild.GetComponent(typeof(Light)) as Light;

                if (placeName == "candlePlacePos")
                {
                    foreach (var t in shadowPropsA)
                    {
                        if (t != null)
                            t.SetActive(true);
                    }
                }

                restrictMode = false;
                /*childIlluminate = true;*/
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StationCandle" && range == false)
        {
            stationName = other.gameObject.name;
            range = true;
        }

        if(other.gameObject.tag == "PlaceCandle" && rangePlace == false)
        {
            placeName = other.gameObject.name;
            rangePlace = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (range)
        {
            range = false;
        }

        if (rangePlace)
        {
            rangePlace = false;
        }
    }

    void candleFlare() //oscillates light intensity for handCandle
    {
        lightSource.intensity = Mathf.Clamp(Mathf.Cos(Time.time), 0.6f, 1.0f);
    }

/*    void placeCandleFlare()
    {
        lightOfChild.intensity = Mathf.Clamp(Mathf.Cos(Time.time), 0.0f, 1.5f);
    }*/

    void Update()
    {
        if (handCandle.activeSelf)
        {
            int randNum = Random.Range(1, 3);

            if (randNum == 2)
            {
                Invoke("candleFlare", 0.0f);
            }

        }

/*        if (childIlluminate)
        {
            Invoke("placeCandleFlare", 0.5f);
        }*/
/*        else if(lightOfChild.intensity == 1.5f)
        {
            childIlluminate = false;
        }*/


       // Debug.Log("Range " +range);
    }
}
