using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBuffScript : MonoBehaviour, IGetItem
{
    private PlayerStateInfo playerStateInfo;
    private float buffValue = 20f;
    private void Start()
    {
        playerStateInfo = UiManager.instance.playerStateInfo;
    }
    public void GetBuff()
    {
        playerStateInfo.HP += buffValue;
        Destroy(gameObject);
    }
    
}

