using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInTrigger : MonoBehaviour
{
    private bool IsEndRoom = false;
    private OnMonster Parent;

    private GameObject[]LockDoor;

    private void Start()
    {
        Parent = transform.parent.gameObject.GetComponent<OnMonster>();
        if (transform.parent.gameObject.transform.position == Vector3.zero)
        {
            IsEndRoom = true;
        }
        LockDoor = new GameObject[transform.childCount];
        for (int i=0; i < transform.childCount; i++)
        {
            LockDoor[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.gameObject.transform.position == Vector3.zero)
        {
            IsEndRoom = true;
        }

        if (!IsEndRoom && other.tag == "Player")
        {
            IsEndRoom = true;
            foreach(GameObject g in LockDoor)
                g.SetActive(true);
            Invoke("SpawnEnemy", 2f); // 함수를 2초뒤 실행하는 기능
            
        }
    }

    private void SpawnEnemy()
    {
        Parent.SpawnEnemy();
    }
}
