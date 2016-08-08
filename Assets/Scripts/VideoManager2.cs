using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
class zVideo
{
    public string name;        // Identifier    // Mainly for the benifit of the Developer
    public string videoUrl;    // Dowload link to video 
    public bool waitAtEnd = false;  // Wait on completion to go to next  // Defualt to false
    public bool loop = false;       // Loop video on end
    public List<zVideo> potentialVideos;    //
    public MediaPlayerCtrl videoPlayerCtrl; // 
    public bool ready = false;
    public bool done = false;
    public GameObject gObject;
}

public class VideoManager2 : MonoBehaviour
{
    // Variables ///////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject VideoTextureScreen;     // Target Object to use this movie Texture
    public UnityEngine.UI.Text DebugScreenText;                 // Debug info to screen     // Remove before ship
    [SerializeField] private List<zVideo> allVideos = new List<zVideo>();   // List of all video that will be handled by this VideoManager
    zVideo currentVideo;                                        // Current video in use
    int index;
    int loadIndex = 0;  // Debug

    // MonoBehavior ////////////////////////////////////////////////////////////////////////////////////
    //[ContextMenu("Test")]
    void Start ()   // Use this for initialization
    {
        foreach(zVideo v in allVideos)
        {
            index = 0;
            GameObject g = new GameObject();
            g.transform.parent = transform;
            g.AddComponent<MediaPlayerCtrl>();

            v.videoPlayerCtrl = g.GetComponent<MediaPlayerCtrl>();      // 
            v.videoPlayerCtrl.m_bAutoPlay = false;
            v.videoPlayerCtrl.m_strFileName = v.videoUrl;   // Setting each video's target path to the value passed into the list 
            v.videoPlayerCtrl.m_TargetMaterial = new GameObject[1];
            v.videoPlayerCtrl.m_TargetMaterial[0] = VideoTextureScreen; // Setting video's screens. In this case only the one
            v.videoPlayerCtrl.m_bLoop = v.loop;             // Does Video loop
            v.gObject = g;
            v.videoPlayerCtrl.Stop();

            if(!v.waitAtEnd)
            {
                v.videoPlayerCtrl.OnEnd = PlayNext;
            }
            //Debug.Log(v.videoUrl + "\n- " +v.videoPlayerCtrl.m_strFileName);
        }

        //if (allVideos.Count > 0) // As long as there is something in the list of videos
        //{
        //    currentVideo = allVideos[0]; // Set intial video
        //    StartCoroutine(LoadVideo());
        //}
        index = 0;
        currentVideo = allVideos[index];
        StartCoroutine(LoadVideo());
        
        
    }
	
	void Update ()  // Update is called once per frame
    {
        DebugScreenText.text = "Current:\n" + index + "\n" +
                                currentVideo.videoPlayerCtrl.GetCurrentState().ToString() + "\n\n" +
                                "Loading: \n" +
                                loadIndex + "\n" +
                                allVideos[loadIndex].videoPlayerCtrl.GetCurrentState().ToString() + "\n" +
                                allVideos[loadIndex].videoPlayerCtrl.GetCurrentSeekPercent().ToString();
        //index = 1;

        //if (currentVideo.videoPlayerCtrl.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END && index < allVideos.Count)
        //{
        //    index += 1;
        //    currentVideo = allVideos[index];
        //    StartCoroutine(PlayVideo());
        //}
        
	}

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    IEnumerator LoadVideo()
    {
        foreach(zVideo v in allVideos)
        {
            MediaPlayerCtrl vc = v.videoPlayerCtrl;
            
            vc.Load(v.videoUrl);         // 
            while (vc.GetCurrentSeekPercent() < 99) // 
                yield return null;                  // 

            //vc.Play();
            //while (vc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            //    yield return null;

            loadIndex += 1;
            v.ready = true;

            vc.Stop();
            PlayVideo(index);   //
        }
    }

    public void PlayVideo(int i)
    {
        currentVideo.videoPlayerCtrl.StopAllCoroutines();
        currentVideo.gObject.SetActive(false);
        currentVideo = allVideos[i];

        currentVideo.gObject.SetActive(true);
        currentVideo.videoPlayerCtrl.Play();
    }

    void PlayNext()
    {
        index += 1;
        currentVideo = allVideos[index];
        PlayVideo(index);
    }

    //public void PlayNext()
    //{
    //    allVideos[2].videoPlayerCtrl.Play();

    //    //index += 1;
    //    //currentVideo = allVideos[index];
    //    //allVideos[index].videoPlayerCtrl.Play();
    //}

    //public void IncreaseIndex()
    //{
    //    index += 1;
    //    if(index >= allVideos.Count)
    //    {
    //        index = 0;
    //    }
    //    //indexButton.text = "Index: " + index;
    //}

    //IEnumerator LoadRest(zVideo loadTarget)
    //{
    //    MediaPlayerCtrl vc = loadTarget.videoPlayerCtrl;    // mediaPlayerCtrl we will be messing with

    //    vc.Load(loadTarget.videoUrl);

    //    while (vc.GetCurrentSeekPercent() < 99)
    //    {
    //        yield return null;
    //    }

    //    vc.Play();
    //    while (vc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
    //        yield return null;

    //    vc.Pause();
    //    while (vc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
    //        yield return false;  // Wait until video has been loaded

    //    index = allVideos.FindIndex(a => a == currentVideo);
    //    currentVideo = allVideos[index + 1];

    //    StartCoroutine(LoadRest(currentVideo));
    //}

    //IEnumerator LoadBranches()
    //{
    //    foreach (zVideo branch in currentVideo.potentialVideos)
    //    {

    //    }
    //    yield return null;
    //}
    
}