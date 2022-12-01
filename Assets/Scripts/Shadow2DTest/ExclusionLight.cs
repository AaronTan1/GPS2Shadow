using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExclusionLight : MonoBehaviour
{
    [SerializeField] float extentScaleIncreaseX;
    [SerializeField] float extentScaleIncreaseY;
    private Collider2D thisBounds; //Exclusion own bounds
    Queue<KeyValuePair<Collider2D, Vector2>> collidedList = new Queue<KeyValuePair<Collider2D, Vector2>>();
    Coroutine crCheck = null;

    private void Start()
    {
        thisBounds = GetComponent<Collider2D>();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blight") || collision.CompareTag("ExcludableProp"))
        {
            collidedList.Enqueue(new KeyValuePair<Collider2D, Vector2>(collision, collision.bounds.extents));
            //Debug.Log($"During Coll {collidedList.Count}");
            collision.enabled = false;
            if (crCheck == null)
            {
                crCheck = StartCoroutine(ColliderUpdate());
            }
        }
    }

    IEnumerator ColliderUpdate()
    {
        while (collidedList.Count > 0)
        {
            KeyValuePair<Collider2D, Vector2> item = collidedList.Dequeue();

            //Debug.Log($"{thisBounds.bounds}, Item = {item.Key.bounds}, {item.Value}");
            if (!IntersectCheck(thisBounds.bounds, item.Key.bounds, item.Value))
            {
                item.Key.enabled = true;
                //Debug.Log($"Reenable");
            }
            else
            {
                collidedList.Enqueue(item);
                //Debug.Log("skip");
            }

            yield return new WaitForSeconds(0.5f);
        }
        crCheck = null;

    }


    //Made own intersect because unity default intersect checks for Z val, will return false even though its 0 <= 0 and think nothing is intersecting at all
    //And also to check for cached bounds.extents val instead of the disabled one.
    public bool IntersectCheck(Bounds thisB, Bounds collidedB, Vector2 collidedExtentCache)
    {

        return IMin(thisB, true) <= IMax(collidedB, true, collidedExtentCache) && IMax(thisB, true) >= IMin(collidedB, true, collidedExtentCache)
            && IMin(thisB, false) <= IMax(collidedB, false, collidedExtentCache) && IMax(thisB, false) >= IMin(collidedB, false, collidedExtentCache);
    }
    public float IMin(Bounds boundStruct, bool isX, Vector2? cache = null)
    {
        if (cache != null)
        {
            return isX ? boundStruct.center.x - (cache.ConvertTo<Vector2>().x + extentScaleIncreaseX) : boundStruct.center.y - (cache.ConvertTo<Vector2>().y + extentScaleIncreaseY);
        }
        else
        {
            return isX ? boundStruct.center.x - boundStruct.extents.x : boundStruct.center.y - boundStruct.extents.y;
        }

    }
    public float IMax(Bounds boundStruct, bool isX, Vector2? cache = null)
    {
        if (cache != null)
        {
            return isX ? boundStruct.center.x + (cache.ConvertTo<Vector2>().x + extentScaleIncreaseX) : boundStruct.center.y + (cache.ConvertTo<Vector2>().y + extentScaleIncreaseY);
        }
        else
        {
            return isX ? boundStruct.center.x + boundStruct.extents.x : boundStruct.center.y + boundStruct.extents.y;
        }

    }
}
