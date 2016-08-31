using UnityEngine;
using System.Collections;

public class Silhouette : MonoBehaviour
{
    // Variables ///////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private AnimationCurve pulseRate;
    private Vector3 originalScale;
    public bool pulse = false;
    Renderer objRend;
    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
	void Awake ()
    {
        objRend = gameObject.GetComponent<Renderer>();
        originalScale = gameObject.transform.localScale;
	}

    public void RayOn()
    {
        objRend.enabled = true;
        if (!pulse)     // If set to true, and was false -- start Coroutine
        {
            pulse = true;
            StartCoroutine(HighLightPulse());
        }
    }

    public void RayOff()
    {
        objRend.enabled = false;
        StopAllCoroutines();
        gameObject.transform.localScale = originalScale;
        pulse = false;
    }

    // Function ////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator HighLightPulse()      // Animation of object
    {
        while (pulse)
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
}
