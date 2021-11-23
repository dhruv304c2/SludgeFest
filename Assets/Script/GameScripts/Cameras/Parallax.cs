using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Vector2 StartPosition;
    Vector2 StartPositionOffset;
    Vector2 CameraStartPosition;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] float ParallaxFactor;
    
    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
        CameraStartPosition = PlayerCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2((PlayerCamera.transform.position.x - CameraStartPosition.x) * ParallaxFactor+StartPosition.x,
                                          (PlayerCamera.transform.position.y-CameraStartPosition.y)+ StartPosition.y);        
    }
}
