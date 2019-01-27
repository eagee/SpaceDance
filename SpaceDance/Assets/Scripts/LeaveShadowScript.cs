using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveShadowScript : MonoBehaviour
{
    public Dancer parent;
    private Vector3 m_startingPosition;
    private DancerStatus m_lastStatus;
    // Start is called before the first frame update
    void Start()
    {
        m_startingPosition = parent.transform.position;
        m_lastStatus = parent.GetDancerStatus();
    }

    public void TrackShadow()
    {
        m_startingPosition = parent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_lastStatus != DancerStatus.Dead && parent.GetDancerStatus() == DancerStatus.Dead)
        {
            m_startingPosition = parent.transform.position;
        }
        m_lastStatus = parent.GetDancerStatus();

        if (parent.GetDancerStatus() == DancerStatus.Dead)
        {
            Vector3 newPosition = m_startingPosition;
            newPosition.x = parent.transform.position.x;
            this.transform.position = newPosition;

            //Slowly fade alpha
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a -= 0.001f;
            if (tmp.a < 0f) tmp.a = 0f;
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
