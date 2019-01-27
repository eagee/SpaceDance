using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceZone : MonoBehaviour, IBeatObserver
{
    enum DanceZoneState
    {
        Idle,
        Dancing,
        Success,
        Failure
    }

    public string Story;
    public Dancer DormantRobot;
    public Dancer PlayerRobot;
    public DanceSpinBar SpinBar;
    public Transform DanceTargetL;
    public Transform DanceTargerR;
    public UnityStandardAssets._2D.Camera2DFollow Camera;
    public Transform DanceCameraTarget;
    public List<int> DanceRotation;

    private int m_currentDanceNumber;
    private DanceZoneState m_currentState;

    // Start is called before the first frame update
    void Start()
    {
        m_currentDanceNumber = 0;
        SetState(DanceZoneState.Idle);
    }

    void SetState(DanceZoneState state)
    {
        //Debug.Log("SetState Dance Zone: " + gameObject.name + " State: " + state.ToString());
        if(m_currentState != DanceZoneState.Failure && m_currentState != DanceZoneState.Success)
        m_currentState = state;
        if(m_currentState == DanceZoneState.Dancing)
        {
            Camera.target = DanceCameraTarget;
        }
        else
        {
            Camera.target = PlayerRobot.transform;
        }
        if(m_currentState == DanceZoneState.Success)
        {
            DormantRobot.SetDancerStatus(DancerStatus.Active);
            PlayerRobot.SetDancerStatus(DancerStatus.Controllable);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && m_currentState == DanceZoneState.Idle)
        {
            Debug.Log("Trigger Entered: " + col.gameObject);
            PlayerRobot = col.gameObject.GetComponent<Dancer>();
            PlayerRobot.TransformToFollow = DanceTargerR;
            DormantRobot.TransformToFollow = DanceTargetL;
            PlayerRobot.DanceBuddy = DormantRobot;
            DormantRobot.DanceBuddy = PlayerRobot;
            PlayerRobot.SetDancerStatus(DancerStatus.Dancing);
            DormantRobot.SetDancerStatus(DancerStatus.Dancing);
            UpdateDanceNumbers();
            SetState(DanceZoneState.Dancing);
            // TODO: Trigger a song change in the RythmController
        }
    }
    private void UpdateDanceNumbers()
    {
        if (DormantRobot != null)
            DormantRobot.SetDanceNumber(DanceRotation[m_currentDanceNumber]);
        if (PlayerRobot != null)
            PlayerRobot.SetDanceNumber(DanceRotation[m_currentDanceNumber]);
        if (SpinBar != null)
            SpinBar.SetDanceNumber(DanceRotation[m_currentDanceNumber]);
    }

    void ChangeDanceNumber()
    {
        m_currentDanceNumber++;
        if(m_currentDanceNumber > DanceRotation.Count - 1)
        {
            m_currentDanceNumber = 0;
            // TODO: REmove this next line
            SetState(DanceZoneState.Success);
        }
        Debug.Log("Changing dance number to: " + m_currentDanceNumber.ToString());
        UpdateDanceNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Tag()
    {
        return gameObject.tag;
    }

    public void OnSongLoaded()
    {
        ChangeDanceNumber();
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
        ChangeDanceNumber();
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
