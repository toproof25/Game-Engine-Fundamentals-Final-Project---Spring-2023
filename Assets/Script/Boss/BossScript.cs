using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public enum State
    {
        idle,
        hornattack,
        move,
        die,
        stern
    }
    public State state;
    
    public BossStateInfo bossState;
    private Animator animator;
    private GameObject target;
    private PlayerState targetState;
    private Rigidbody rd;

    private bool left = false;
    private bool right = false;
    private bool isStern = false;

    private float skillTimer = 5f;
    void Start()
    {
        rd = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
        bossState.HP = 1000f;
        bossState.LeftShoulderHP = 200f;
        bossState.RightShoulderHP = 200f;
        target = GameObject.Find("Player");
        targetState = target.GetComponent<PlayerState>();
        animator.SetBool("Move", false);
        state = State.idle;
    }

    void Update()
    {
        if (state == State.die || state == State.stern || !GameManager.gm.bossOn) //|| !GameManager.gm.bossOn
            return;

        skillTimer -= Time.deltaTime;
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (skillTimer < 0 && distance < 7f)
        { 
            StartCoroutine(HornAttack());
        }
        else if (state == State.idle || state == State.move)
        {
            Move();
        }
    }

    IEnumerator HornAttack()
    {
        transform.LookAt(target.transform);
        animator.SetTrigger("HornAttack");
        animator.SetBool("Move", false);
        state = State.hornattack;
        yield return new WaitForSeconds(3f);
        state = State.idle;
        skillTimer = 10f;
    }


    private void Move()
    {
        state = State.move;
        animator.SetBool("Move", true);
        transform.LookAt(target.transform);
        rd.velocity = transform.forward * bossState.Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 공격 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == State.move)
            {
                targetState.Hit(10f);
            }
            if (state == State.hornattack)
            {
                targetState.Hit(60f);
            }
        }

            // 히트 처리
            if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            ContactPoint contact = collision.contacts[0];     // 몬스터 부위
            GameObject collidedObject = collision.gameObject; // 총알
            float hitDamage = collidedObject.GetComponent<bullet>().damage;

            if (contact.thisCollider.CompareTag("BossHead"))
            {
                Debug.Log("머리 히트");
                bossState.HP -= hitDamage+20;
            }
            else if (contact.thisCollider.CompareTag("LeftShoulder"))
            {
                Debug.Log("몸통 / 왼쪽 어깨 히트");
                bossState.HP -= hitDamage;
                bossState.LeftShoulderHP -= hitDamage;
                if (bossState.LeftShoulderHP < 0)
                {
                    GameObject shoulderObj = GameObject.Find("L_Shoulder01");
                    Vector3 scale = shoulderObj.transform.localScale;
                    scale.x = 0f;
                    shoulderObj.transform.localScale = scale;
                    left = true;
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
                    right = true;
                }
            }
            else if (contact.thisCollider.CompareTag("BossBody"))
            {
                Debug.Log("몸통 히트");
                bossState.HP -= hitDamage;
            }

            if (left && right && !isStern)
            {
                IEnumerator stern()
                {
                    isStern = true;
                    state = State.stern;
                    animator.SetBool("Stern", true);
                    yield return new WaitForSeconds(3f);
                    state = State.idle;
                    animator.SetBool("Stern", false);
                }
                StartCoroutine(stern());
            }

            Debug.Log("보스 피 : " + bossState.HP);
            Destroy(collidedObject);

            // 보스의 HP 체크
            if (bossState.HP <= 0 && state != State.die)
            {
                state = State.die;
                animator.SetTrigger("Die");
                GameManager.gm.Die();
            }

        }
    }
}
