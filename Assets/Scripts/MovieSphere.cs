using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(MediaPlayerCtrl))]

public class MovieSphere : MonoBehaviour
{
    void Awake()
    {
        M = GetComponent<MediaPlayerCtrl>();
        M.m_TargetMaterial = new GameObject[1];
        M.m_TargetMaterial[0] = gameObject;
        //GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(Preload());
    }

    //public void Load()
    //{
    //    M.m_TargetMaterial = new GameObject[1];
    //    M.m_TargetMaterial[0] = gameObject;

    //    M.Load(M.m_strFileName);
    //}

	void Update ()
    {
        t.text = GetComponent<MediaPlayerCtrl>().GetCurrentState().ToString();
	    //if(M.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
     //   {
     //       GetComponent<MeshRenderer>().enabled = false;
     //   }
     //   if (M.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
     //   {
     //       GetComponent<MeshRenderer>().enabled = true;
     //   }
     //   if (M.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.ERROR)
     //   {
     //       M.Stop();
     //       StartCoroutine(Preload());
     //   }
    }

    IEnumerator Preload()
    {
        M.Load(M.m_strFileName);

        while(M.GetCurrentSeekPercent() < 99)
        {
            t.text = M.GetCurrentSeekPercent().ToString();
            yield return null;
        }
        M.Stop();
        while (M.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED)
        {
            yield return null;
        }
        //M.Play();
        //while (M.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        //{
        //    yield return null;
        //}
        //M.Pause();
        //while(M.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
        //{
        //    yield return null;
        //}

    }
    public Text t;
    private MediaPlayerCtrl M;
}
