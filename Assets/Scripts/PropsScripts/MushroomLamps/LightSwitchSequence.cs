using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchSequence : MonoBehaviour
{
    [SerializeField] float switchInterval;
    [SerializeField] GameObject exclightLeft;
    [SerializeField] GameObject exclightMid;
    [SerializeField] GameObject exclightRight;
    public Queue<GameObject> lights = new Queue<GameObject>();
    private int lightIndex = 0;
    private void Start()
    {
        exclightLeft.SetActive(false);
        exclightMid.SetActive(false);
        exclightRight.SetActive(false);

        lights.Enqueue(exclightLeft);
        lights.Enqueue(exclightMid);
        lights.Enqueue(exclightRight);
        StartCoroutine(SwitchLight());
    }

    IEnumerator SwitchLight()
    {
        while(lights.Count>0)
        {
            GameObject thisLight = lights.Dequeue();
            thisLight.SetActive(true);
            lights.Enqueue(thisLight);
            yield return new WaitForSeconds(switchInterval);
            thisLight.SetActive(false);
        }
        
    }
}
