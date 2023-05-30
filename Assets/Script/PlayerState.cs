using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public PlayerStateInfo playerStateInfo;

    private Rigidbody rd;
    private float BeforeGunDamage = 0;
    private Animator animator;

    private AudioSource BuffAudio;
    private AudioSource GunAudio;
    private AudioSource DieAudio;
    private void Start()
    {
        setState();
        rd = GetComponent<Rigidbody>();
        BuffAudio = transform.Find("BuffAudio").gameObject.GetComponent<AudioSource>();
        GunAudio = transform.Find("GunAudio").gameObject.GetComponent<AudioSource>();
        DieAudio = transform.Find("DieAudio").gameObject.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        UiManager.instance.SetStateText();
        ChangeGun(10f, "BasicGun");
    }

    private void Update()
    {
        if (GameManager.gm.GameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeGun(10f, "BasicGun");
            playerStateInfo.OnGun = 0;
        }
        else if (playerStateInfo.secondGunName != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeGun(playerStateInfo.secondGunDamage, playerStateInfo.secondGunName);
            playerStateInfo.OnGun = 1;
        }
    }

    private void setState()
    {
        playerStateInfo.MaxHP = 100f;
        playerStateInfo.HP = playerStateInfo.MaxHP;
        playerStateInfo.Speed = 5f;
        playerStateInfo.BulletSpeed = 15f;
        playerStateInfo.Damage = 10f;
        playerStateInfo.Gun = null;
        playerStateInfo.OnGun = 0;
        playerStateInfo.accuracy = 15;
        playerStateInfo.secondGunDamage = 0;
        playerStateInfo.secondGunName = null;

        playerStateInfo.TotalNumberOfBullets[0] = 330;
        playerStateInfo.TotalNumberOfBullets[1] = 30;
        playerStateInfo.TotalNumberOfBullets[2] = 330;
        playerStateInfo.NumberOfBullets[0] = 20;
        playerStateInfo.NumberOfBullets[1] = 30;
        playerStateInfo.NumberOfBullets[2] = 10;
    }
    public void Hit(float damage)
    {
        playerStateInfo.HP -= damage;

        if (playerStateInfo.HP < 1)
        {
            animator.SetTrigger("Die");
            DieAudio.Play();
            GameManager.gm.Die();
        }

        UiManager.instance.SetStateText();
    }

    public void ChangeGun(float BulletDamage, string GunName)
    {
        playerStateInfo.Damage -= BeforeGunDamage;
        playerStateInfo.Damage += BulletDamage;
        BeforeGunDamage = BulletDamage;
        playerStateInfo.Gun = GunName;

        if(GunName != "BasicGun")
        {
            playerStateInfo.secondGunDamage = BulletDamage;
            playerStateInfo.secondGunName = GunName;
        }

        UiManager.instance.SetBulletCount();
        UiManager.instance.SetStateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        IGetGun gunItem = other.GetComponent<IGetGun>();
        if (gunItem != null)
        {
            gunItem.GetItem();
            GunAudio.Play();
            ChangeGun(playerStateInfo.secondGunDamage, playerStateInfo.secondGunName);
        }

        IGetItem buffItem = other.GetComponent<IGetItem>();
        if (buffItem != null)
        {
            buffItem.GetBuff();
            BuffAudio.Play();
            UiManager.instance.SetStateText();
        }


    }

    public void HornAttackHit()
    {
        Hit(60f);
    }

}
