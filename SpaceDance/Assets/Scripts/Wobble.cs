using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour {

    public float WobbleSize = 0.25f;
    private float yStartingScale = 0f;
    private bool m_wobbleOn = false;
    // Use this for initialization
    void Start () {
        yStartingScale = this.transform.localScale.y;
    }

    float GetWobble()
    {
        m_wobbleOn = !m_wobbleOn;
        if (m_wobbleOn)
            return WobbleSize;
        return 0f;
    }

    // Update is called once per frame
    void Update () {
        Vector3 scale = this.transform.localScale;
        scale.y = yStartingScale + GetWobble();
        this.transform.localScale = scale;
    }
}
