///////////////////////////////////////////////////////////////////////////
// Author:  Zac King            ///////////////////////////////////////////
// Contact: ZacKingx@Gmail.com  ///////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////
// Usage:                                                                //
// //////                                                                //
// Notes:                                                                //
///////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialLoad : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image progressCircle;  // Load image
    [SerializeField] private float timeToPlay = 3f; // Seconds to complete load
    [Range(0, 3)] [SerializeField] private float timeToLoad;// Time till load
    [SerializeField] private Gradient loadGradient; // Gradient over time for load
    public bool isLoading = false;                  // Bool for current state of loading 
    private _8222016 vm = null;                     // VideoManager
    #endregion

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    [ContextMenu("Set the Type to Fill")]
    void Awake()    // If not already set up, it will be fixed on awake
    {
        vm = FindObjectOfType<_8222016>();// VideoManager is a Singleton, so it'll be the only one in the scene

        progressCircle.type = Image.Type.Filled;    // Amount visible dictacted by value
        progressCircle.fillAmount = 0;              // Zero it out
        progressCircle.fillOrigin = 2;              // Fill Origin = top
        progressCircle.fillClockwise = false;       // FIll Counter clockwise
    }

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    public void StopLoad()  // Public function allowing asessing obj halt the Load Coroutine 
    {
        isLoading = false;
    }

    public void LoadTarget(int vidIndex)  //
    {
        isLoading = true;
        StartCoroutine(LoadCircle(vidIndex));
    }

    IEnumerator LoadCircle(int index)    // Load coroutine
    {
        bool startLoad = false;
        float timer = 0;
        while (timer <= timeToPlay && isLoading)
        {
            progressCircle.fillAmount = timer / timeToPlay;
            progressCircle.color = loadGradient.Evaluate(timer / timeToPlay);
            timer += Time.deltaTime;

            if (timer >= timeToLoad && !startLoad)
            {
                startLoad = true;
                // call load at index
            }

            yield return null;
        }

        if(isLoading)
        {
            if (index != -1)
                vm.StopCurrentAndPlayAt(index);

            else    // index -1 denotes selection intended to be used to open webpage
                Application.OpenURL("http://www.tantrumlab.com/");
        }

        else
        {
            // Unload at index
        }

        progressCircle.fillAmount = 0;   // Reset
    }
}
