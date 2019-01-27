using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPosition : MonoBehaviour, IBeatObserver {

    public int ActivateAfterChanges = 2;
    public Transform TargetPosition;
    private int m_ChangeCount = 0;
    private Vector3 m_StartPosition;

    // Use this for initialization
    void Start () {
        m_ChangeCount = 0;
        m_StartPosition = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_ChangeCount >= ActivateAfterChanges)
        {
            Vector3 pos = this.transform.position;
            pos = Vector3.Slerp(pos, TargetPosition.position, Time.deltaTime / 3f);
            //pos = Vector3.MoveTowards(pos, TargetPosition.position, Time.deltaTime * 3f);
            this.transform.position = pos;
        }
        else
        {
            Vector3 pos = this.transform.position;
            pos = Vector3.Slerp(pos, m_StartPosition, Time.deltaTime / 3f);
            //pos = Vector3.MoveTowards(pos, TargetPosition.position, Time.deltaTime * 3f);
            this.transform.position = pos;
        }
	}

    public string Tag()
    {
        return gameObject.tag;
    }

    public void OnSongLoaded()
    {
        m_ChangeCount++;
        if (m_ChangeCount == ActivateAfterChanges)
        {
            var particleSystems = this.GetComponentsInChildren<ParticleSystem>();
            foreach(var ps in particleSystems)
            {
                ps.Play();
            }
        }
        else
        {
            m_ChangeCount = 1;
            var particleSystems = this.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                ps.Stop();
            }
        }
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

    public int OnsetIndex
    {
        get
        {
            return 0;
        }
        set
        {
            
        }
    }

    public void OnDanceStarted()
    {

    }


}
