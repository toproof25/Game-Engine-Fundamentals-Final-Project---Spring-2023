using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffScript : MonoBehaviour, IGetItem
{
    private PlayerStateInfo playerStateInfo;
    private float buffValue = 10f;
    private void Start()
    {
        playerStateInfo = UiManager.instance.playerStateInfo;
    }
    public void GetBuff()
    {
        playerStateInfo.Damage += buffValue;
        Destroy(gameObject);
    }
    
}

