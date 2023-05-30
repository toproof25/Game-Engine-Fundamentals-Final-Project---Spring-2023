using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : MonoBehaviour
{
    private Renderer rd;
    private float BulletDamage;
    private string GunName;
    private AudioSource audioSource;
    private bool IsTrigger = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rd = GetComponent<Renderer>();
        int ranNum = Random.Range(1, 101);

        if(ranNum < 60)
        {
            rd.material.color = Color.black;
            BulletDamage = 40f;
            GunName = "Dotted Gun";
        }
        else if(ranNum < 80)
        {
            rd.material.color = Color.red;
            BulletDamage = 20f;
            GunName = "Automatic Gun";
        }
        else if (ranNum <= 100)
        {
            rd.material.color = Color.cyan;
            BulletDamage = 10f;
            GunName = "Shotgun";
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && IsTrigger)
        {
            IsTrigger = false;
            audioSource.Play();
            gameObject.GetComponent<Renderer>().enabled = false;
            other.GetComponent<PlayerState>().ChangeGun(BulletDamage, GunName);
            Destroy(gameObject, audioSource.clip.length);
        }
    }

}
