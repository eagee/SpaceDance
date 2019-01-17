using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformModify : MonoBehaviour {

    public float BaseSize = 8.0f;
    public float BeatMagnitude = 2.0f;
    private float m_targetSize;
    private bool m_OnBeat = false;
    private bool m_Spin = false;
	// Use this for initialization
	void Start () {
        m_OnBeat = false;
        m_targetSize = BaseSize;
        m_Spin = false;
    }

    public void UpdateBehavior()
    {
        m_Spin = !m_Spin;
    }

    public void UpdateBeat()
    {
        m_OnBeat = !m_OnBeat;
        if (m_OnBeat)
        {
            m_targetSize = BaseSize + BeatMagnitude;
        }
        else
        {
            m_targetSize = BaseSize - BeatMagnitude;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 vec = this.transform.localScale;
        vec.x = Mathf.Lerp(vec.x, m_targetSize, Time.deltaTime);
        this.transform.localScale = vec;

        if(m_Spin)
        {
            Vector3 rot = new Vector3(0f, 0f, 50f * Time.deltaTime);
            this.transform.Rotate(rot);
        }
	}
}
