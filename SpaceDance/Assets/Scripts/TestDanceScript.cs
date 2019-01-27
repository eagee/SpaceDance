using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDanceScript : MonoBehaviour, IBeatObserver {
    public Transform TransformToFollow;
    public TestDanceScript DanceBuddy;
    public float DanceMagnitude = 2f;
    public bool Reverse = false;
    private int m_direction;
    private Vector3 m_startingOffset;
    private Vector3 m_targetPosition;
    private const int SOUTH = 0;
    private const int EAST = 1;
    private const int NORTH = 2;
    private const int WEST = 3;
    private const int SPIN = 4;
    private int m_danceNumber;

    // Use this for initialization
    void Start () {
        m_startingOffset = this.transform.position;
        if (Reverse)
            m_direction = 3;
        else
            m_direction = 0;
        m_danceNumber = 0;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 vec = this.transform.position;
        vec = Vector3.Lerp(vec, m_targetPosition, 2f * Time.deltaTime);
        this.transform.position = vec;

        if (m_danceNumber == 1)
            DoSpinDance();
        else if (m_danceNumber == 2)
            DoWaltz();
    }

    private void DoDiamondDance()
    {
        GetComponent<Animator>().speed = 0.5f;
        Reverse = false;
        // Move our character based on the direction specified
        if (m_direction == SOUTH)
        {
            m_targetPosition = m_startingOffset;
            m_targetPosition.y -= DanceMagnitude;
        }
        else if (m_direction == EAST)
        {
            m_targetPosition = m_startingOffset;
            m_targetPosition.x += DanceMagnitude;
        }
        else if (m_direction == NORTH)
        {
            m_targetPosition = m_startingOffset;
            m_targetPosition.y += DanceMagnitude;
        }
        else if (m_direction == WEST)
        {
            m_targetPosition = m_startingOffset;
            m_targetPosition.x -= DanceMagnitude;
        }

        // Handle our movement animation
        GetComponent<Animator>().SetInteger("Direction", m_direction);
        if (Reverse)
        {
            m_direction--;
            if (m_direction < SOUTH)
            {
                m_direction = WEST;
            }
        }
        else
        {
            m_direction++;
            if (m_direction > WEST)
            {
                m_direction = SOUTH;
            }
        }
    }

    private float GetAngleToVector(Vector3 target)
    {
        Vector3 vectorToTarget = target - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) / Mathf.PI) * 180f;
        //angle = Mathf.Clamp(angle, -360f, 360f);
        if (angle < 0) angle += 360f;
        return angle;
    }

    private void DoWaltz()
    {
        GetComponent<Animator>().speed = 0.5f;
        m_targetPosition = TransformToFollow.position;

        // Set our sorting order assuming y depth translates to z depth
        if(DanceBuddy.transform.position.y > this.transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        float angle = GetAngleToVector(DanceBuddy.transform.position);

        if ((angle >= 315f && angle <= 360f) || (angle >= 0f && angle < 45f ))
        {
            if(GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "E: " + angle.ToString();
            m_direction = EAST;
        }
        else if (angle >= 45f && angle < 135f)
        {
            if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "N: " + angle.ToString();
            m_direction = NORTH;
        }
        else if (angle >= 135f && angle < 225f)
        {
            if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "W: " + angle.ToString();
            m_direction = WEST;
        }
        else if (angle >= 225f && angle < 315f)
        {
            if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "S: " + angle.ToString();
            m_direction = SOUTH;
        }

        GetComponent<Animator>().SetInteger("Direction", m_direction);
    }

    private void DoSpinDance()
    {
        m_targetPosition = TransformToFollow.position;
        
        // Handle our movement animation
        if(GetComponent<Animator>().GetInteger("Direction") != SPIN)
            GetComponent<Animator>().SetInteger("Direction", SPIN);

        //GetComponent<Animator>().SetInteger("Direction", m_direction);
        if (Reverse)
        {
            m_direction--;
            if (m_direction < SOUTH)
            {
                m_direction = WEST;
            }
        }
        else
        {
            m_direction++;
            if (m_direction > WEST)
            {
                m_direction = SOUTH;
            }
        }
    }

    public void UpdateDance()
    {
        m_startingOffset = TransformToFollow.position;
        if (m_danceNumber == 0)
            DoDiamondDance();
    }

    public void ChangeDance() {
        m_danceNumber++;
        if (m_danceNumber > 2) m_danceNumber = 0;
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
        
    }

    public void OnOnset(OnsetType type, Onset onset)
    {
        UpdateDance();
    }

    public void OnChange(int index, float change)
    {
        ChangeDance();
    }

    public void OnMissedBeat()
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

}
