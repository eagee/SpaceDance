using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSpinBar : MonoBehaviour, IBeatObserver {

    public bool allowSpin = false;
    public float m_baseSize = 8.0f;
    public float BeatMagnitude = 2.0f;
    private float m_targetSize;
    private bool m_OnBeat = false;
    private int m_SpinType = 0;
    private int m_danceNumber = 0;

    public void SetDanceNumber(int danceNumber)
    {
        m_danceNumber = danceNumber;
    }

	// Use this for initialization
	void Start () {
        m_OnBeat = false;
        m_targetSize = m_baseSize;
        m_danceNumber = 0;
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
            
        if(m_danceNumber == 0)
        {
            Vector3 rot = new Vector3(0f, 0f, 25f * Time.deltaTime);
            this.transform.Rotate(rot);
            vec.x = Mathf.Lerp(vec.x, adjustedTargetSize, Time.deltaTime);
            this.transform.localScale = vec;
        }
        else if (m_danceNumber == 1)
        {
            Vector3 rot = new Vector3(0f, 0f, 50f * Time.deltaTime);
            this.transform.Rotate(rot);
            vec.x = Mathf.Lerp(vec.x, adjustedTargetSize, Time.deltaTime);
            this.transform.localScale = vec;
        }
        else if (m_danceNumber == 2)
        {
            adjustedTargetSize -= 4.0f;
            Vector3 rot = new Vector3(0f, 0f, 130f * Time.deltaTime);
            this.transform.Rotate(rot);
            vec.x = Mathf.Lerp(vec.x, adjustedTargetSize, Time.deltaTime);
            this.transform.localScale = vec;
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
        //UpdateBehavior();
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
           // do nothing
        }
    }

    public void OnDanceStarted()
    {

    }

}
