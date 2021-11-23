using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FollowTarget : MonoBehaviourPun
{
    [SerializeField] Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Follow();
    }
    
    [PunRPC]

    public void Follow()
    {
        transform.position = Target.position;
    }
}
