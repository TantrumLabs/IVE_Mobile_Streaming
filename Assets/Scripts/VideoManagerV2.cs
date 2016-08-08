using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VideoManagerV2 : MonoBehaviour
{
    void Start()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            MediaPlayerCtrl m = transform.GetChild(i).GetComponent<MediaPlayerCtrl>();
            if (m == null)
            {
                continue;
            }
            else
            {
                m.m_TargetMaterial[0] = null;
                MPCs.Add(m);
            }
        }
        StartCoroutine(Preload());
    }

    void Update()
    {
        if(MPCs[CurrentIndex].GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END && (CurrentIndex + 1) < MPCs.Count)
        {
            MPCs[CurrentIndex + 1].Play();
            MPCs[CurrentIndex + 1].m_TargetMaterial[0] = gameObject;
            MPCs[CurrentIndex].m_TargetMaterial[0] = null;

            CurrentIndex++;
        }
    }

    IEnumerator Preload()
    {
        t.text = "Begin";
        foreach(MediaPlayerCtrl m in MPCs)
        {
            t.text = m.gameObject.name.ToString() + Time.time.ToString();
            m.Load(m.m_strFileName);
            yield return new WaitUntil(() => m.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.READY);
            m.Play();
            yield return new WaitUntil(() => m.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);
            m.Pause();

            t.text = m.gameObject.name.ToString() + " Done";
        }
        MPCs[CurrentIndex].m_TargetMaterial[0] = gameObject;
        MPCs[CurrentIndex].Play();
    }

    public Text t;
    public int CurrentIndex = 0;
    public List<MediaPlayerCtrl> MPCs = new List<MediaPlayerCtrl>();
}
