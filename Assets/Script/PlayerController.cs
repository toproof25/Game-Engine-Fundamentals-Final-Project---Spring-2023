using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Reloading,
        Running,
        Walking,
        Aiming,
        Shooting,
        Idle
    }
    public State state { get; set; }

    public PlayerStateInfo playerStateInfo;
    public GameObject PlayerBulletPrefab;
    public Transform shotPosition;
    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody rd;

    private float wait;
    private int[] NumberBullet = { 20, 30, 10 };
    private float speed;

    public AudioClip shotgunClip;
    public AudioClip gunClip;

    public AudioClip reroadClip1;
    public AudioClip reroadClip2;


    void Start()
    {
        rd = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        wait = 1f;
        state = State.Walking;
        speed = playerStateInfo.Speed;

        shotPosition.transform.SetParent(transform.Find("AssaultRifle"), false);
    }

    void Update()
    {
        if (GameManager.gm.GameOver)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        animator.SetBool("Horizontal", x == 0 ? false : true);
        animator.SetBool("Vertical", y == 0 ? false : true);

        rd.velocity = new Vector3(x * speed, 0, y * speed);

        float moveValue = Mathf.Clamp(Mathf.Abs(x) + Mathf.Abs(y), -1, 1);
        animator.SetFloat("Move", moveValue, 0.1f, Time.deltaTime);

        if (x == 0 && y == 0)
        {
            animator.SetBool("IsMove", false);
            state = State.Idle;
        }
        else if(x != 0 || y != 0)
        {
            animator.SetBool("IsMove", true);
            state = State.Walking;
        }

        MouseDir();

        if (Input.GetKeyDown(KeyCode.R) && state!=State.Reloading)
        {
            Reroding();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) )
        {
            state = State.Running;
            animator.SetBool("Run", true);
            speed = playerStateInfo.Speed * 1.5f;
            return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            state = State.Walking;
            animator.SetBool("Run", false);
            speed = playerStateInfo.Speed;
        }

        if (Input.GetMouseButton(1))
        {
            state = State.Aiming;
            animator.SetBool("Run", false);
            animator.SetBool("Shoot", true);
            speed = playerStateInfo.Speed * 0.3f;
            playerStateInfo.accuracy = 0;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            state = State.Walking;
            animator.SetBool("Shoot", false);
            speed = playerStateInfo.Speed;
            playerStateInfo.accuracy = 15;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(1) && state == State.Walking)
        {
            animator.SetBool("Run", false);
            speed = playerStateInfo.Speed;
        }

        if (state == State.Reloading)
            return;

        wait -= Time.deltaTime;
        

        if (Input.GetMouseButton(0) && state!=State.Running)
        {
            ShootBullet();
        }

        

    }

    private void MouseDir()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y; // 여기서 거리값은 높이와 비슷한 값으로 설정하는 것이 적합합니다.
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 lookDirection = new Vector3(worldMousePosition.x, transform.position.y, worldMousePosition.z) - transform.position;
        transform.forward = lookDirection;

        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 Dir = (new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position).normalized;
            transform.forward = Dir;
        }
        */
    }

    private void Reroding()
    {
        int num;
        if (playerStateInfo.Gun == "Dotted Gun")
            num = 0;
        else if (playerStateInfo.Gun == "Automatic Gun")
            num = 1;
        else if (playerStateInfo.Gun == "Shotgun")
            num = 2;
        else
            return;

        audioSource.clip = reroadClip1;
        audioSource.Play();

        if (playerStateInfo.TotalNumberOfBullets[num] == 0) // 여분 탄알이 없으면 리턴
            return;
        else if(playerStateInfo.NumberOfBullets[num] == NumberBullet[num])
            return;
        

        IEnumerator ReloadComplete()
        {
            state = State.Reloading;
            animator.SetTrigger("Reload");

            if (playerStateInfo.TotalNumberOfBullets[num] + playerStateInfo.NumberOfBullets[num] <= NumberBullet[num]) // 사용중 탄알과 여분 탄알의 합이 최대 탄창이 아니라면 
            {
                playerStateInfo.NumberOfBullets[num] += playerStateInfo.TotalNumberOfBullets[num];
                playerStateInfo.TotalNumberOfBullets[num] = 0;
            }
            else
            {
                int bulletNum = NumberBullet[num] - playerStateInfo.NumberOfBullets[num];
                playerStateInfo.TotalNumberOfBullets[num] -= bulletNum;
                playerStateInfo.NumberOfBullets[num] += bulletNum;
            }
            Debug.Log("재장전 중......");
            yield return new WaitForSeconds(2.2f);
            Debug.Log("재장전 완료......");
            audioSource.clip = reroadClip2;
            audioSource.Play();
            state = State.Shooting;
            wait = 0;
            UiManager.instance.SetBulletCount();
        }
        StartCoroutine(ReloadComplete());

    }
    private void ShootBullet()
    {
        if (wait > 0)
            return;

        if (playerStateInfo.Gun == "BasicGun")
        {
            wait = 1f;
            BasicGun();
        }
        else if (playerStateInfo.Gun == "Dotted Gun")
        {
            wait = 1.5f;
            DottedGun();
        }
        else if (playerStateInfo.Gun == "Automatic Gun")
        {
            wait = 0.1f;
            SubmachineGun();
        }
        else if (playerStateInfo.Gun == "Shotgun")
        {
            wait = 3f;
            ShotGun();
        }
    }
    private void BasicGun()
    {
        animator.SetTrigger("Shooting");
        audioSource.clip = gunClip;
        audioSource.Play();
        GameObject bullet = Instantiate(PlayerBulletPrefab, shotPosition.position, transform.rotation);
        bullet.GetComponent<bullet>().speed = playerStateInfo.BulletSpeed;
        bullet.GetComponent<bullet>().damage = playerStateInfo.Damage;
        bullet.GetComponent<bullet>().critical = 5;
        int acc = playerStateInfo.accuracy;
        bullet.transform.Rotate(Random.Range(-acc, acc), Random.Range(-acc, acc), 0);
    }
    private void DottedGun()
    {
        
        IEnumerator ShootBurst()
        {
            for (int i = 0; i < 3; i++)
            {
                animator.SetTrigger("Shooting");
                audioSource.clip = gunClip;
                audioSource.Play();
                GameObject bullet = Instantiate(PlayerBulletPrefab, shotPosition.position, transform.rotation);
                bullet.GetComponent<bullet>().speed = playerStateInfo.BulletSpeed;
                bullet.GetComponent<bullet>().damage = playerStateInfo.Damage;
                bullet.GetComponent<bullet>().critical = 10;
                int acc = playerStateInfo.accuracy;
                bullet.transform.Rotate(Random.Range(-acc, acc), Random.Range(-acc, acc), 0);

                if (playerStateInfo.NumberOfBullets[0] == 0)
                    yield break;
                else
                {
                    playerStateInfo.NumberOfBullets[0]--;
                    UiManager.instance.SetBulletCount();
                }
                    

                Debug.Log(playerStateInfo.NumberOfBullets[0]);
                yield return new WaitForSeconds(0.2f);
            }
        }
        StartCoroutine(ShootBurst());

    }
    private void SubmachineGun()
    {
        
        if (playerStateInfo.NumberOfBullets[1] == 0)
            return;
        else
            playerStateInfo.NumberOfBullets[1]--;

        animator.SetTrigger("Shooting");
        audioSource.clip = gunClip;
        audioSource.Play();
        GameObject bullet = Instantiate(PlayerBulletPrefab, shotPosition.position, transform.rotation);
        bullet.GetComponent<bullet>().speed = playerStateInfo.BulletSpeed;
        bullet.GetComponent<bullet>().damage = playerStateInfo.Damage;
        bullet.GetComponent<bullet>().critical = 10;
        int acc = playerStateInfo.accuracy;
        bullet.transform.Rotate(Random.Range(-acc, acc), Random.Range(-acc, acc), 0);

        UiManager.instance.SetBulletCount();
    }
    private void ShotGun()
    {
        if (playerStateInfo.NumberOfBullets[2] == 0)
            return;
        else
            playerStateInfo.NumberOfBullets[2]--;

        animator.SetTrigger("Shooting");
        audioSource.clip = shotgunClip;
        audioSource.Play();
        for (int i = 0; i < 10; i++) // 10발을 발사
        {
            GameObject bullet = Instantiate(PlayerBulletPrefab, shotPosition.position, transform.rotation); // 총알 생성
            //bullet.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // 총알 크기 줄이기
            bullet.GetComponent<bullet>().speed = playerStateInfo.BulletSpeed; // 총알 속도 설정
            bullet.GetComponent<bullet>().damage = playerStateInfo.Damage * 0.7f; // 총알 데미지 설정
            bullet.GetComponent<bullet>().critical = 15; // 총알 크리티컬 설정
            int acc = playerStateInfo.accuracy;
            bullet.transform.Rotate(Random.Range(-10-acc, 10+acc), Random.Range(-7-acc, 7+acc), 0);
        }
        UiManager.instance.SetBulletCount();
    }
}


