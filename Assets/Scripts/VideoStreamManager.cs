using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StreamVideo
{
    public string name;         // Identifier    // Mainly for the benifit of the Developer
    public string videoUrl;     // Dowload link to video 
    public bool loop = false;   // Loop video on end
    public List<StreamVideo> potentialVideos;    //
    public MediaPlayerCtrl vidPlayer; // 
    public bool ready = false;
    
    //public void StreamVideo()
    //{
    //    name = "asd";
    //}
}

public class VideoStreamManager : MonoBehaviour
{
    // Variables ///////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject VideoTextureScreen;     // Target Object to use this movie Texture
    public UnityEngine.UI.Text DebugScreenText;                 // Debug info to screen     // Remove before ship
    [SerializeField] private List<StreamVideo> allVideos = new List<StreamVideo>();   // List of all video that will be handled by this VideoManager
    StreamVideo currentVideo;                                        // Current video in use
    int index;
    int loadIndex = 0;  // Debug

    // MonoBehavior ////////////////////////////////////////////////////////////////////////////////////
    //[ContextMenu("Test")]
    void Start ()   // Use this for initialization
    {
        foreach(StreamVideo v in allVideos)
        {
            index = 0;
            GameObject g = new GameObject();
            g.transform.parent = transform;
            g.AddComponent<MediaPlayerCtrl>();

            v.vidPlayer = g.GetComponent<MediaPlayerCtrl>();      // 
            v.vidPlayer.m_bAutoPlay = false;
            v.vidPlayer.m_strFileName = v.videoUrl;   // Setting each video's target path to the value passed into the list 
            v.vidPlayer.m_TargetMaterial = new GameObject[1];
            v.vidPlayer.m_TargetMaterial[0] = VideoTextureScreen; // Setting video's screens. In this case only the one
            v.vidPlayer.m_bLoop = v.loop;             // Does Video loop
            v.vidPlayer.Stop();
            
            v.vidPlayer.OnEnd = PlayNext;
           
            //Debug.Log(v.videoUrl + "\n- " +v.vidPlayer.m_strFileName);
        }
        
        index = 0;
        currentVideo = allVideos[index];
        StartCoroutine(LoadVideo());
        
        
    }
	
	void Update ()  // Update is called once per frame
    {
        DebugScreenText.text = "Current:\n" + index + "\n" +
                                currentVideo.vidPlayer.GetCurrentState().ToString() + "\n\n" +
                                "Loading: \n" +
                                loadIndex + "\n" +
                                allVideos[loadIndex].vidPlayer.GetCurrentState().ToString() + "\n" +
                                allVideos[loadIndex].vidPlayer.GetCurrentSeekPercent().ToString();   
	}

    // Functions ///////////////////////////////////////////////////////////////////////////////////////
    IEnumerator LoadVideo()
    {
        foreach(StreamVideo v in allVideos) // Load Each Stream vido
        {
            MediaPlayerCtrl vc = v.vidPlayer;
            
            vc.Load(v.videoUrl);         // 
            while (vc.GetCurrentSeekPercent() < 99) // 
                yield return null;                  // 

            loadIndex += 1;
            v.ready = true;

            vc.Stop();
            PlayVideo(index);   //
        }
    }

    public void PlayVideo(int i)
    {
        currentVideo.vidPlayer.Stop();
        currentVideo.vidPlayer.SeekTo(0);
        currentVideo.vidPlayer.StopAllCoroutines();
        //currentVideo.gObject.SetActive(false);
        currentVideo = allVideos[i];

        //currentVideo.gObject.SetActive(true);
        currentVideo.vidPlayer.Play();
    }

    void PlayNext()
    {
        index += 1;
        currentVideo = allVideos[index];
        PlayVideo(index);
    }

}