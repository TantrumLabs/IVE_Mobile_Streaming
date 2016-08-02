using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VideoManager : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0; i < VideoUrls.Count; i++)
        {
            MediaPlayerCtrl pl = gameObject.AddComponent<MediaPlayerCtrl>();
            GameObject screen = Instantiate(Resources.Load("SphereScreen")) as GameObject;
            screen.SetActive(false);

            pl.m_bInit = false;
            pl.m_bAutoPlay = false;
            pl.m_bLoop = false;

            pl.m_TargetMaterial = new GameObject[1];
            pl.m_TargetMaterial[0] = screen;

            pl.m_strFileName = VideoUrls[i];

            pl.m_TargetMaterial[0].name = pl.m_strFileName;

            preLoaded.Add(pl);
        }

        preLoaded[currentVideoIndex].m_TargetMaterial[0].SetActive(true);

        StartCoroutine(Preload());
    }

    public void PlayVideoAtIndex(int index)
    {
        if (currentVideoIndex == index)
        {
            preLoaded[currentVideoIndex].Play();
            return;
        }

        preLoaded[currentVideoIndex].Stop();
        preLoaded[currentVideoIndex].m_TargetMaterial[0].SetActive(false);

        currentVideoIndex = index;

        preLoaded[currentVideoIndex].Play();
        preLoaded[currentVideoIndex].m_TargetMaterial[0].SetActive(true);
    }

    public void PlayNextVideo()
    {
        PlayVideoAtIndex(currentVideoIndex + 1);
    }

    public void TogglePlayPause()
    {
        MediaPlayerCtrl.MEDIAPLAYER_STATE cState = preLoaded[currentVideoIndex].GetCurrentState();

        if (cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            preLoaded[currentVideoIndex].Pause();
        }

        else if (cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.READY ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
        {
            preLoaded[currentVideoIndex].Play();
        }
    }

    IEnumerator Preload(int index = 0)
    {
        preLoaded[index].Load(preLoaded[index].m_strFileName);

        while (preLoaded[index].GetCurrentSeekPercent() < 99)
        {
            t.text = index.ToString() + " @ " + preLoaded[index].GetCurrentSeekPercent().ToString(); 
            yield return null;
        }

        preLoaded[index].Play();

        while (preLoaded[index].GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            yield return null;
        }

        preLoaded[index].Pause();

        preLoaded[index].OnEnd += PlayNextVideo;

        t.text = index.ToString() + " Done";

        if (index + 1 < preLoaded.Count)
            StartCoroutine(Preload(++index));
        
    }

    public Text t;
    public List<string> VideoUrls = new List<string>();

    [SerializeField]
    private int currentVideoIndex = 0;
    private List<MediaPlayerCtrl> preLoaded = new List<MediaPlayerCtrl>();
}
