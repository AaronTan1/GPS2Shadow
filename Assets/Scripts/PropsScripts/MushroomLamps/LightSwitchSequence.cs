using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchSequence : MonoBehaviour
{
    [SerializeField] float switchInterval;
    [SerializeField] GameObject exclightLeft;
    [SerializeField] GameObject exclightMid;
    [SerializeField] GameObject exclightRight;

    [SerializeField] GameObject plLeft;
    [SerializeField] GameObject plMid;
    [SerializeField] GameObject plRight;

    public Queue<GameObject> lights = new Queue<GameObject>();
    private int lightIndex = 0;
    private void Start()
    {
        exclightLeft.SetActive(false);
        exclightMid.SetActive(false);
        exclightRight.SetActive(false);

        plLeft.SetActive(false);
        plMid.SetActive(false);
        plRight.SetActive(false);

        lights.Enqueue(exclightLeft);
        lights.Enqueue(plLeft);
        lights.Enqueue(exclightMid);
        lights.Enqueue(plMid);
        lights.Enqueue(exclightRight);
        lights.Enqueue(plRight);

        StartCoroutine(SwitchLight());
    }

    IEnumerator SwitchLight()
    {
        while(lights.Count>0)
        {
            GameObject thisLight = lights.Dequeue();
            GameObject pointLight = lights.Dequeue();
            thisLight.SetActive(true);
            pointLight.SetActive(true);

            lights.Enqueue(thisLight);
            lights.Enqueue(pointLight);

            yield return new WaitForSeconds(switchInterval);

            thisLight.SetActive(false);
            pointLight.SetActive(false);
        }
        
    }
}
