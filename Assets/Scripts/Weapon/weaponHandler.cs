using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class weaponHandler : NetworkBehaviour
{
    [Networked(OnChanged = "OnFireChanged")]
    public NetworkBool isFiring { get; set; }
    float lastTimeFired = 0;
    [SerializeField] Transform aimPoint;
    public Animator fireAnimator;
    [SerializeField] LayerMask shootables;
    [SerializeField]GameObject hittedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Time.time);
      
    }

    public override void FixedUpdateNetwork()
    {
        //Debug.Log(GetInput(out NetworkInputData networkInputData));
        if(GetInput(out NetworkInputData networkInputData))
        {
            if(networkInputData.isFired)
            {
                //Debug.Log(networkInputData.isFired);
                Fire(networkInputData.aimForwardVector);
                Debug.Log("Idhar aaya 1 " + this.name);
            }
        }
    }
    void Fire(Vector3 aimForwardVector)
    {
        if (Time.time - lastTimeFired < 0.15f)
            return;

        StartCoroutine(FireEffect());
        Runner.LagCompensation.Raycast(aimPoint.position, aimForwardVector,100,Object.InputAuthority,out var hitInfo, shootables,HitOptions.IncludePhysX);
        hittedPlayer = hitInfo.GameObject;
        float hitDistance = 100;
        bool isHitOtherPlayer = false;

        if(hitInfo.Hitbox !=null)
        {

            Debug.Log(Time.time + " " + hitInfo.Hitbox.transform.root.name);

            if (Object.HasStateAuthority)
                hitInfo.Hitbox.transform.root.GetComponent<HPHandler>().OnTakeDamage();
            isHitOtherPlayer = true;
        }
        else if(hitInfo.Collider != null)
        {
            Debug.Log(Time.time + " " + hitInfo.Collider.transform.name);
        }

        // Debug for hit 
        if (isHitOtherPlayer)
        {
            Debug.Log("Working 1");
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.red, 1);
        }
        else
        {
            Debug.Log("Working 2");
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.green, 1);
        }

        lastTimeFired = Time.time;
    }
   
    IEnumerator FireEffect()
    {
        isFiring = true;
        fireAnimator.enabled = true;
        fireAnimator.Play("FireAnim");

        yield return new WaitForSeconds(0.09f);
        isFiring = false;
        fireAnimator.Rebind();
    }
   
    

    static void OnFireChanged(Changed<weaponHandler> changed)
    {
        bool firingCurrent = changed.Behaviour.isFiring;

        changed.LoadOld();

        bool firingOld = changed.Behaviour.isFiring;

        if (firingCurrent && !firingOld)
        {
            changed.Behaviour.OnFireRemote(true);
        }
        else
        {
            changed.Behaviour.OnFireRemote(false);
        }

    }
     
    void OnFireRemote(bool toPlay)
    {
        if(!Object.HasInputAuthority)
        {
            if (toPlay)
            {
                fireAnimator.enabled = true;
                fireAnimator.Play("FireAnim");
            }
            else
            {
                fireAnimator.enabled = false;
                fireAnimator.Rebind();
            }
        }
    }
}
