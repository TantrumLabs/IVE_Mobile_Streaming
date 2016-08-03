using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RadialLoad))]
public class RaySelect : MonoBehaviour
{
    private RaycastHit rayCursor;   //
    private RadialLoad loader;      //
    int targetIndex;                //
    //VideoManager vm;                //
    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        //vm = FindObjectOfType<VideoManager>();
        loader = gameObject.GetComponent<RadialLoad>();
    }

    void FixedUpdate ()
    {
        //target = rayCursor.collider.gameObject.GetComponent<Clickable>();
        if (Physics.Raycast(transform.position, transform.forward, out rayCursor, 100))
        {
            if(!loader.isLoading)
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
        targetIndex = rayCursor.collider.gameObject.GetComponent<Clickable>().VideoIndex;
        //Callback<int> cb = new Callback<int>(vm.PlayVideoAt);
        loader.LoadTarget(2, targetIndex);
    }

    void OnSelectionExit()  // Stop Load
    {
        loader.StopLoad();
    }
}
