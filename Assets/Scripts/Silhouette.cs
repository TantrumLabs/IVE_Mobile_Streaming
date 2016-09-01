///////////////////////////////////////////////////////////////////////////
// Author:  Zac King            ///////////////////////////////////////////
// Contact: ZacKingx@Gmail.com  ///////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////
// Usage:                                                                //
// //////                                                                //
// Notes:                                                                //
///////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class Silhouette : MonoBehaviour
{
    #region Variables
    [SerializeField] private AnimationCurve pulseRate;
    private Vector3 originalScale;
    public bool pulse = false;
    private Renderer objRend;
    #endregion

    #region Function
    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    void Awake ()
    {
        objRend = gameObject.GetComponent<Renderer>();      // Store object's renderer 
        originalScale = gameObject.transform.localScale;    // Store Original scale
	}

    // Function ////////////////////////////////////////////////////////////////////////////////////////
    public void RayOn()     // Start selected animations 
    {
        objRend.enabled = true;     // Outline is visible
        if (!pulse) // Ensure corutine is only started once
        {
            pulse = true;                       // Set to true
            StartCoroutine(HighLightPulse());   // Start coroutine for animation
        }
    }

    public void RayOff()    // 
    {
        objRend.enabled = false;    // Outline is not visible
        gameObject.transform.localScale = originalScale;
        pulse = false;
    }
    
    IEnumerator HighLightPulse()    // Animation of object
    {
        while (pulse)   // bool Pulse
        {
            float timer = 0;    // Interator
            while (timer <= 1 && pulse)
            {
                float s = pulseRate.Evaluate(timer / 1);
                transform.localScale = new Vector3(originalScale.x * s, originalScale.y * s, originalScale.z);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
    #endregion
}
