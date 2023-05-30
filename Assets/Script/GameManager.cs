using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject[] RoomPrefabs;
    private List<GameObject> Room = new List<GameObject>();
    public GameObject BossKey;
    private int roomNum = 15;

    public bool bossOn = false;
    public bool GameOver = false;
    private void Awake()
    {
        gm = this;
    }
    void Start()
    {
        CreativeRoom();
    }

    public void Die()
    {
        GameOver = true;
        IEnumerator DelayDie()
        {
            yield return new WaitForSeconds(1.1f);
            Time.timeScale = 0f;
            UiManager.instance.EscOn();
        }
        StartCoroutine(DelayDie());
    }

    private void CreativeRoom()
    {
        Room.Add(Instantiate(RoomPrefabs[Random.Range(0, 3)], transform.position, transform.rotation));

        for (int i = 1; i < roomNum; i++)
        {
            Vector3 roomPosition;
            GameObject ranRoom = null;
            List<GameObject> matchingRooms = null;

            switch (Random.Range(0, 3))
            {
                case 0: //  가로방
                    matchingRooms = Room.FindAll(room => room.CompareTag("HorizontalRoom") || room.CompareTag("FourWayRoom"));
                    try
                    {
                        ranRoom = matchingRooms[Random.Range(0, matchingRooms.Count)];
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        i--;
                        continue;
                    }

                    roomPosition = ranRoom.transform.position + (Random.Range(0, 2) > 0 ? new Vector3(-40, 0, 0) : new Vector3(40, 0, 0));

                    if (OverlapRoom(roomPosition))
                    {
                        i--;
                        continue;
                    }
                    else
                    {
                        Room.Add(Instantiate(RoomPrefabs[Random.Range(0, 10) >= 2 ? 0 : 2], roomPosition, transform.rotation));
                    }
                    break;

                case 1: // 세로방

                    // 세로방 or 4방 랜덤
                    matchingRooms = Room.FindAll(room => room.CompareTag("VerticalRoom") || room.CompareTag("FourWayRoom"));
                    try
                    {
                        ranRoom = matchingRooms[Random.Range(0, matchingRooms.Count)];
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        i--;
                        continue;
                    }

                    roomPosition = ranRoom.transform.position + (Random.Range(0, 2) > 0 ? new Vector3(0, 0, 35) : new Vector3(0, 0, -35));

                    if (OverlapRoom(roomPosition))
                    {
                        i--;
                        continue;
                    }
                    else
                    {
                        Room.Add(Instantiate(RoomPrefabs[Random.Range(0, 10) >= 2 ? 1 : 2], roomPosition, transform.rotation));
                    }
                    break;

                case 2: // 4방향 방

                    ranRoom = Room[Random.Range(0, Room.Count)];
                    int ranNum;

                    if (ranRoom.tag == "HorizontalRoom")
                    {
                        ranNum = 0;
                        roomPosition = ranRoom.transform.position + (Random.Range(0, 2) > 0 ? new Vector3(-40, 0, 0) : new Vector3(40, 0, 0));
                    }
                    else if (ranRoom.tag == "VerticalRoom")
                    {
                        ranNum = 1;
                        roomPosition = ranRoom.transform.position + (Random.Range(0, 2) > 0 ? new Vector3(0, 0, 35) : new Vector3(0, 0, -35));
                    }
                    else
                    {
                        ranNum = 2;
                        int rn = Random.Range(0, 4); // 0 왼쪽  1 오른쪽  2 위  3 아래
                        if (rn == 0)
                        {
                            roomPosition = ranRoom.transform.position + new Vector3(-40, 0, 0);
                        }
                        else if (rn == 1)
                        {
                            roomPosition = ranRoom.transform.position + new Vector3(40, 0, 0);
                        }
                        else if (rn == 2)
                        {
                            roomPosition = ranRoom.transform.position + new Vector3(0, 0, 35);
                        }
                        else
                        {
                            roomPosition = ranRoom.transform.position + new Vector3(0, 0, -35);
                        }
                    }



                    if (OverlapRoom(roomPosition))
                    {
                        i--;
                        continue;
                    }
                    else
                    {
                        Room.Add(Instantiate(RoomPrefabs[ranNum], roomPosition, transform.rotation));
                    }
                    break;

            }
        }

        Room[Random.Range(5, Room.Count)].GetComponent<OnMonster>().BossKeyRoom = true;
        LockDoor();
    }
    private bool OverlapRoom(Vector3 pos)
    {
        foreach (GameObject r in Room)
        {
            if (Vector3.Distance(r.transform.position, pos) < 10f)
            {
                return true;
            }
        }
        return false;
    }

    private void LockDoor()
    {
        
        foreach(GameObject r in Room)
        {
            if(r.tag == "HorizontalRoom")
            {
                if(!LockRoomCheck(r.transform.position + new Vector3(-40, 0, 0), 0))
                {
                    r.transform.Find("StageWall").Find("LeftWall").gameObject.SetActive(true);
                }
                if (!LockRoomCheck(r.transform.position + new Vector3(40, 0, 0), 0))
                {
                    r.transform.Find("StageWall").Find("RightWall").gameObject.SetActive(true);
                }
            }
            else if (r.tag == "VerticalRoom")
            {
                if (!LockRoomCheck(r.transform.position + new Vector3(0, 0, 35), 1))
                {
                    r.transform.Find("StageWall").Find("UpWall").gameObject.SetActive(true);
                }
                if (!LockRoomCheck(r.transform.position + new Vector3(0, 0, -35), 1))
                {
                    r.transform.Find("StageWall").Find("DownWall").gameObject.SetActive(true);
                }
            }
            else
            {
                if (!LockRoomCheck(r.transform.position + new Vector3(-40, 0, 0), 0))
                {
                    r.transform.Find("StageWall").Find("LeftWall").gameObject.SetActive(true);
                }
                if (!LockRoomCheck(r.transform.position + new Vector3(40, 0, 0), 0))
                {
                    r.transform.Find("StageWall").Find("RightWall").gameObject.SetActive(true);
                }
                if (!LockRoomCheck(r.transform.position + new Vector3(0, 0, 35), 1))
                {
                    r.transform.Find("StageWall").Find("UpWall").gameObject.SetActive(true);
                }
                if (!LockRoomCheck(r.transform.position + new Vector3(0, 0, -35), 1))
                {
                    r.transform.Find("StageWall").Find("DownWall").gameObject.SetActive(true);
                }
            }
        }
    }
    private bool LockRoomCheck(Vector3 pos, int position)
    {
        foreach (GameObject r in Room)
        {
            if (Vector3.Distance(r.transform.position, pos) < 10f)
            {
                if (position == 0 && r.tag == "VerticalRoom")
                    return false;
                else if (position == 1 && r.tag == "HorizontalRoom")
                    return false;

                return true;
            }         

        }
        return false;
    }
}

