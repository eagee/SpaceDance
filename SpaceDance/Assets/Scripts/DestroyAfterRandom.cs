using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterRandom : MonoBehaviour
{
    public float MaxTimeout = 1f;
    private float m_timer = 0f;
    private float m_maxTime = 1f;

    void Start()
    {
        m_maxTime = Random.Range(0f, MaxTimeout);
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_maxTime)
        {
            m_timer = 0f;
            GameObject.Destroy(this.gameObject);
        }
    }
}
