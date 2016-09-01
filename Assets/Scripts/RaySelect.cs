using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(RadialLoad))]
public class RaySelect : MonoBehaviour
{
    private RaycastHit rayCursor;   // Ray cast from camera
    private RadialLoad loader;      // Reference to the RadialLoad (On the same Game Object)
    int targetIndex;                // Storage container
    GameObject selected = new GameObject();
    

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        loader = gameObject.GetComponent<RadialLoad>();     //
    }

    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out rayCursor, 100))
        {
            if (!loader.isLoading)   // makes sure not to trigger a load if one is already im progress
                OnSelectionHit();   // RayCast on a valid Selection Volume
        }
        else
        {
            OnSelectionExit();  // RayCast is no longer on a Selection Volume
        }
        

    }

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    void OnSelectionHit()   // Start Load
    {
        if (selected == null)
            selected = (rayCursor.collider.gameObject);

        targetIndex = rayCursor.collider.gameObject.GetComponent<Clickable>().VideoIndex;
        loader.LoadTarget(targetIndex);  // Start
        rayCursor.collider.gameObject.GetComponent<Silhouette>().RayOn();
    }

    void OnSelectionExit()  // Stop Load
    {
        if(selected != null)
        {
            selected.GetComponent<Silhouette>().RayOff();
            selected = null;
        }
        loader.StopLoad();
    }
}
