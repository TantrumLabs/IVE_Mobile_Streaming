using UnityEngine;
using System.Collections;

public class HelmetController : MonoBehaviour
{
    public GameObject IntroScreen;
    public _8222016 VideoPlayer;
	
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.F7))
        {
            if(IntroScreen.activeSelf)
            {
                IntroScreen.SetActive(false);
            }

            VideoPlayer.PlayPause();
        }
	}
}
