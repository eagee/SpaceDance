using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IBeatObserver implementation that can be used to handle all dance behavior and character animations.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Dancer : MonoBehaviour, IBeatObserver
{
    public float StartingHealth = 1f;
    public DancerStatus StartingStatus = DancerStatus.Controllable;
    public float ControllableSpeed = 2f;
    public float DanceMagnitude = 2f;
    public Transform TransformToFollow;
    public Dancer DanceBuddy;

    /// <summary>
    ///  Direction and behavioral values/consts 
    /// </summary>
    private const int SOUTH = 0;
    private const int EAST = 1;
    private const int NORTH = 2;
    private const int WEST = 3;
    private const int SPIN = 4;
    private int m_direction;

    /// <summary>
    ///  This is the state that robots will return to after a dance, provided they're not dead.
    /// </summary>
    private DancerStatus m_currentStatus;
    private float m_currentHealth;
    private float m_HorizontalVelocity;
    private float m_VerticalVelocity;
    private int m_danceNumber;
    private Vector3 m_targetPosition;
    private Vector3 m_startingOffset;

    public void SetDancerStatus(DancerStatus status)
    {
        m_currentStatus = status;
    }

    public void SetDanceNumber(int danceNumber)
    {
        m_danceNumber = danceNumber;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_danceNumber = 0;
        m_currentStatus = StartingStatus;
        m_currentHealth = StartingHealth;
    }

    void HandleControllableState()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        GetComponent<Rigidbody2D>().velocity = targetVelocity * ControllableSpeed;
        if (targetVelocity.x < 0)
            m_direction = WEST;
        else if (targetVelocity.x > 0)
            m_direction = EAST;
        else if (targetVelocity.y < 0)
            m_direction = SOUTH;
        else if (targetVelocity.y > 0)
            m_direction = NORTH;

        GetComponent<Animator>().SetInteger("Direction", m_direction);
    }

    private void SetupDiamondDance()
    {
        m_startingOffset = TransformToFollow.position;
        GetComponent<Animator>().speed = 1.0f;
        
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
        m_direction++;
        if (m_direction > WEST)
        {
            m_direction = SOUTH;
        }
    }

    private void LerpToTargetPosition()
    {
        Vector3 vec = this.transform.position;
        vec = Vector3.Lerp(vec, m_targetPosition, 2f * Time.deltaTime);
        this.transform.position = vec;
    }

    void HandleCurrentDance()
    {
        if (m_danceNumber == 0)
        {
            LerpToTargetPosition();
        }
        else if (m_danceNumber == 1)
        {
            LerpToTargetPosition();
        }
        else if (m_danceNumber == 2)
        {
            LerpToTargetPosition();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_currentStatus == DancerStatus.Controllable)
        {
            HandleControllableState();
        }
        else if (m_currentStatus == DancerStatus.Dancing)
        {
            HandleCurrentDance();
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

    }

    public void OnOnset(OnsetType type, Onset onset)
    {
        if (m_currentStatus == DancerStatus.Dancing) ;
            //UpdateDance();
    }

    public void OnChange(int index, float change)
    {
        if (m_currentStatus == DancerStatus.Dancing && m_danceNumber == 0)
        {
            SetupDiamondDance();
        }
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
