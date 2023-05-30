using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.gm.bossOn = true;
            other.transform.position = new Vector3(0, 0, -680);
        }
    }
}
