using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDanceScript : MonoBehaviour {
    public Transform TransformToFollow;
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
        vec = Vector3.Lerp(vec, m_targetPosition, Time.deltaTime);
        this.transform.position = vec;
    }

    private void DoDiamondDance()
    {
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

    private void DoSpinDance()
    {
        m_targetPosition = TransformToFollow.position;
        //m_direction = Random.RandomRange(SOUTH, WEST);
        // Move our character based on the direction specified
        //if (m_direction == SOUTH)
        //{
        //    m_targetPosition = m_startingOffset;
        //    m_targetPosition.y -= DanceMagnitude;
        //}
        //else if (m_direction == EAST)
        //{
        //    m_targetPosition = m_startingOffset;
        //    m_targetPosition.x += DanceMagnitude;
        //}
        //else if (m_direction == NORTH)
        //{
        //    m_targetPosition = m_startingOffset;
        //    m_targetPosition.y += DanceMagnitude;
        //}
        //else if (m_direction == WEST)
        //{
        //    m_targetPosition = m_startingOffset;
        //    m_targetPosition.x -= DanceMagnitude;
        //}

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
        if (m_danceNumber % 2 == 0)
            DoDiamondDance();
        else
            DoSpinDance(); 
    }

    public void ChangeDance() {
        m_danceNumber++;
    }

}
