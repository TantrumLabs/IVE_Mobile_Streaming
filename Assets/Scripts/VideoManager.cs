using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VideoManager : Singleton<VideoManager>
{
    new void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        for (int i = 0; i < Videos.Count; i++)
        {
            MediaPlayerCtrl pl = gameObject.AddComponent<MediaPlayerCtrl>();
            GameObject screen = Videos[i].Screen ?
                Instantiate(Videos[i].Screen) as GameObject :
                Instantiate(Resources.Load("SphereScreen")) as GameObject;
            screen.SetActive(false);

            pl.m_bInit = true;
            pl.m_bAutoPlay = false;
            pl.m_bLoop = Videos[i].Loop;

            pl.m_TargetMaterial = new GameObject[1];
            pl.m_TargetMaterial[0] = screen;

            pl.m_strFileName = Videos[i].VideoURL;

            preLoaded.Add(pl);
        }

        preLoaded[currentVideoIndex].m_TargetMaterial[0].SetActive(true);
        StartCoroutine(Preload());
    }

    void Update()
    {
        cVideo = preLoaded[currentVideoIndex];
        if (cVideo.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
        {
            cVideo.Pause();
            if (!cVideo.m_bLoop && Videos[currentVideoIndex].PlayNextOnEnd)
            {
                StartCoroutine(PlayVideoAtIndex(currentVideoIndex + 1));
            }
        }
    }

    public void PlayNextVideo()
    {
        StartCoroutine(PlayVideoAtIndex(currentVideoIndex + 1));
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
            yield return null;
        }

        preLoaded[index].Play();

        while (preLoaded[index].GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            yield return null;
        }

        preLoaded[index].Pause();

        if (index + 1 < preLoaded.Count)
            StartCoroutine(Preload(index + 1));
    }

    IEnumerator PlayVideoAtIndex(int index)
    {
        while (currentVideoIndex != index)
        {
            preLoaded[currentVideoIndex].Pause();
            while (preLoaded[currentVideoIndex].GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
                yield return null;

            preLoaded[currentVideoIndex].m_TargetMaterial[0].SetActive(false);

            preLoaded[index].Play();
            while (preLoaded[index].GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                yield return null;

            preLoaded[index].m_TargetMaterial[0].SetActive(true);

            currentVideoIndex = index;
        }
    }
    public List<Video> Videos = new List<Video>();

    private MediaPlayerCtrl cVideo;
    private int currentVideoIndex = 0;
    private List<MediaPlayerCtrl> preLoaded = new List<MediaPlayerCtrl>();
}

[System.Serializable]
public class Video
{
    public string Name;
    public string VideoURL;
    public GameObject Screen;
    public bool AutoPlay;
    public bool Loop;
    public bool PlayNextOnEnd;
}