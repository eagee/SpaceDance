using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMonitor : MonoBehaviour
{
    public Dancer DancerToMonitor;
    public string Prefix = "Shields: ";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMesh>().text = Prefix + DancerToMonitor.CurrentHealth.ToString() + "%";
    }
}
