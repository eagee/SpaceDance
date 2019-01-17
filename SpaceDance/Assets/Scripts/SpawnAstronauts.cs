using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAstronauts : MonoBehaviour
{
    public GameObject[] ObjectPrefabs;
    public GameObject[] TargetObjects;
    public float MinTimeToSpawn = 3f;
    public float MaxTimeToSpawn = 6f;
    private float m_Timer = 3f;

    // Use this for initialization
    void Start()
    {
        m_Timer = Random.Range(MinTimeToSpawn, MaxTimeToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer < 0f)
        {
            m_Timer = Random.Range(MinTimeToSpawn, MaxTimeToSpawn);

            int spawnElement = (int)Random.Range(0f, (float)ObjectPrefabs.Length);
            int targetElement = (int)Random.Range(0f, (float)TargetObjects.Length);

            Debug.Log("Spawn el: " + spawnElement.ToString());
            Debug.Log("Spawn ta: " + targetElement.ToString());

            GameObject newObject = (GameObject)Instantiate(ObjectPrefabs[spawnElement], this.transform.position, this.transform.rotation);
            newObject.GetComponent<Rigidbody>().AddForce((TargetObjects[targetElement].transform.position - this.transform.position) * 6f);
        }
    }
}
