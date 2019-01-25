using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to object (probably an invisible one) that we want to follow user input,
/// whether mouse or touch behavior.
/// </summary>
public class InputTracking : MonoBehaviour {

    public float Speed = 1f;
    private float m_InputZOffset = 10f;
    private Camera m_Camera;


	// Use this for initialization
	void Start () {
        m_Camera = FindObjectOfType<Camera>();
    }

    private bool HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            FollowInputPosition(Input.mousePosition);
            return true;
        }
        return false;
    }

    private bool HandleTouchInput()
    {
        if(Input.touchCount == 1)
        {
            FollowInputPosition(Input.GetTouch(0).position);
            return true;
        }
        return false;
    }

    private void FollowInputPosition(Vector3 targetPosition)
    {
        targetPosition.z = m_InputZOffset;
        targetPosition = m_Camera.ScreenToWorldPoint(targetPosition);
        this.transform.position = targetPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleMouseInput();
        HandleTouchInput();
    }
}
