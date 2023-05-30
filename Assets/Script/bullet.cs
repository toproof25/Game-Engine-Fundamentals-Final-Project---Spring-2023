using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float speed { get; set; }
    public float damage { get; set; }
    public int critical { get; set; }

    private Rigidbody rd;
    
    void Start()
    {
        if (Random.Range(0, 100) + 1 <= critical)
        {
            damage = (float)(damage * 1.5);
            GetComponent<Renderer>().material.color = Color.red;
        }

        rd = GetComponent<Rigidbody>();
        rd.velocity = transform.forward * speed;
        Destroy(gameObject, 20f);

        
    }
    private void Update()
    {
        if (GameManager.gm.GameOver)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if(gameObject.tag == "EnemyBullet")
        {
            if(other.tag == "Player")
            {
                other.GetComponent<PlayerState>().Hit(damage);
                Destroy(gameObject);
            }
        }
        else if (gameObject.tag == "PlayerBullet")
        {
            if (other.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }
        else if (other.tag == "LockDoor")
        {
            Destroy(gameObject);
        }
    }

}
