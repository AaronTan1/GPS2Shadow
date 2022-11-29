using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExclusionLight : MonoBehaviour
{
    private Collider2D thisBounds; //Exclusion own bounds
    Queue<KeyValuePair<Collider2D, Vector2>> collidedList = new Queue<KeyValuePair<Collider2D, Vector2>>();
    Coroutine crCheck = null;

    private void Start()
    {
        thisBounds = GetComponent<Collider2D>();
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Blight"))
        {
            collidedList.Enqueue(new KeyValuePair<Collider2D, Vector2>(collision, collision.bounds.extents));
            Debug.Log($"During Coll {collidedList.Count}");
            collision.enabled = false;
            if(crCheck == null)
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

            if (!IntersectCheck(thisBounds.bounds, item.Key.bounds, item.Value))
            {
                item.Key.enabled = true;
                //Debug.Log($"During Check {collidedList.Count}");
            }
            else
            {
                collidedList.Enqueue(item);
            }

            yield return new WaitForSeconds(0.3f);
        }
        crCheck = null;
        
    }




    //Made own intersect because unity default intersect checks for Z val, will return false even though its 0 <= 0 and think nothing is intersecting at all
    //And also to check for cached bounds.extents val instead of the disabled one.
    public bool IntersectCheck(Bounds thisB, Bounds collidedB, Vector2 collidedExtentCache)
    {

        return IMin(thisB, true) <= IMax(collidedB, true) && IMax(thisB, true) >= IMin(collidedB, true)
            && IMin(thisB, false) <= IMax(collidedB, false) && IMax(thisB, false) >= IMin(collidedB, false);
    }    
    public float IMin(Bounds boundStruct, bool isX, Vector2 ? cache = null)
    {
        return isX? boundStruct.center.x - boundStruct.extents.x: boundStruct.center.y - boundStruct.extents.y;
    }
    public float IMax(Bounds boundStruct, bool isX, Vector2 ? cache = null)
    {
        return isX ? boundStruct.center.x + boundStruct.extents.x : boundStruct.center.y + boundStruct.extents.y;
    }
}
