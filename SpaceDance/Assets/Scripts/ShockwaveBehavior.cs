using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveBehavior : MonoBehaviour
{

    private float m_DeathTimer = 0.25f;
    private float m_speed = 80f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_DeathTimer -= Time.deltaTime;
        if (m_DeathTimer < 0)
        {
            GameObject.Destroy(this.gameObject);
        }
        Vector3 scale = this.transform.localScale;
        scale.x += m_speed * Time.deltaTime;
        scale.y += m_speed * Time.deltaTime;
        this.transform.localScale = scale;
        transform.Rotate(0f, 0f, m_speed * Time.deltaTime);
    }
}

