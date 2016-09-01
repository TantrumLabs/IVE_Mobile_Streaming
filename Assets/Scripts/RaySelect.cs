///////////////////////////////////////////////////////////////////////////
// Author:  Zac King            ///////////////////////////////////////////
// Contact: ZacKingx@Gmail.com  ///////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////
// Usage:                                                                //
//          Add component to the object to act as raycast                //
//          (Ray cast is from the objects forward)                       //
// //////                                                                //
// Notes:                                                                //
//          Do not have Clickable objects colliders overlapping          //
//          When Component is added it will require <RadialLoad>         //
///////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(RadialLoad))]
public class RaySelect : MonoBehaviour
{
    #region Variables
    private RaycastHit rayCursor;   // Ray cast from camera
    private RadialLoad loader;      // Reference to the RadialLoad (On the same Game Object)
    int targetIndex;                // Storage container
    GameObject selected;
    #endregion

    #region Functions
    void Awake()    // MonoBehaviour
    {
        loader = gameObject.GetComponent<RadialLoad>();     // Store Loader
    }

    void FixedUpdate()   // Called every Physics step
    {
        if(Physics.Raycast(transform.position, transform.forward, out rayCursor, 100))  // If raycast hits something...
        {
            if (!loader.isLoading)  // makes sure not to trigger a load if one is not  already in progress
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
        if (selected == null)   // Set selected on raycast hit
            selected = (rayCursor.collider.gameObject);

        targetIndex = rayCursor.collider.gameObject.GetComponent<Clickable>().VideoIndex;   // get index from the Clickable obj (selection volumes)
        loader.LoadTarget(targetIndex);  // Start loading image
        rayCursor.collider.gameObject.GetComponent<Silhouette>().RayOn();   // Notify that raycast is on obj
    }

    void OnSelectionExit()  // Stop Load
    {
        if(selected != null)    // Wipe selected when Raycast exit
        {
            selected.GetComponent<Silhouette>().RayOff();   // Notify on raycast exit
            selected = null;    // Clear selected
        }
        loader.StopLoad();  // Stop loading image
    }
    #endregion
}
