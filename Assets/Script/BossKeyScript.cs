using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKeyScript : MonoBehaviour
{
    private float MaxHp = 101f;
    private float Hp;
    private Renderer color;
    public GameObject bossGo;

    private void Start()
    {
        Hp = MaxHp;
        color = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "PlayerBullet")
        {
            Hp -= 20f;

            float hpColor = Hp / MaxHp;

            if (hpColor <= 0.2f)
            {
                color.material.color = new Color(255 / 255f, 0, 0);
            }
            else if (hpColor <= 0.5f)
            {
                color.material.color = new Color(255 / 255f, 133 / 255f, 133 / 255f);
            }
            else if (hpColor <= 0.7f)
            {
                color.material.color = new Color(255 / 255f, 213 / 255f, 213 / 255f);
            }

            if(Hp < 1)
            {
                Destroy(GameObject.Find("BossWall"));
                UiManager.instance.SetBossRoomUI();
                Instantiate(bossGo, transform.position+Vector3.up, transform.rotation);
                Destroy(gameObject);
            }

       }
    }
}

