using UnityEngine;
using UnityEngine.UI;
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
        for(int i = 0; i < Videos.Count; ++i)
        {
            Video v = Videos[i];

            GameObject go = new GameObject();
            MPCs.Insert(i, go.AddComponent<MediaPlayerCtrl>());
            MediaPlayerCtrl m = MPCs[i];

            m.m_bSupportRockchip = false;
            m.m_bInit = true;
            m.m_bAutoPlay = false;

            m.m_strFileName = v.VideoURL;
            m.m_bLoop = v.Loop;

            m.m_TargetMaterial = new GameObject[1];
            m.m_TargetMaterial[0] = null;

            if(v.Screen)
            {
                v.Screen = Instantiate(v.Screen) as GameObject;
                v.Screen.SetActive(false);
            }

            if(v.NextVideoIndex >= 0 && v.NextVideoIndex < Videos.Count)
            {
                m.OnEnd += PlayNextVideo;
            }

            //if(v.BranchIndicies.Count > 0)
            //{
            //    m.OnVideoFirstFrameReady += PreloadBranches;
            //}
        }

        StartCoroutine(PreloadAll());
        MPCs[0].m_TargetMaterial[0] = gameObject;
    }

    public void TogglePlayPause()
    {
        MediaPlayerCtrl m = MPCs[m_currentVideoIndex];
        MediaPlayerCtrl.MEDIAPLAYER_STATE cState = m.GetCurrentState();

        if (cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
        {
            m.Pause();
        }

        else if (cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.READY ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED ||
                 cState == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
        {
            m.Play();
        }
    }

    public void PlayVideoAtIndex(int index)
    {
        if (index != CurrentVideoIndex && index >= 0 && index < Videos.Count)
        {
            Video v1 = Videos[CurrentVideoIndex];
            Video v2 = Videos[index];
            MediaPlayerCtrl m1 = MPCs[CurrentVideoIndex];
            MediaPlayerCtrl m2 = MPCs[index];
            
            if(v1.Screen)
            {
                v1.Screen.SetActive(false);
            }
            if(v2.Screen)
            {
                v2.Screen.SetActive(true);
            }

            m2.Play();

            m2.m_TargetMaterial[0] = m1.m_TargetMaterial[0];
            m1.m_TargetMaterial[0] = null;

            CurrentVideoIndex = index;
        }

    }

    public void PlayNextVideo()
    {
        t.text = Videos[CurrentVideoIndex].NextVideoIndex.ToString();
        PlayVideoAtIndex(Videos[CurrentVideoIndex].NextVideoIndex);
    }

    public void PreloadBranches()
    {
        StartCoroutine(PreloadNext());
    }

    // Hazard /!\//////////////////////////////////
    IEnumerator PreloadAll()
    {
        for (int i = 0; i < MPCs.Count; ++i)
        {
            t.text = "BEGIN";
            MediaPlayerCtrl m = MPCs[i];
            m.Load(m.m_strFileName);

            //while (m.GetCurrentSeekPercent() < 99)
            //{
            //    t.text = (1 + i).ToString() + " @ " + m.GetCurrentSeekPercent().ToString() + " : " + Time.time.ToString(); 
            //    yield return null;
            //}

            //t.text += " Preloaded";

            //if (m.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            //{

            t.text = m.GetCurrentState().ToString();
            yield return new WaitForSeconds(3);

            m.Play();

            t.text = m.GetCurrentState().ToString();
            yield return new WaitForSeconds(3);

            while (m.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING || m.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.ERROR)
                {
                    if(m.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.ERROR)
                    {
                        //m.Pause();
                        m.Stop();
                        StopCoroutine(PreloadAll());

                    PlayNextVideo();
                }
                    t.text = CurrentVideoIndex.ToString() + " " +  m.GetCurrentState().ToString();
                    yield return null;
                }

                //m.Pause();
            //}

            t.text = i.ToString() + " " + m.GetCurrentState().ToString();
            yield return null;
        }
        t.text = "All Good";
        MPCs[CurrentVideoIndex].Play();
    }

    IEnumerator PreloadNext()
    {
        foreach(int i in Videos[CurrentVideoIndex].BranchIndicies)
        {
            MediaPlayerCtrl m = MPCs[i];
            m.Load(m.m_strFileName);

            while (m.GetCurrentSeekPercent() < 99)
            {
                t.text = (1 + i).ToString() + " @ " + m.GetCurrentSeekPercent().ToString() + " : " + Time.time.ToString();
                yield return null;
            }

            t.text += " Preloaded";

            if (m.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            {
                m.Play();

                while (m.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
                {
                    yield return null;
                }

                m.Pause();
            }

            t.text += " " + m.GetCurrentState().ToString();
        }
        t.text = "All Good";
    }

    public Text t;


    private int m_currentVideoIndex = 0;
    private List<MediaPlayerCtrl> MPCs = new List<MediaPlayerCtrl>();

    public int CurrentVideoIndex
    {
        get { return m_currentVideoIndex; }
        set { m_currentVideoIndex = value; }
    }

    public List<Video> Videos = new List<Video>();
}

[System.Serializable]
public class Video
{
    public string VideoURL;
    public GameObject Screen;
    public bool Loop;
    public List<int> BranchIndicies;
    public int NextVideoIndex;
}
/*///////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        for (int i = 0; i < Videos.Count; i++)
        {
            Videos[i].mpc = gameObject.AddComponent<MediaPlayerCtrl>();
            GameObject screen = Videos[i].Screen ?
                Instantiate(Videos[i].Screen) as GameObject :
                Instantiate(Resources.Load("SphereScreen")) as GameObject;
            Videos[i].Screen = screen;
            screen.SetActive(false);
            screen.name = Videos[i].Name;

            Videos[i].mpc.m_bSupportRockchip = false;
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
        if(Videos[currentVideoIndex].NextVideoIndex != -1)
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
            t.text = (1+index).ToString() + " @ " + Videos[index].mpc.GetCurrentSeekPercent().ToString();
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
            Videos[currentVideoIndex].mpc.Stop();
            while (Videos[currentVideoIndex].mpc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED)
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
*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////