using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
public class RadialLoad : MonoBehaviour
{
    #region Variables
    private Image progressCircle;                   // 
    [SerializeField] private float loadTime = 3f;   // Seconds to complete load
    [SerializeField] private Gradient loadGradient; // Gradient over time for load
    public bool isLoading = false;                  // Bool for current state of loading 
    #endregion

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    [ContextMenu("Set the Type to Fill")]
    void Awake()    // If not already set up, it will be fixed on awake
    {
        progressCircle        = gameObject.GetComponent<Image>();
        progressCircle.type   = Image.Type.Filled;
    }
    
    // Context Menu Functions //////////////////////////////////////////////////////////////////////////
    [ContextMenu("Test Load")]
    private void testLoad()                 // Simulate an outside function influence on script
    {
        Callback c = assdf;
        isLoading = true;
        LoadTarget(3, c);
    }

    void assdf()
    {
        Debug.Log("Loading complete");
    }
    
    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    public void LoadTarget(float loadTime, Callback a)
    {
        loadTime = loadTime;
        StartCoroutine(LoadCircle(a));
    }

    IEnumerator LoadCircle(Callback callback)    // Load coroutine
    {
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
            callback();
        }

        progressCircle.fillAmount = 0;   // Reset
    }
}
