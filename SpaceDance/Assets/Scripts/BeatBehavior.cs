using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBehavior : MonoBehaviour, IBeatObserver
{

    public string Text;
    public float Opacity;
    public GameObject SuccessEffect;
    public GameObject FailEffect;
    private bool m_activeKeyPressed;
    private TextMesh m_TextMesh;
    private KeyCode m_StringKeyCode;
    private bool m_inContactWithStar = false;
    private bool m_successful;
    private int m_OnsetIndex;

    // Use this for initialization
    void Start () {
        m_successful = false;
        m_activeKeyPressed = false;
        m_TextMesh = GetComponentInChildren<TextMesh>();
        m_TextMesh.text = Text;
        Color tcolor = Color.white;
        tcolor.a = Opacity;
        m_TextMesh.color = tcolor;
        m_inContactWithStar = false;
        m_StringKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), "S");
    }

    void CheckKeyPressed()
    {
        if(Input.GetKeyDown(m_StringKeyCode))
        {
            Debug.Log("Key Down!");
            m_activeKeyPressed = true;
        }
        else if (Input.GetKeyUp(m_StringKeyCode))
        {
            Debug.Log("Key Up!");
            m_activeKeyPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Star")
        {
            Debug.Log("Coll In!");
            m_inContactWithStar = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Star")
        {
            Debug.Log("Coll Out!");
            if (!m_successful)
            {
                m_inContactWithStar = false;
                //HandleFailure();
            }
        }
    }

    private void HandleFailure()
    {
        Instantiate(FailEffect, this.transform.position, this.transform.rotation);
        GameObject.Destroy(this.gameObject);
    }

    private void HandleSuccess()
    {
        m_successful = true;
        Instantiate(SuccessEffect, this.transform.position, this.transform.rotation);
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
        CheckKeyPressed();
        if (m_activeKeyPressed && m_inContactWithStar)
        {
            //HandleSuccess();
        }
        else if(m_activeKeyPressed && !m_inContactWithStar)
        {
            //HandleFailure();
        }
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
        Destroy(this.gameObject);
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
            return m_OnsetIndex;
        }
        set
        {
            m_OnsetIndex = value;
        }
    }
}
