using UnityEngine;
using System.Collections;

public class VRController : MonoBehaviour
{
    void Start()
    {
        gvrMain = FindObjectOfType<GvrViewer>();
        gvrHead = FindObjectOfType<GvrHead>();
    }

    public void SwitchToVRMode()
    {
        gvrMain.VRModeEnabled = true;
        gvrHead.enabled = true;
    }

    public void SwitchToFullscreen(bool trackHead = false)
    {
        gvrMain.VRModeEnabled = false;
        gvrHead.enabled = trackHead;
    }

    public void SetHeadTracking(bool trackHead)
    {
        if (gvrMain.VRModeEnabled)
            return;

        else
            gvrHead.enabled = trackHead;
    }

    private GvrViewer gvrMain;
    private GvrHead gvrHead;
}
