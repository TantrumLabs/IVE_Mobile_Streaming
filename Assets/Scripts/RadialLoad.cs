using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialLoad : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image progressCircle;  // 
    [SerializeField] private float loadTime = 3f;   // Seconds to complete load
    [SerializeField] private Gradient loadGradient; // Gradient over time for load
    public bool isLoading = false;                  // Bool for current state of loading 
    public VideoManager2 vm;                                // VideoManager
    #endregion

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    [ContextMenu("Set the Type to Fill")]
    void Awake()    // If not already set up, it will be fixed on awake
    {
        vm = FindObjectOfType<VideoManager2>();  // VideoManager is a Singleton, so it'll be the only one in the scene

        progressCircle.type = Image.Type.Filled;    // Amount visible dictacted by value
        progressCircle.fillAmount = 0;              // Zero it out
        progressCircle.fillOrigin = 2;              // Fill Origin = top
        progressCircle.fillClockwise = false;       //  
    }

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    public void StopLoad()  //
    {
        isLoading = false;
    }

    public void LoadTarget(float time, int vidIndex)  //
    {
        isLoading = true;
        loadTime = time;
        StartCoroutine(LoadCircle(vidIndex));
    }

    IEnumerator LoadCircle(int index)    // Load coroutine
    {
        //Debug.Log("Load started");
        float timer = 0;
        while (timer <= loadTime && isLoading)
        {
            progressCircle.fillAmount = timer / loadTime;
            progressCircle.color = loadGradient.Evaluate(timer / loadTime);
            timer += Time.deltaTime;
            yield return null;
        }

        if(isLoading)
        {
            vm.PlayVideo(index);
            //Debug.Log("Load done");
        }

        progressCircle.fillAmount = 0;   // Reset
    }
}
