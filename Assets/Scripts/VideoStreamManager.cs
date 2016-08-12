using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StreamVideo
{
    public string name;         // Identifier    // Mainly for the benifit of the Developer
    public string videoUrl;     // Dowload link to video 
    public bool loop = false;   // Loop video on end
    public List<int> potentialVideos;    // videos by index
    private MediaPlayerCtrl _vidPlayer;
    public MediaPlayerCtrl vidPlayer
    {
        get { return _vidPlayer; }
        set { _vidPlayer = value; }
    } 
    [HideInInspector] public bool ready = false;          //

}

public class VideoStreamManager : MonoBehaviour
{
    // Variables ///////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject VideoTextureScreen;                         // Target Object to use this movie Texture
    public UnityEngine.UI.Text DebugScreenText;     // Debug info to screen     // Remove before ship
    [SerializeField] private List<GameObject> selectionVolumes  = new List<GameObject>();
    [SerializeField] private List<StreamVideo> allVideos        = new List<StreamVideo>(); // List of all video that will be handled by this VideoManager
    StreamVideo currentVideo;   // Current video in use
    int index;                  // Current video index
    int loadIndex;  // Track load

    // MonoBehavior ////////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        index = 0;
        currentVideo = allVideos[index];
        SetUpAllStreamPlayers();
        StartCoroutine(LoadInitVid());
    }

    [ContextMenu("Set up Media Controls")]
    void SetUpAllStreamPlayers()
    {
        foreach (StreamVideo v in allVideos)
        {
            GameObject g = new GameObject();    // Instansiate an empty gameObject
            g.transform.parent = transform;     // Parent to gameObject
            g.AddComponent<MediaPlayerCtrl>();  // 

            v.vidPlayer = g.GetComponent<MediaPlayerCtrl>();
            v.vidPlayer.m_bAutoPlay = false;
            v.vidPlayer.m_strFileName = v.videoUrl;   // Setting each video's target path to the value passed into the list 
            v.vidPlayer.m_TargetMaterial = new GameObject[1];
            v.vidPlayer.m_TargetMaterial[0] = VideoTextureScreen; // Setting video's screens. In this case only the one
            v.vidPlayer.m_bLoop = v.loop;             // Does Video loop
            v.vidPlayer.Stop();
            if (v.loop)
                v.vidPlayer.OnEnd = PlaySelf;
            else
                v.vidPlayer.OnEnd = PlayNext;  // On record... I don't like how I did this part    // Review
        }
        index = 0;
    }

    void Update()
    {
        // Uncomment for onscreen debug text
        DebugScreenText.text = "Current:\n" + index + "\n" +
                                currentVideo.vidPlayer.GetCurrentState().ToString() + "\n\n" +
                                "Loading: \n" +
                                loadIndex + "\n" +
                                allVideos[loadIndex].vidPlayer.GetCurrentState().ToString() + "\n" +
                                allVideos[loadIndex].vidPlayer.GetCurrentSeekPercent().ToString() + "\n" + 
                                currentVideo.vidPlayer.err;
    }

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    IEnumerator LoadPotenialVideos(StreamVideo sv)
    {
        sv.vidPlayer.Play();
        foreach (int i in sv.potentialVideos)
        {
            loadIndex = i;

            MediaPlayerCtrl t = allVideos[loadIndex].vidPlayer;
            t.Load(allVideos[loadIndex].videoUrl);
            while (t.GetCurrentSeekPercent() < 99)
                yield return null;
        }
    }

    IEnumerator LoadInitVid()
    {
        allVideos[0].vidPlayer.Load(currentVideo.videoUrl);
        while(allVideos[0].vidPlayer.GetCurrentSeekPercent() < 99)
            yield return null;

        loadIndex = 0;

        allVideos[0].vidPlayer.Stop();
        PlayVideo(0);
    }

    void PlaySelf()
    {
        currentVideo.vidPlayer.Play();
    }

    void PlayIntialVideo()
    {
        PlayVideo(0);
    }

    public void PlayVideo(int i)
    {
        foreach (GameObject g in selectionVolumes)
            g.SetActive(allVideos[i].loop);
        
        if(allVideos[i] != null)
        {
            index = i;
            currentVideo = allVideos[i];        // Set video to passed in index
            List<MediaPlayerCtrl> unloadVids = new List<MediaPlayerCtrl>();
            foreach(StreamVideo sv in allVideos)
            {
                if (sv != currentVideo && !currentVideo.potentialVideos.Contains(allVideos.IndexOf(sv)))
                    sv.vidPlayer.UnLoad();
            }
            currentVideo.vidPlayer.SetSeekBarValue(0);   // Go to the begining of the video
            StartCoroutine(LoadPotenialVideos(currentVideo));
        }
    }

    void PlayNext()
    {
        //if(currentVideo.potentialVideos.Count > 0)
            PlayVideo(currentVideo.potentialVideos[0]);   //
    }

}