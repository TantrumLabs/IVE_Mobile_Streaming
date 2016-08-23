using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _8222016 : MonoBehaviour
{
    public UnityEngine.UI.Text t;

    void Awake()
    {
        t.text = vInfo.Count.ToString();
        StartUp();
        cVideo = vInfo[0];
    }

    void StartUp()
    {
        for(int i = 0; i < vInfo.Count; ++i)
        {
            vInfo[i].MPC = gameObject.AddComponent<MediaPlayerCtrl>();

            vInfo[i].fsm = new _FSM<VideoInfo.VideoStates>();

            vInfo[i].fsm.AddState(VideoInfo.VideoStates.INIT);
            vInfo[i].fsm.AddState(VideoInfo.VideoStates.READY);
            vInfo[i].fsm.AddState(VideoInfo.VideoStates.PLAYING);
            vInfo[i].fsm.AddState(VideoInfo.VideoStates.PAUSED);
            vInfo[i].fsm.AddState(VideoInfo.VideoStates.STOPPED);
            vInfo[i].fsm.AddState(VideoInfo.VideoStates.END);
            vInfo[i].fsm.AddState(VideoInfo.VideoStates.OUT);

            //VideoInfo.Handler toInit = vInfo[i].Init;
            VideoInfo.Handler toReady =  vInfo[i].Ready;
            VideoInfo.Handler toPlay = vInfo[i].Play;
            toPlay += SetActiveScreenTrue;
            VideoInfo.Handler toPause = vInfo[i].Pause;
            VideoInfo.Handler toStopped = vInfo[i].Stop;
            VideoInfo.Handler toEnd = vInfo[i].End;
            //toEnd += SetActiveScreenFalse;

            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.INIT,     VideoInfo.VideoStates.READY,    toReady);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.READY,    VideoInfo.VideoStates.PLAYING,  toPlay);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.PLAYING,  VideoInfo.VideoStates.PAUSED,   toPause);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.PAUSED,   VideoInfo.VideoStates.PLAYING,  toPlay);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.PLAYING,  VideoInfo.VideoStates.STOPPED,  toStopped);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.PAUSED,   VideoInfo.VideoStates.STOPPED,  toStopped);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.STOPPED,  VideoInfo.VideoStates.PLAYING,  toPlay);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.STOPPED,  VideoInfo.VideoStates.END,      toEnd);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.END,      VideoInfo.VideoStates.OUT,      null);
            vInfo[i].fsm.AddTransition(VideoInfo.VideoStates.OUT,      VideoInfo.VideoStates.READY,    toReady);


            vInfo[i].fsm.m_currentState = VideoInfo.VideoStates.INIT;
        }
    }

    void FixedUpdate()
    {
        switch(cVideo.fsm.m_currentState)
        {
            case VideoInfo.VideoStates.INIT:
                cVideo.Init();
                cVideo.fsm.MakeTransitionTo(VideoInfo.VideoStates.READY);
                break;

            case VideoInfo.VideoStates.READY:
                if(cVideo.MPC.GetCurrentSeekPercent() > 99)
                {
                    cVideo.fsm.MakeTransitionTo(VideoInfo.VideoStates.PLAYING);
                }
                break;

            case VideoInfo.VideoStates.PLAYING:
                if (cVideo.MPC.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
                {
                    if(cVideo.loop)
                    {
                        cVideo.Play();
                    }
                    else if(cVideo.autoNext >= 0 && cVideo.autoNext < vInfo.Count)
                    {
                        StopCurrentAndPlayAt(cVideo.autoNext);
                    }
                }
                break;

            case VideoInfo.VideoStates.PAUSED:
                break;

            case VideoInfo.VideoStates.STOPPED:
                break;

            case VideoInfo.VideoStates.END:
                cVideo.fsm.MakeTransitionTo(VideoInfo.VideoStates.OUT);
                break;

            case VideoInfo.VideoStates.OUT:
                cVideo.fsm.MakeTransitionTo(VideoInfo.VideoStates.READY);
                break;

            default:
                break;
        };

        // DEBUGGING ////////////////////////////////////////////////////////////

        t.text = ((int)Time.time).ToString();
        t.text += cVideo.Name;
        t.text += cVideo.fsm.m_currentState.ToString();
        t.text += cVideo.MPC.GetCurrentSeekPercent().ToString();
        t.text += cVideo.MPC.err;

        //////////////////////////////////////////////////////////////////////////
    }

    public void SetActiveScreenTrue()
    {
        cVideo.MPC.m_TargetMaterial[0] = gameObject;
    }
    public void SetActiveScreenFalse()
    {
        cVideo.MPC.m_TargetMaterial[0] = null;
    }

    public void StopCurrentAndPlayAt(int index)
    {
        cVideo.fsm.MakeTransitionTo(VideoInfo.VideoStates.STOPPED);
        cVideo.fsm.MakeTransitionTo(VideoInfo.VideoStates.END);
        cVideo = vInfo[index];
    }

    public List<VideoInfo> vInfo = new List<VideoInfo>();
    private VideoInfo cVideo;
}

[System.Serializable]
public class VideoInfo
{
    public string Name = "";
    /// <summary>
    /// Absolut path, or direct download link to video
    /// </summary>
    public string videoPath = "";
    /// <summary>
    /// True if the video should loop on end
    /// </summary>
    public bool loop = false;
    /// <summary>
    /// Video index to auto play when this one ends
    /// </summary>
    public int autoNext = -1;

    public GameObject SelectionScreens;



    public enum VideoStates
    {
        INIT,
        READY,
        PLAYING,
        PAUSED,
        STOPPED,
        END,
        OUT,
    }
    public _FSM<VideoStates> fsm;
    public delegate void Handler();

    /// <summary>
    /// The MediaPlayerCtrl to control the video
    /// </summary>
    [HideInInspector]
    public MediaPlayerCtrl MPC;

    public void Init()
    {
        MPC.m_strFileName = videoPath;
        MPC.m_TargetMaterial = new GameObject[1];
        MPC.m_bFullScreen = false;
        MPC.m_bSupportRockchip = false;
        MPC.m_ScaleValue = MediaPlayerCtrl.MEDIA_SCALE.SCALE_X_TO_Y;
        MPC.m_bAutoPlay = false;
        MPC.m_bInit = true;

        if (SelectionScreens)
            SelectionScreens.SetActive(false);
    }

    public void Ready()
    {
        MPC.Load(MPC.m_strFileName);
    }

    public void Play()
    {
        if (SelectionScreens)
            SelectionScreens.SetActive(true);
        MPC.Play();
    }

    public void Pause()
    {
        MPC.Pause();
    }

    public void Stop()
    {
        MPC.Pause();
        MPC.SeekTo(0);
        if (SelectionScreens)
            SelectionScreens.SetActive(false);
    }

    public void End()
    {
        MPC.Stop();
        MPC.m_TargetMaterial[0] = null;
        if (SelectionScreens)
            SelectionScreens.SetActive(false);
        MPC.UnLoad();
    }
}