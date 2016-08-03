using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RadialLoad))]
public class RaySelect : MonoBehaviour
{
    private RaycastHit rayCursor;   //
    private RadialLoad loader;      //

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    void Update ()
    {
        Physics.Raycast(transform.position, transform.forward, out rayCursor, 100);

    }

    void OnSelecetionHit()
    {
        // call loader function
    }

    void OnSelectionExit()
    {
        loader.isLoading = false;
    }
}
