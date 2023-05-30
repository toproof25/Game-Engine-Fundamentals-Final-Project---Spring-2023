using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{
    private BossScript bossScript;
    private BossStateInfo bossState;

    void Start()
    {
        bossScript = GameObject.Find("Red").GetComponent<BossScript>();
        bossState = bossScript.bossState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            ContactPoint contact = collision.contacts[0];     // 몬스터 부위
            GameObject collidedObject = collision.gameObject; // 총알
            float hitDamage = collidedObject.GetComponent<bullet>().damage;

            if (contact.thisCollider.CompareTag("BossHead"))
            {
                Debug.Log("머리 히트");
                bossState.HP -= hitDamage;
            }
            else if (contact.thisCollider.CompareTag("LeftShoulder"))
            {
                Debug.Log("몸통 / 왼쪽어깨 히트");
                bossState.HP -= hitDamage;
                bossState.LeftShoulderHP -= hitDamage;
                if (bossState.LeftShoulderHP < 0)
                {
                    GameObject shoulderObj = GameObject.Find("L_Shoulder01");
                    Vector3 scale = shoulderObj.transform.localScale;
                    scale.x = 0f;
                    shoulderObj.transform.localScale = scale;
                }
            }
            else if (contact.thisCollider.CompareTag("RightShoulder"))
            {
                Debug.Log("몸통 / 오른쪽 어깨 히트");
                bossState.HP -= hitDamage;
                bossState.RightShoulderHP -= hitDamage;
                if (bossState.RightShoulderHP < 0)
                {
                    GameObject shoulderObj = GameObject.Find("R_Shoulder01");
                    Vector3 scale = shoulderObj.transform.localScale;
                    scale.x = 0f;
                    shoulderObj.transform.localScale = scale;
                }
            }
            else if (contact.thisCollider.CompareTag("BossBody"))
            {
                Debug.Log("몸통 히트");
                bossState.HP -= hitDamage;
            }

            Debug.Log("보스 피 : "+bossState.HP);
            // 충돌한 총알 제거
            Destroy(collidedObject);

            // 보스의 HP 체크
            if (bossState.HP <= 0)
            {
                //bossScript.Die();
            }

        }
    }
}
