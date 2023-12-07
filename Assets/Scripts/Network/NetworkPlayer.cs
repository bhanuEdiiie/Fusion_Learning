using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour , IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    public Slider healthBar;

    public Transform model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {
        if(Object.HasInputAuthority)
        {
            Local = this;
            Utils.SetLayerOfChildren(model, LayerMask.NameToLayer("Playerskin"));
        }
        else
        {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
            healthBar.gameObject.SetActive(false);
            Debug.Log("spawned remote player");
        }
    }
    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
