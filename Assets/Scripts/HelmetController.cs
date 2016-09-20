using UnityEngine;
using System.Collections;

public class HelmetController : MonoBehaviour
{
    public GameObject IntroScreen;
    public _8222016 VideoPlayer;
	
	void Update ()
    {
	    if(Input.GetKeyUp(KeyCode.F7))
        {
            if(IntroScreen.activeSelf)
            {
                IntroScreen.SetActive(false);
                VideoPlayer.enabled = true;
            }

            VideoPlayer.PlayPause();
        }
        if(VideoPlayer.GetCurrentVideoMPCState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
        {
            IntroScreen.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
