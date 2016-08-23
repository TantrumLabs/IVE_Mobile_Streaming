using UnityEngine;
using System.Collections;

public class VRController : MonoBehaviour
{
    void Start()
    {
        gvrMain = FindObjectOfType<GvrViewer>();
        gvrHead = FindObjectOfType<GvrHead>();
    }

    public void ToggleVRMode(bool set)
    {
        gvrMain.VRModeEnabled = set;
        gvrHead.enabled = set;
    }

    public void TrackHead(bool set)
    {
        gvrHead.enabled = set;
    }


    public void ToggleHeadTracking()
    {
        if (gvrMain.VRModeEnabled)
            return;

        else
            gvrHead.enabled = !gvrHead.enabled;
    }

    private GvrViewer gvrMain;
    private GvrHead gvrHead;
}