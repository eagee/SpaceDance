using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideBeatObserver : MonoBehaviour, IBeatObserver
{


    public string Tag()
    {
        return "Nope";
    }

    public void OnSongLoaded()
    {
        
    }

    public void OnSongEnded()
    {
        
    }

    public void OnBeat(Beat beat)
    {
        
    }

    public void OnOnset(OnsetType type, Onset onset)
    {
        
    }

    public void OnChange(int index, float change)
    {
        
    }

    public void OnMissedBeat()
    {
        
    }

    public void OnDanceFinished()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        GetComponent<SpriteRenderer>().color = tmp;
    }

    public void OnDanceStarted()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 1f;
        GetComponent<SpriteRenderer>().color = tmp;
    }

    public Vector3 Position
    {
        get
        {
            return this.gameObject.transform.position;
        }
        set
        {
            this.gameObject.transform.position = value;
        }
    }

    public GameObject ObserverGameObject
    {
        get
        {
            return this.gameObject;
        }
    }

    /// <summary>
    /// Returns an index specifying the onset index associated with the BeatObserver
    /// Note: This will probably only be used on Beat objects with the tag, "Beat" that
    /// are used for the actual game mechanic.
    /// </summary>
    public int OnsetIndex {
        get { return 0;
        }
        set {
        }
    }

}
