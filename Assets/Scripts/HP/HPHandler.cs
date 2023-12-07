using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HPHandler : NetworkBehaviour
{

    // Declaration of Fields and properties
    #region    
    [Networked(OnChanged = nameof(OnHpChanged))]
    public byte HP { get; set; }
    [Networked(OnChanged = nameof(OnStateChanged))]
    public NetworkBool IsDead { get; set; }
    [Space]
    public byte startingHp;
    public Color uiOnHitColor;
    public Color uiOnDeadColor;
    public Color defaultBodyColour;
    public Image uiOnHitImage;
    NetworkBool isIntialized;
    [SerializeField] Slider healthBar;
    public MeshRenderer bodyMeshRender;
    #endregion



    //Start of method 


    // Method called on Player Spawned and to intialize values 
    public override void Spawned()
    {
        HP = startingHp;
        healthBar.maxValue = HP;
        healthBar.value = HP;
        IsDead = false;
        defaultBodyColour = bodyMeshRender.material.color;
        isIntialized = true;
    }



    // Method called to change colors for milli second and to restore on original colour 
    IEnumerator OnHitCO()
    {
        bodyMeshRender.material.color = Color.white;

        if (Object.HasInputAuthority)
            uiOnHitImage.color = uiOnHitColor;

        yield return new WaitForSeconds(.2f);
        bodyMeshRender.material.color = defaultBodyColour;
        if (Object.HasInputAuthority && !IsDead)
        {
            uiOnHitImage.color = new Color(0,0,0,0);
        }

    }



    // local method to deduct HP from state authorized sytem 
    public void OnTakeDamage()
    {
        if (IsDead)
            return;
        HP--;
        Debug.Log($"{Time.time} {transform.name} took damage got {HP} left");
        if(HP<=0)
        {
            Debug.Log($"{Time.time} {transform.name} died");
            IsDead = true;
        }
    }
    

    //Network method called On HP Changed 
    static void OnHpChanged(Changed<HPHandler> changed)
    {
        Debug.Log($"{Time.time} Hp changed {changed.Behaviour.HP}");

        byte newHp = changed.Behaviour.HP;

        changed.LoadOld();
        byte oldHp = changed.Behaviour.HP;

        if (newHp < oldHp)
        {
            changed.Behaviour.OnHPReduced();
        }
    }


    // Local function to show effect of bullet hit and update health bar 
    void OnHPReduced()  
    {
        if (!isIntialized)
            return;
        healthBar.value = HP;
        StartCoroutine(OnHitCO());
    }
    static void OnStateChanged(Changed<HPHandler> changed)
    {
        Debug.Log($"{Time.time} IsDead state changed {changed.Behaviour.IsDead}");
        changed.Behaviour.healthBar.value = 0;
        bool deadNew = changed.Behaviour.IsDead;
        changed.LoadOld();
        bool deadOld = changed.Behaviour.IsDead;
        if(deadNew && !deadOld)
        {
            changed.Behaviour.VisualEffects();
            changed.Behaviour.Respawn();
        }
    }

    void VisualEffects()
    {

    }
    void Respawn()
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("Workedd..");
            transform.root.position = Utils.GetRandomSpawnPoint();
            
        }
        if (Object.HasInputAuthority)
        {
            uiOnHitImage.color = uiOnDeadColor;
            StartCoroutine(nameof(Reseting));
        }
       
    }
    IEnumerator Reseting()
    {
        
        yield return new WaitForSeconds(2);

        if (Object.HasStateAuthority)
        {
            Debug.Log("............?");
            IsDead = false;
            HP = startingHp;
            transform.root.GetComponent<CharacterController>().enabled = false;
        }
        uiOnHitImage.color = new Color(0, 0, 0, 0);
        healthBar.value = HP;
    }
  
    private void OnTriggerEnter(Collider other)
    {
      
    }
}
