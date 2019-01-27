using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform mainCamera;
    public float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.x = -mainCamera.position.x * speed;
        pos.y = -mainCamera.position.y * speed;
        transform.position = pos;
    }
}
