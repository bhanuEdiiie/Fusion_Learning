using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class HPHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHpChanged))]
    byte HP { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    public NetworkBool IsDead { get; set; }
    [SerializeField]
    byte startingHp = 5;

    public Color uiOnHitColor;
    public Color defaultBodyColour;
    public Image uiOnHitImage;
    NetworkBool isIntialized;


    public MeshRenderer bodyMeshRender;
    
    // Start is called before the first frame update
    public override void Spawned()
    {
        HP = startingHp;
        IsDead = false;
        defaultBodyColour = bodyMeshRender.material.color;
        isIntialized = true;
    }



    [ContextMenu("Set Health")]
    private void SetHealth()
    {
        HP = 250;
    }


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
            Debug.Log("........1");
        }

    }

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
    
    static void OnHpChanged(Changed<HPHandler> changed)
    {
        Debug.Log($"{Time.time} Hp changed {changed.Behaviour.HP}");

        byte newHp = changed.Behaviour.HP;

        changed.LoadOld();
        byte oldHp = changed.Behaviour.HP;

        if (newHp < oldHp)
            changed.Behaviour.OnHPReduced();

    }

    void OnHPReduced()
    {
        if (!isIntialized)
            return;
        StartCoroutine(OnHitCO());

    }
    static void OnStateChanged(Changed<HPHandler> changed)
    {
        Debug.Log($"{Time.time} IsDead state changed {changed.Behaviour.IsDead}");

    }
}
