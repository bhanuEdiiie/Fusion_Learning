using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BulletScript : NetworkBehaviour
{
    [SerializeField]
    float speed;
    public override void FixedUpdateNetwork()
    {
        this.transform.position += speed * transform.forward * Runner.DeltaTime;
    }
    
}
