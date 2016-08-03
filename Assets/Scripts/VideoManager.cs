using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VideoManager : Singleton<VideoManager>
{
    public void next()
    {
        Camera.main.transform.position = Videos[Videos[currentVideoIndex].NextVideoIndex].Screen.transform.position;
    }

    new void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        for (int i = 0; i < Videos.Count; i++)
        {
            Videos[i].mpc = gameObject.AddComponent<MediaPlayerCtrl>();
            GameObject screen = Videos[i].Screen ?
                Instantiate(Videos[i].Screen) as GameObject :
                Instantiate(Resources.Load("SphereScreen")) as GameObject;
            Videos[i].Screen = screen;
            //screen.SetActive(false);
            screen.name = Videos[i].Name;
            screen.transform.position += new Vector3(i * 10, 0, 0);

            Videos[i].mpc.m_bSupportRockchip = true;
            Videos[i].mpc.m_bInit = true;
            Videos[i].mpc.m_bAutoPlay = false;
            Videos[i].mpc.m_bLoop = Videos[i].Loop;

            Videos[i].mpc.m_TargetMaterial = new GameObject[1];
            Videos[i].mpc.m_TargetMaterial[0] = Videos[i].Screen;

            Videos[i].mpc.m_strFileName = Videos[i].VideoURL;
        }

        Videos[currentVideoIndex].mpc.m_TargetMaterial[0].SetActive(true);
        StartCoroutine(Preload());
    }

    public void PlayNextVideo()
    {
        StartCoroutine(PlayVideoAtIndex(Videos[currentVideoIndex].NextVideoIndex));
    }

    public void PlayVideoAt(int index)
    {
        StartCoroutine(PlayVideoAtIndex(index));
    }

    public void TogglePlayPause()
    {
        MediaPlayerCtrl.MEDIAPLAYER_STATE cState = Videos[currentVideoIndex].mpc.GetCurrentState();

        if (cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            Videos[currentVideoIndex].mpc.Pause();
        }

        else if (cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.READY ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
        {
            Videos[currentVideoIndex].mpc.Play();
        }
    }

    IEnumerator Preload(int index = 0)
    {
        Videos[index].mpc.Load(Videos[index].VideoURL);

        while (Videos[index].mpc.GetCurrentSeekPercent() < 99)
        {
            t.text = index.ToString() + " @ " + Videos[index].mpc.GetCurrentSeekPercent().ToString();
            yield return null;
        }

        Videos[index].mpc.Play();

        while (Videos[index].mpc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            yield return null;
        }

        Videos[index].mpc.Pause();

        if (index + 1 < Videos.Count)
            StartCoroutine(Preload(index + 1));

        t.text += " Done";
    }

    IEnumerator PlayVideoAtIndex(int index)
    {
        while (currentVideoIndex != index)
        {
            Videos[currentVideoIndex].mpc.Pause();
            while (Videos[currentVideoIndex].mpc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
                yield return null;

            Videos[currentVideoIndex].mpc.m_TargetMaterial[0].SetActive(false);

            Videos[index].mpc.Play();
            while (Videos[index].mpc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                yield return null;

            Videos[index].mpc.m_TargetMaterial[0].SetActive(true);

            currentVideoIndex = index;
        }
    }

    public Text t;

    public List<Video> Videos = new List<Video>();
    private MediaPlayerCtrl cVideo;
    private int currentVideoIndex = 0;
}

[System.Serializable]
public class Video
{
    public string Name;
    public string VideoURL;
    public GameObject Screen;
    public bool Loop;
    public bool PlayNextOnEnd;
    public int NextVideoIndex;
    [HideInInspector]
    public MediaPlayerCtrl mpc;
}