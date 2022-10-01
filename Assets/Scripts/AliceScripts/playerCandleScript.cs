using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCandleScript : MonoBehaviour
{
    [SerializeField] public GameObject[] floorCandle; //stationary candle
    [SerializeField] public GameObject[] placeCandle; //place candle after hold
    [SerializeField] public GameObject handCandle; //preset candle on hand
    [SerializeField] public Light lightSource; //candleLight on hand
    GameObject childOfPlace; //placeCandle's child
    private string stationName, placeName; //names of stationary and placed candle
    private bool hold = false;
    private bool range = false; // for picking up
    private bool rangePlace = false; // for placing

    // Start is called before the first frame update
    void Start()
    {
        hold = false;
        range = false;
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
        handCandle.SetActive(true);

        for (int i = 0; i < floorCandle.Length; i++)
        {
            if (floorCandle[i].name == stationName)
            {
                floorCandle[i].SetActive(false);
            }
        }

        lightSource.intensity = 0.0f;
    }

    private void candleToPlace()
    {
        handCandle.SetActive(false);

        for(int i = 0; i < placeCandle.Length; i++)
        {
            if(placeCandle[i].name == placeName)
            {
                childOfPlace = placeCandle[i].transform.GetChild(0).gameObject;
                childOfPlace.SetActive(true);
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

        Debug.Log("Range " +range);
        Debug.Log("PlaceName: " + placeName);
    }
}
