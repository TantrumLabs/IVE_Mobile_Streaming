/*  Author: Eric Mouledoux
 *      Thanks to Zac King for original script
 *  Contact: Eric@Tantrumlab.com
 *  
 *  Usage:
 *  
 *  Notes:
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _ERIC_VideoStreamManager : MonoBehaviour
{
    // \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/ \/
    public UnityEngine.UI.Text t;

    void Update()
    {
        t.text = m_currentVideo.MPC.GetCurrentState().ToString() + Time.time.ToString();
        if (m_currentVideo.MPC.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END)
        {
            if (m_currentVideo.loop)
            {
                m_currentVideo.MPC.Play();
            }

            else if (m_currentVideo.potentialNextIndex.Count == 1)
            {
                StopCurrentAndPlayAtIndex(m_currentVideo.potentialNextIndex[0]);
            }
        }
    }

    // /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\ /\

    void Awake()
    {
        Startup();
    }

    void Startup()
    {
        foreach (StreamVideoInfo s in videos)
        {
            MediaPlayerCtrl m = gameObject.AddComponent<MediaPlayerCtrl>();
            m.m_strFileName = s.videoPath;
            m.m_TargetMaterial = new GameObject[1];
            m.m_bFullScreen = false;
            m.m_bSupportRockchip = true;
            m.m_ScaleValue = MediaPlayerCtrl.MEDIA_SCALE.SCALE_X_TO_Y;
            m.m_bAutoPlay = false;
            m.m_bInit = true;

            if(s.SelectionScreens)
                s.SelectionScreens.SetActive(false);

            s.MPC = m;
        }
        videos[0].MPC.Load(videos[0].MPC.m_strFileName);
    }

    public void PlayPause()
    {
        if (m_currentVideo.MPC.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            m_currentVideo.MPC.Pause();
        else
            m_currentVideo.MPC.Play();
    }

    public void StopCurrentAndPlayAtIndex(int index)
    {
        StopThisVideo(m_currentVideo);

        foreach (int i in m_currentVideo.potentialNextIndex)
        {
            if (i != index)
                videos[i].MPC.UnLoad();
        }

        PlayVideoAt(index);
    }

    public void PlayVideoAt(int index)
    {
        videos[index].MPC.Play();
        videos[index].MPC.m_TargetMaterial[0] = gameObject;
        m_currentVideo = videos[index];


        if (videos[index].SelectionScreens)
            videos[index].SelectionScreens.SetActive(true);

        if (videos[index].potentialNextIndex.Count > 0)
        {
            foreach (int next in videos[index].potentialNextIndex)
            {
                videos[next].MPC.Load(videos[next].MPC.m_strFileName);
            }
        }
    }

    public void StopThisVideo(StreamVideoInfo video)
    {
        video.MPC.m_TargetMaterial[0] = null;
        video.MPC.Stop();
        video.MPC.UnLoad();

        if (video.SelectionScreens)
            video.SelectionScreens.SetActive(false);
    }

    /// <summary>
    /// List of videos for streaming
    /// </summary>
    public List<StreamVideoInfo> videos;

    private StreamVideoInfo m_currentVideo;
}

/// <summary>
/// Container to access/set values for streaming videos
/// </summary>
[System.Serializable]
public class StreamVideoInfo
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

    public GameObject SelectionScreens;
    /// <summary>
    /// List of videos that could possibly follow this one
    /// </summary>
    public List<int> potentialNextIndex = new List<int>();
    /// <summary>
    /// The MediaPlayerCtrl to control the video
    /// </summary>
    [HideInInspector] public MediaPlayerCtrl MPC;
}
