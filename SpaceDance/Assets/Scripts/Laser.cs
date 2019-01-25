using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class Laser : MonoBehaviour, IBeatObserver
{
    public float MinWarmingTime = 0.5f;
    public float MaxWarmingTime = 2;
    public float MinFiringTime = 2;
    public float MaxFiringTime = 4;
    public float MinFiringScale = 0f;
    public float MaxFiringScale = 0.1f;
    public float WobbleSize = 0.01f;
    public LayerMask PhaserInteractionLayerMask;
    
    public LaserTarget m_currentTarget;
    static private int m_targetIndex;
    //static private PhaserTarget[] m_potentialTargets = new PhaserTarget[1];
    static private LaserState m_state;
    static private LaserState m_lastState;
    static private float m_currentStateTime;
    static private float m_lineWidth;
    private bool m_wobbleOn = false;
    static private bool m_UseLastTargetLocation = false;
    private Vector3 m_lastTargetLocation;
    private int m_OnsetIndex;

    void Start()
    {
        m_UseLastTargetLocation = false;
        m_lineWidth = 0f;
        ChangeState(LaserState.WarmingUp);
    }

    void ChangeState(LaserState state)
    {
        m_state = state;
        m_currentStateTime = GetTimeoutForState();
        //Debug.Log("Phaser: Changing state to: " + m_state.ToString());
        //if (state == LaserState.WarmingUp)
        //{
        //    m_currentTarget = null;
        //}
        //if(state == LaserState.Off)
        //{
        //    //GameObject.Destroy(this.gameObject);
        //}
    }

    //void SelectTarget()
    //{
    //    m_potentialTargets = FindObjectsOfType<LaserTarget>();
    //    m_currentTarget = null;
    //    if (m_potentialTargets.Length > 0)
    //    {
    //        for (int x = 0; x < m_potentialTargets.Length; x++)
    //        {
    //            if (m_potentialTargets[x].tag == "BeatTarget")
    //            {
    //                //Debug.Log("Phaser: Target Found!");
    //                m_targetIndex = x;
    //                m_currentTarget = m_potentialTargets[x];
    //                m_UseLastTargetLocation = false;
    //            }
    //        }
    //    }
    //
    //    // If we can't find a target, then we return our state to off (nothing to fire at, so destory self)
    //    if (m_currentTarget == null)
    //    {
    //        //Debug.Log("Phaser: No Target Found!");
    //        ChangeState(LaserState.Off);
    //    }
    //}

    void HandleStateChanges()
    {
        m_currentStateTime -= Time.deltaTime;
        if (m_currentStateTime < 0)
        {
            if (m_state == LaserState.WarmingUp)
            {
                ChangeState(LaserState.Firing);
                //SelectTarget();
            }
            else if(m_state == LaserState.Firing)
            {
                ChangeState(LaserState.PoweringDown);
            }
            else
            {
                ChangeState(LaserState.Off);
                //GameObject.Destroy(this.gameObject);
            }
        }
    }

    float GetTimeoutForState()
    {
        if (m_state == LaserState.WarmingUp)
        {
            return Random.Range(MinWarmingTime, MaxWarmingTime);
        }
        else// if (m_state == LaserState.Firing)
        {
            return Random.Range(MinFiringTime, MaxFiringTime);
        }
    }

    float GetWobble()
    {
        m_wobbleOn = !m_wobbleOn;
        if (m_wobbleOn)
            return WobbleSize;
        return 0f;
    }

    void HandleSizeChanges(float wobble)
    {
        if (m_state == LaserState.PoweringDown)
        {
            Vector3 scale = this.transform.localScale;
            scale.x = Mathf.Lerp(scale.x, MinFiringScale, Time.deltaTime * 2f);
            scale.y = scale.x;
            this.transform.localScale = scale;
        }
        else if (m_state == LaserState.WarmingUp)
        {
            Vector3 scale = this.transform.localScale;
            scale.x = Mathf.Lerp(scale.x, MaxFiringScale * 2f, Time.deltaTime);
            scale.y = scale.x;
            this.transform.localScale = scale;
        }
        else
        {
            Vector3 scale = this.transform.localScale;
            scale.x = Mathf.Lerp(scale.x, MaxFiringScale * 2f, 1f) + wobble;
            scale.y = scale.x;
            this.transform.localScale = scale;
        }
    }

    void HandlePhaserSizeChanges(float wobble)
    {
        //if (m_state == LaserState.Off)
        //{
        //    m_lineWidth = Mathf.Lerp(m_lineWidth, MinFiringScale, Time.deltaTime * 2f);
        //    GetComponent<LineRenderer>().startWidth = (m_lineWidth / 2f);
        //    GetComponent<LineRenderer>().endWidth = m_lineWidth;
        //}
        //else 
        if (m_state == LaserState.WarmingUp)
        {
            m_lineWidth = Mathf.Lerp(m_lineWidth, MinFiringScale, Time.deltaTime * 2f);
            GetComponent<LineRenderer>().startWidth = (m_lineWidth / 2f);
            GetComponent<LineRenderer>().endWidth = m_lineWidth;
        }
        else if (m_state == LaserState.PoweringDown)
        {
            m_lineWidth = Mathf.Lerp(m_lineWidth, MinFiringScale, Time.deltaTime * 4f);
            GetComponent<LineRenderer>().startWidth = (m_lineWidth / 2f);
            GetComponent<LineRenderer>().endWidth = m_lineWidth;

        }
        else
        {
            m_lineWidth = Mathf.Lerp(m_lineWidth, MaxFiringScale, Time.deltaTime);
            GetComponent<LineRenderer>().startWidth = (m_lineWidth / 2f) + wobble;
            GetComponent<LineRenderer>().endWidth = m_lineWidth + wobble;
        }
    }

    void SetLineRendererPosition()
    {
        Vector3 targetLocation;
        if (!m_UseLastTargetLocation)
        {
            targetLocation = m_currentTarget.transform.position;
            m_lastTargetLocation = targetLocation;
        }
        else
        {
            targetLocation = m_lastTargetLocation;
        }
        this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
        this.GetComponent<LineRenderer>().SetPosition(1, targetLocation);
    }

    //void HandlePhaserTargetChanges()
    //{
    //    if (m_state == LaserState.Firing || m_state == LaserState.Idle)
    //    {
    //        if (m_potentialTargets == null)
    //        {
    //            return;
    //        }
    //        else if (m_currentTarget == null && m_potentialTargets.Length - 1 >= m_targetIndex && m_potentialTargets[m_targetIndex] != null)
    //        {
    //            m_currentTarget = m_potentialTargets[m_targetIndex];
    //        }
    //        else if (m_potentialTargets[m_targetIndex] == null)
    //        {
    //            m_currentStateTime = 0f;
    //            return;
    //        }
    //        else if (m_currentTarget != m_potentialTargets[m_targetIndex])
    //        {
    //            SetLineRendererPosition();
    //        }
    //        else if (m_currentTarget == m_potentialTargets[m_targetIndex])
    //        {
    //            SetLineRendererPosition();
    //        }
    //
    //    }
    //}

    //void SwitchTargetIfInRaycast()
    //{
    //    if (m_state == LaserState.WarmingUp || m_currentTarget == null)
    //        return;
    //
    //    RaycastHit hit;
    //    //Debug.DrawRay(transform.position, m_currentTarget.transform.position - transform.position, Color.yellow);
    //    Physics.Raycast(transform.position, (m_currentTarget.transform.position - transform.position), out hit, PhaserInteractionLayerMask);
    //    if (hit.transform.gameObject != null)
    //    {
    //
    //        PhaserTarget phaserTarget = hit.transform.gameObject.GetComponent<PhaserTarget>();
    //        if (phaserTarget != null && m_currentTarget != phaserTarget)
    //        {
    //            m_currentTarget = phaserTarget;
    //        }
    //    }
    //}

    //void HandlePhaserEffectsForTarget()
    //{
    //    if (m_currentTarget != null && m_state == LaserState.Firing)
    //    {
    //        m_currentTarget.HandlePhaserEffects(this);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        float wobble = GetWobble();

        m_lastState = m_state;

        HandleStateChanges();
        //HandlePhaserTargetChanges();
        SetLineRendererPosition();
        HandleSizeChanges(wobble);
        HandlePhaserSizeChanges(wobble);
        //SwitchTargetIfInRaycast();
        //HandlePhaserEffectsForTarget();
    }

    public string Tag()
    {
        return this.gameObject.tag;
    }

    public void OnSongLoaded()
    {

    }

    public void OnSongEnded()
    {
        //Destroy(this.gameObject);
    }

    public void OnBeat(Beat beat)
    {
        // TODO: perhaps shift colors on beat or add some kind of animation.
    }

    public void OnOnset(OnsetType type, Onset onset)
    {
        // TODO: Perhaps changes colors based on onset type
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
            return m_OnsetIndex;
        }
        set
        {
            m_OnsetIndex = value;
        }
    }
}
