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
        if(m_currentStatus == DancerStatus.Controllable)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
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
        m_currentHealth = StartingHealth;
        m_startingOffset = this.transform.position;
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
        GetComponent<Animator>().SetBool ("Dancing", true);
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
        GetComponent<Animator>().SetBool("Dancing", true);
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
            if (GetComponentInChildren<TextMesh>()) GetComponentInChildren<TextMesh>().text = "E: " + angle.ToString();
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_currentStatus == DancerStatus.Controllable)
        {
            HandleControllableState();
        }
    }

    void Update()
    {
        if (m_currentStatus == DancerStatus.Dancing)
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
        if (m_currentStatus == DancerStatus.Dancing && m_danceNumber == 0)
        {
            SetupDiamondDance();
        }
    }

    public void OnChange(int index, float change)
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
