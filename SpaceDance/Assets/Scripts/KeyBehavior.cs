using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KeyBehavior : MonoBehaviour {

    public string Text;
    public int Index;
    public GameObject SuccessEffect;
    public GameObject FailEffect;
    private bool m_activeKeyPressed;
    private TextMesh m_TextMesh;
    private KeyCode m_StringKeyCode;

    // Use this for initialization
    void Start () {
        m_activeKeyPressed = false;
        m_TextMesh = GetComponentInChildren<TextMesh>();
        m_TextMesh.text = Text;
        m_StringKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), Text.ToUpper());
    }

    void CheckKeyPressed()
    {
        if(Input.GetKeyDown(m_StringKeyCode))
        {
            m_activeKeyPressed = true;
        }
        else if (Input.GetKeyUp(m_StringKeyCode))
        {
            m_activeKeyPressed = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckKeyPressed();
        if(m_activeKeyPressed) // and something else like collision with a beat object
        {
            GameObject effect = (GameObject)Instantiate(SuccessEffect, this.transform.position, this.transform.rotation);
            GameObject.Destroy(this.gameObject);
        }

    }
}
