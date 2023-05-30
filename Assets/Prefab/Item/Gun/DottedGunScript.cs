using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedGunScript : MonoBehaviour, IGetGun
{
    private PlayerStateInfo playerStateInfo;
    private float damage = 30f;
    private string gunName = "Dotted Gun";

    private bool move = true;
    private void Start()
    {
        playerStateInfo = UiManager.instance.playerStateInfo;
    }
    public void GetItem()
    {
        playerStateInfo.secondGunDamage = damage;
        playerStateInfo.secondGunName = gunName;
        playerStateInfo.TotalNumberOfBullets[0] += 20;
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

