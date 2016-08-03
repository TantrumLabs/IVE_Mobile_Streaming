using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialLoad : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image progressCircle;                   // 
    [SerializeField] private float loadTime = 3f;   // Seconds to complete load
    [SerializeField] private Gradient loadGradient; // Gradient over time for load
    public bool isLoading = false;                  // Bool for current state of loading 
    VideoManager vm;

    #endregion

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    [ContextMenu("Set the Type to Fill")]
    void Awake()    // If not already set up, it will be fixed on awake
    {
        vm = FindObjectOfType<VideoManager>();
        progressCircle.type   = Image.Type.Filled;
    }
    
    // Context Menu Functions //////////////////////////////////////////////////////////////////////////
    //[ContextMenu("Test Load")]
    //private void testLoad() // Simulate an outside function influence on script
    //{
    //    Callback c = assdf;
    //    isLoading = true;
    //    LoadTarget(3, c);
    //}

    //void assdf()
    //{
    //    Debug.Log("Loading complete");
    //}

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
    //public void LoadTarget<T>(float time, Callback<T> a, T arg1)  //
    //{
    //    loadTime = time;
    //    StartCoroutine(LoadCircle(a, arg1));
    //}


    IEnumerator LoadCircle(int index)    // Load coroutine
    {
        Debug.Log("Load started");
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
            //vm.PlayVideoAt(index);
            Debug.Log("Load done");
        }

        progressCircle.fillAmount = 0;   // Reset
    }
    //IEnumerator LoadCircle<T>(Callback<T> callback, T arg1)    // Load coroutine
    //{
    //    float timer = 0;
    //    while (timer <= loadTime && isLoading)
    //    {
    //        progressCircle.fillAmount = timer / loadTime;
    //        progressCircle.color = loadGradient.Evaluate(timer / loadTime);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    if (isLoading)
    //    {
    //        callback(arg1);
    //    }

    //    progressCircle.fillAmount = 0;   // Reset
    //}


}
