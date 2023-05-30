using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;

    private Animator animator;
    private AudioSource audiosource;
    private Transform target;

    private enum State {
        idle,
        moving,
        shot
    }
    private State state {get; set;}

    private float spawnRateMin = 0.5f;
    private float spawnRateMax = 3f;
    private float spawnRate;
    private float timer=0f;
    private int bulletCount = 1;
    private float bulletSpeed = 20f;
    private float bulletDamage = 10f;
    private float MaxHP = 200;
    private float Hp;

    private GameObject parentObject;

    public GameObject[] ItemPrafeb;
    public GameObject[] GunPrafeb;


    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        target = FindObjectOfType<PlayerController>().transform;

        bulletSpeed = Random.Range(10f, 20f);
        parentObject = transform.parent.gameObject;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        bulletCount = Random.Range(0, 100) < 30 ? 3 : 1;
        Hp = MaxHP;
    }
    

    
    void Update()
    {
        if (GameManager.gm.GameOver)
            return;

        if (timer == 0)
        {
            state = State.idle;
        }  

        timer += Time.deltaTime;
        if (timer >= spawnRate && state == State.idle)
        {
            if(Random.value > 0.4f)
            {
                SpawnBullet();
            }
                
            else
            {
                StartCoroutine(IdleANdMove());
            }

        }

    }

    IEnumerator IdleANdMove()
    {
        if (Random.value > 0.5f)
        {
            Vector3 randomPosition = parentObject.transform.position + new Vector3(Random.Range(-15, 15), 0, Random.Range(-13, 13));
            state = State.moving;
            animator.SetBool("move", true);
            while (Vector3.Distance(transform.position, randomPosition) > 0.5f)
            {      
                transform.position = Vector3.MoveTowards(transform.position, randomPosition, 2 * Time.deltaTime);
                yield return null;
            }
        }

        animator.SetBool("move", false);
        yield return new WaitForSeconds(2);
        timer = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
    }

    private void LateUpdate()
    {

        transform.LookAt(target.position);
    }

    private void SpawnBullet()
    {
        state = State.shot;
        timer = 0f;
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position+Vector3.up, transform.rotation);
            bullet.transform.LookAt(target.position);
            bullet.GetComponent<bullet>().speed = bulletSpeed;
            bullet.GetComponent<bullet>().damage = bulletDamage;
            bullet.transform.Rotate(0f, (i - (bulletCount / 2)) * 10f, 0f);
            animator.SetTrigger("EnemyShot");
            audiosource.Play();
        }

        spawnRate = Random.Range(spawnRateMin, spawnRateMax);

    }
  

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBullet")
        {
            float damage = other.GetComponent<bullet>().damage;
            Hp -= damage;
            

        }

        if(Hp < 1)
        {
            float randomValue = Random.value; // 0.0 ~ 1.0 사이의 난수

            if (randomValue <= 0.55f)
            {
                Instantiate(GunPrafeb[Random.Range(0, GunPrafeb.Length)], transform.position+Vector3.right, transform.rotation);
            }
            else if (randomValue <= 0.90f)
            {
                Instantiate(ItemPrafeb[Random.Range(0, GunPrafeb.Length)], transform.position + Vector3.left, transform.rotation);
            }

            Destroy(gameObject);
        }
    }


}
