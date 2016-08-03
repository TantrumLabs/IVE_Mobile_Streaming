 using UnityEngine;
using System.Collections;

public class RaycastInput : MonoBehaviour
{
	void Start ()
    {
	    
	}

	void Update ()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100);
        if(hit.collider != null)
        {

        }
	}

    void OnRayHit()
    {

    }

    void OnRayExit()
    {

    }

    IEnumerator ObjectHightlighted()
    {
        yield return true;
    }

    RaycastHit hit;
}
