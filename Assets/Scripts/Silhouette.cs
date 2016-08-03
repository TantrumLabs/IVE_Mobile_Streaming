using UnityEngine;
using System.Collections;

public class Silhouette : MonoBehaviour
{
    // Variables ///////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private AnimationCurve pulseRate;

    // MonoBehaviour ///////////////////////////////////////////////////////////////////////////////////
    [ContextMenu("Pulse")]
	void Start ()
    {
        StartCoroutine(HighLightPulse(1));
	}
    // Function ////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator HighLightPulse(float time)      // Animation of object
    {
        while (true)
        {
            float timer = 0;    // Interator
            while (timer <= time)
            {
                float s = pulseRate.Evaluate(timer / time);
                transform.localScale = new Vector3(s, s, s);
                timer += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
}
