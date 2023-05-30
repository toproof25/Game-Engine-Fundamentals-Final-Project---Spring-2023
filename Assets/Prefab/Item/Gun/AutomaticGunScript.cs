using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGunScript : MonoBehaviour, IGetGun
{
    private PlayerStateInfo playerStateInfo;
    private float damage = 20f;
    private string gunName = "Automatic Gun";

    private bool move = true;
    private void Start()
    {
        playerStateInfo = UiManager.instance.playerStateInfo;
    }
    public void GetItem()
    {
        playerStateInfo.secondGunDamage = damage;
        playerStateInfo.secondGunName = gunName;
        playerStateInfo.TotalNumberOfBullets[1] += 30;
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Rotate(0f, -50f * Time.deltaTime, 0f);

        if(move)
        {
            transform.position += Vector3.up * (Time.deltaTime/2);
            if (transform.position.y >= 1f)
                move = !move;
        }
        else
        {
            transform.position -= Vector3.up * (Time.deltaTime/2);
            if (transform.position.y <= 0.5f)
                move = !move;
        }
            
    }

}

