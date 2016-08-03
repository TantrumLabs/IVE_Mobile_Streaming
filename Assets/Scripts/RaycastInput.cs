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
	}

    RaycastHit hit;
}
