using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLevelBounds : MonoBehaviour
{
    public Collider2D CameraBounds;
    // Start is called before the first frame update
    void Start()
    {
        CameraBounds = GameObject.Find("CameraBounds").GetComponent<Collider2D>();
        GetComponent<CinemachineConfiner>().m_BoundingShape2D = CameraBounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
