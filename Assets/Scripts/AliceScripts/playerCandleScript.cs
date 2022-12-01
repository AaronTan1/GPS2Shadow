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
    [SerializeField] public GameObject candleHolder; //candleLight on hand
    [SerializeField] public Light lightSource; //candleLight on hand
    [SerializeField] private Light[] placePosIllumObj; //placePos 
    [SerializeField] private Animator blightAnim; //only start blight sec A tuto
    GameObject childOfPlace; //placeCandle's child
    GameObject childOfChild; //placeCandle's grandson
    Light lightOfChild; //child's reference to light
    public static bool restrictMode; // stops button function before light is placed
    public static bool playAlicePick; //enables anim for picking candle up
    public static bool placePosHandIndi; //candle on hand with range indi
    public static bool blightAgro;
    private string stationName, placeName; //names of stationary and placed candle
    /*private bool childIlluminate = false;*/
    private bool hold = false;
    private bool range = false; // for picking up
    private bool rangePlace = false; // for placing
    private bool litHandCandle = false;
    private bool stationFade = false;
    private bool placeIllum = false;

    void Start()
    {
        restrictMode = true; //REMEMBER TO MAKE IT TO TRUE
        hold = false;
        range = false;
        /*childIlluminate = false;*/
        playAlicePick = false;
        stationName = "";
        placeName = "";

        handCandle.SetActive(false);
        handCandle.GetComponentInChildren<Light>().range = 0.0f;
        litHandCandle = false;
        placeIllum = false;
        placePosHandIndi = false;
        blightAgro = false;
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
        placePosHandIndi = true;
        candleHolder.SetActive(true);
        Invoke(nameof(CandleInteract), 0.1f);
    
        for (int i = 0; i < floorCandle.Length; i++)
        {
            if (floorCandle[i].name == stationName)
            {
                /*floorCandle[i].SetActive(false);*/
                stationFade = true;
            }
        }

        lightSource.intensity = 0.0f;
    }

    void CandleInteract()
    {
        handCandle.SetActive(true);
       
        if (litHandCandle)
        {
            litHandCandle = false;
        }
        else
        {
            litHandCandle = true;
        }

    }

    private void candleToPlace()
    {
        handCandle.SetActive(false);
        placePosHandIndi = false;
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
            if (placeCandle[i].name == placeName || placeCandle[i].name == "candleStand (1)")
            {           

                childOfPlace = placeCandle[i].transform.GetChild(0).gameObject;
                childOfChild = childOfPlace.transform.GetChild(0).gameObject;
                lightOfChild = childOfChild.GetComponent(typeof(Light)) as Light;
                childOfPlace.SetActive(true);
                lightOfChild.intensity = 0.0f;     

                placeIllum = true;

                if (placeName == "candleStand")
                {
                    foreach (var t in shadowPropsA)
                    {
                        if (t != null)
                            t.SetActive(true);
                    }
                }

                //ASDGGREG
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
        if(characterControl.callOnce == 1)
        {
            blightAnim.SetTrigger("Entrance");
        }

        if(blightAgro)
        {
            blightAgro = false;
            blightAnim.SetBool("Seek", true);
        }

        if (handCandle.activeSelf)
        {
            int randNum = Random.Range(1, 3);

            if (randNum == 2)
            {
                Invoke("candleFlare", 0.0f);
            }

        }

        if (stationFade)
        {
            if(floorCandle[0].GetComponentInChildren<Light>().range <= 0.0f)
            {
                stationFade = false;
                floorCandle[0].SetActive(false);
            }
            floorCandle[0].GetComponentInChildren<Light>().range -= Mathf.Clamp(6.0f * Time.deltaTime, 0.0f, 6.0f);
        }


        if (litHandCandle)
        {
            if (handCandle.GetComponentInChildren<Light>().range >= 5.0f)
            {
                litHandCandle = false;
            }


            handCandle.GetComponentInChildren<Light>().range += Mathf.Clamp(0.5f * 3.0f * Time.deltaTime, 0.0f, 5.0f);
        }

        if (placeIllum)
        {
            if (placePosIllumObj[0].intensity > 0.5f)
            {
                placeIllum = false;
            }
            placePosIllumObj[0].intensity += Mathf.Clamp(0.4f * Time.deltaTime, 0.0f, 1.0f);

            if (placePosIllumObj[1].intensity > 0.5f)
            {
                placeIllum = false;
            }
            placePosIllumObj[1].intensity += Mathf.Clamp(0.4f * Time.deltaTime, 0.0f, 1.0f);
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
