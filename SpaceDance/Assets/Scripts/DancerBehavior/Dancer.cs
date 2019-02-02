using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

/// <summary>
/// IBeatObserver implementation that can be used to handle all dance behavior and character animations.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(Animator))]
public class Dancer : MonoBehaviour, IBeatObserver
{
    /// <summary>
    ///  Direction and behavioral values/consts 
    /// </summary>
    private const int DORMANT = -1;
    private const int SOUTH = 0;
    private const int EAST = 1;
    private const int NORTH = 2;
    private const int WEST = 3;
    private const int SPIN = 4;
    private int m_direction;

    public float StartingHealth = 1f;
    public DancerStatus StartingStatus = DancerStatus.Controllable;
    public float ControllableSpeed = 2f;
    public float DanceMagnitude = 2f;
    public Transform TransformToFollow;
    public Dancer DanceBuddy;
    public GameObject SparkEffect;
    public List<Transform> RepairPoints;
    public float RepairTime = 5f;
    private int m_repairIndex = 0;
    private float m_repairTimer = 10f;
    public int ActiveDirection = NORTH;

    /// <summary>
    ///  This is the state that robots will return to after a dance, provided they're not dead.
    /// </summary>
    private DancerStatus m_currentStatus;
    private float m_currentHealth;

    public float CurrentHealth
    {
        get { return m_currentHealth; }
        set {
            m_currentHealth = value;
            if(m_currentHealth < 0f)
            {
                m_currentHealth = 0f;
                SetDancerStatus(DancerStatus.Dead);
            }
        }
    }
    private float m_HorizontalVelocity;
    private float m_VerticalVelocity;
    private int m_danceNumber;
    private Vector3 m_targetPosition;
    private Vector3 m_startingOffset;
    private float m_floatingSpeed = 0.01f;

    public DancerStatus GetDancerStatus()
    {
        return m_currentStatus;
    }

    public void SetDancerStatus(DancerStatus status)
    {
        //Debug.Log("SetDancerStatus Dancer: " + gameObject.name + " Status: " + status.ToString());
        if (m_currentStatus == DancerStatus.Dormant && status != DancerStatus.Dormant && status != DancerStatus.Controllable)
        {
            GameObject.Instantiate(SparkEffect, this.transform.position, this.transform.rotation);
        }

        if (m_currentStatus != DancerStatus.Dead)
            m_currentStatus = status;

        if(m_currentStatus == DancerStatus.Controllable || m_currentStatus == DancerStatus.Active)
        {
            GetComponent<Animator>().SetBool("Dancing", false);
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        if(m_currentStatus == DancerStatus.Dormant)
        {
            GetComponent<Animator>().SetBool("Dancing", false);
            GetComponent<Animator>().SetInteger("Direction", DORMANT); 
        }
        else if (m_currentStatus == DancerStatus.Dead)
        {
            GetComponent<Animator>().SetInteger("Direction", DORMANT);
            GetComponentInChildren<RepairScript>().enabled = true;
        }
        
    }

    public void SetDanceNumber(int danceNumber)
    {
        m_danceNumber = danceNumber;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_danceNumber = 0;
        SetDancerStatus(StartingStatus);
        CurrentHealth = StartingHealth;
        m_startingOffset = this.transform.position;
        m_repairIndex = 0;
        m_repairTimer = 3f;
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

    private void LerpToTargetPosition(float speed = 2f)
    {
        Vector3 vec = this.transform.position;
        vec = Vector3.Lerp(vec, m_targetPosition, speed * Time.deltaTime);
        this.transform.position = vec;
    }

    private void DoSpinDance()
    {
        m_targetPosition = TransformToFollow.position;
        GetComponent<Animator>().SetBool("Dancing", false);
        GetComponent<Animator>().speed = 0.5f;

        // Set our sorting order assuming y depth translates to z depth
        if (DanceBuddy.transform.position.y > this.transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        // Handle our movement animation
        if (GetComponent<Animator>().GetInteger("Direction") != SPIN)
            GetComponent<Animator>().SetInteger("Direction", SPIN);

        m_direction++;
        if (m_direction > WEST)
        {
            m_direction = SOUTH;
        }
    }

    private void DoWaltz()
    {
        GetComponent<Animator>().speed = 1.0f;
        GetComponent<Animator>().SetBool("Dancing", false);
        m_targetPosition = TransformToFollow.position;

        // Set our sorting order assuming y depth translates to z depth
        if (DanceBuddy.transform.position.y > this.transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        float angle = GetAngleToVector(DanceBuddy.transform.position);

        if ((angle >= 315f && angle <= 360f) || (angle >= 0f && angle < 45f))
        {
            //if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "E: " + angle.ToString();
            m_direction = EAST;
        }
        else if (angle >= 45f && angle < 135f)
        {
            //if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "N: " + angle.ToString();
            m_direction = NORTH;
        }
        else if (angle >= 135f && angle < 225f)
        {
            //if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "W: " + angle.ToString();
            m_direction = WEST;
        }
        else if (angle >= 225f && angle < 315f)
        {
            //if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "S: " + angle.ToString();
            m_direction = SOUTH;
        }

        GetComponent<Animator>().SetInteger("Direction", m_direction);
    }

    private float GetAngleToVector(Vector3 target)
    {
        Vector3 vectorToTarget = target - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) / Mathf.PI) * 180f;
        //angle = Mathf.Clamp(angle, -360f, 360f);
        if (angle < 0) angle += 360f;
        return angle;
    }

    void HandleCurrentDance()
    {
        if (m_danceNumber == 0)
        {
            GetComponent<Animator>().SetBool("Dancing", true);
            // Set our sorting order assuming y depth translates to z depth
            if (DanceBuddy.transform.position.y > this.transform.position.y)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            LerpToTargetPosition();
        }
        else if (m_danceNumber == 1)
        {
            LerpToTargetPosition();
            DoSpinDance();
        }
        else if (m_danceNumber == 2)
        {
            LerpToTargetPosition(8f);
            DoWaltz();
        }
    }

    /// <summary>
    /// Sets the current animation based on the current polynav state.
    /// </summary>
    void SetPolyNavAnimation()
    {
        PolyNavAgent nav = GetComponent<PolyNavAgent>();
        Vector2 direction = nav.movingDirection;
        int animDirection;

        if(Mathf.Abs(direction.x) <= 0.1f && Mathf.Abs(direction.y) <= 0.1f)
        {
            animDirection = ActiveDirection;
            GetComponent<Animator>().SetBool("Dancing", true);
        }
        else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            GetComponent<Animator>().SetBool("Dancing", false);
            if (direction.x > 0)
            {
                animDirection = EAST;
            }
            else
            {
                animDirection = WEST;
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("Dancing", false);
            if (direction.y > 0)
            {
                animDirection = NORTH;
            }
            else
            {
                animDirection = SOUTH;
            }
        }
        GetComponent<Animator>().SetInteger("Direction", animDirection);
    }

    void HandleActiveBehavior()
    {
        //Debug.Log("Shield bot velocity: " + GetComponent<Rigidbody2D>().velocity.ToString());
        SetPolyNavAnimation();
        m_repairTimer += Time.deltaTime;
        if(m_repairTimer >= RepairTime)
        {
            m_repairTimer = 0f;
            m_repairIndex++;
            if(m_repairIndex > RepairPoints.Count - 1)
            {
                m_repairIndex = 0;
            }
            if(RepairPoints[m_repairIndex] != null)
            {
                GetComponent<PolyNavAgent>().SetDestination(RepairPoints[m_repairIndex].position);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_currentStatus == DancerStatus.Controllable)
        {
            HandleControllableState();
        }
    }

    void HandleDeadBehavior()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * Time.deltaTime * m_floatingSpeed);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * Time.deltaTime * m_floatingSpeed);
        GetComponent<Rigidbody2D>().freezeRotation = false;
        m_floatingSpeed -= 0.0001f;
        if (m_floatingSpeed < 0f) m_floatingSpeed = 0f;
        transform.Rotate(0f, 0f, 0.08f);
    }

    void Update()
    {
        if (m_currentStatus == DancerStatus.Dancing)
        {
            HandleCurrentDance();
        }
        else if(m_currentStatus == DancerStatus.Active)
        {
            HandleActiveBehavior();
        }
        else if (m_currentStatus == DancerStatus.Dead)
        {
            HandleDeadBehavior();
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
        if (m_currentStatus == DancerStatus.Dancing && m_danceNumber == 0)
        {
            SetupDiamondDance();
        }
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
            // do nothing
        }
    }

    public void OnDanceStarted()
    {

    }
}
