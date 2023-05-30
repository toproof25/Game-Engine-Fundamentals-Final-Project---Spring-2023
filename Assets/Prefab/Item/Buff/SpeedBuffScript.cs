using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffScript : MonoBehaviour, IGetItem
{
    private PlayerStateInfo playerStateInfo;
    private float buffValue = 1f;
    private void Start()
    {
        playerStateInfo = UiManager.instance.playerStateInfo;
    }
    public void GetBuff()
    {
        playerStateInfo.Speed += buffValue;
        Destroy(gameObject);
    }

   
}

