using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMonster : MonoBehaviour
{
    public GameObject monster;

    private Renderer Groud;
    private bool IsClear = true;

    public bool BossKeyRoom { get; set; }
    private void Start()
    {
        
        Groud = transform.Find("Ground").gameObject.GetComponent<Renderer>();
        if (gameObject.transform.position == Vector3.zero)
        {
            Groud.material.color = Color.white;
        }

    }

    public void SpawnEnemy()
    {
        int randomNum = Random.Range(2, 5);
        IsClear = false;

        for (int i = 1; i <= randomNum; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-15, 15), 0, Random.Range(-13, 13));
            Instantiate(monster, transform.position + randomPosition, transform.rotation, this.gameObject.transform);
            
        }
    }

    private void Update()
    {
        if (IsClear)
            return;

        if(transform.Find("SoldierEnemyPrefab(Clone)") == null)
        {
            IsClear = true;
            Groud.material.color = Color.gray;
            if (BossKeyRoom)
            {
                Instantiate(GameManager.gm.BossKey, transform.position, transform.rotation);
            }
            Destroy(transform.Find("RoomInTrigger").gameObject);
        }
    }
}
