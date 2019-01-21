using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformModify : MonoBehaviour, IBeatObserver {

    private float m_baseSize = 8.0f;
    public float BeatMagnitude = 2.0f;
    private float m_targetSize;
    private bool m_OnBeat = false;
    private int m_SpinType = 0;
    private bool m_SpinClose = true;
	// Use this for initialization
	void Start () {
        m_OnBeat = false;
        m_targetSize = m_baseSize;
        m_SpinType = 0;
        m_SpinClose = false;
    }

    public void UpdateBehavior()
    {
        m_SpinType++;
        if (m_SpinType >= 3) m_SpinType = 0;
    }

    public void UpdateBeat()
    {
        m_OnBeat = !m_OnBeat;
        if (m_OnBeat)
        {
            m_targetSize = m_baseSize + BeatMagnitude;
        }
        else
        {
            m_targetSize = m_baseSize - BeatMagnitude;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 vec = this.transform.localScale;

        float adjustedTargetSize = m_targetSize;
        if(m_SpinType == 2)
        {
            adjustedTargetSize -= 6.0f;
        }
        vec.x = Mathf.Lerp(vec.x, adjustedTargetSize, Time.deltaTime);
        this.transform.localScale = vec;

        if(m_SpinType != 0)
        {
            if(m_SpinType == 2)
            {
                Vector3 rot = new Vector3(0f, 0f, 130f * Time.deltaTime);
                this.transform.Rotate(rot);
            }
            else
            {
                Vector3 rot = new Vector3(0f, 0f, 50f * Time.deltaTime);
                this.transform.Rotate(rot);
            }
            
        }
	}

    public string Tag()
    {
        return gameObject.tag;
    }

    public void OnSongLoaded()
    {
        
    }

    public void OnSongEnded()
    {
        
    }

    public void OnBeat(Beat beat)
    {
        UpdateBeat();
    }

    public void OnOnset(OnsetType type, Onset onset)
    {
        
    }

    public void OnChange(int index, float change)
    {
        UpdateBehavior();
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
           // do nothing
        }
    }

}
