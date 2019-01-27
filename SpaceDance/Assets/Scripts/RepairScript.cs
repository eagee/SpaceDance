using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairScript : MonoBehaviour
{
    public GameObject SparkObject;
    public float MaxTimeout = 3f;
    public bool RequireTrigger = false;
    private float m_timer = 0f;
    private float m_maxTime = 1f;
    private bool m_active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Robot")
        {
            m_active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_active || RequireTrigger == false)
        { 
            m_timer += Time.deltaTime;
            if(m_timer > m_maxTime)
            {
                m_timer = 0f;
                m_maxTime = Random.Range(1f, MaxTimeout);
                Vector3 effectPosition = this.transform.position;
                effectPosition.y += Random.Range(1f, 2.5f);
                effectPosition.x += Random.Range(-1.5f, 1.5f);
                GameObject.Instantiate(SparkObject, effectPosition, this.transform.rotation);
            }
        }
    }
}
