using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    ParticleSystem System = null;
    [SerializeField] SplatObject splatObject;
    
    // Start is called before the first frame update
    
    void Start()
    {
        System = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();
        int EvenCount = System.GetCollisionEvents(other,particleCollisionEvents);
        for(int i = 0;i<EvenCount;i++)
        {
            Instantiate(splatObject, particleCollisionEvents[i].intersection, Quaternion.identity);            
        }
    }

}
