using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public PlayerStateInfo playerStateInfo;

    private float GameTime;

    // UI
    public GameObject EscapeButton;
    public Text GameTimeText;
    public Text PlayerHp;
    public Text SetGun;
    public Text DamageUI;
    public Text SpeedUI;
    public Text NumberOfBullet;
    public Text BossRoomUI;
    public GameObject gunImage;
    private AudioSource ButtonAudio;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        ButtonAudio = transform.Find("ButtonAudio").gameObject.GetComponent<AudioSource>();
        BossRoomUI.text = "보스방 상태 : 봉인";
        EscapeButton.SetActive(false); // esc버튼창 닫기
        Time.timeScale = 1f; // 게임 진행 속도 초기화(다시시작 시 필요)
        GameTime = 0; // 게임 시간 초기화
        SetBulletCount();

    }

    private void Update()
    {
        if (GameManager.gm.GameOver)
            return;

        GameTime += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscOn();
        }
    }
    private void LateUpdate()
    {
        if (GameManager.gm.GameOver)
            return;
        GameTimeText.text = "Time : " + (int)GameTime;
    }

    public void EscOn()
    {
        ButtonAudio.Play();
        EscapeButton.SetActive(!EscapeButton.activeSelf);
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }
    public void SetStateText()
    {
        SetGun.text = "착용 중 : " + playerStateInfo.Gun;
        PlayerHp.text = ((int)playerStateInfo.HP).ToString();
        DamageUI.text = playerStateInfo.Damage.ToString();
        SpeedUI.text = playerStateInfo.Speed.ToString();


        int count = gunImage.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            if (gunImage.transform.GetChild(i).name == playerStateInfo.Gun)
                gunImage.transform.GetChild(i).gameObject.SetActive(true);
            else
                gunImage.transform.GetChild(i).gameObject.SetActive(false);
        }

    }
    public void SetBossRoomUI()
    {
       // BossRoomUI.text = "보스방 상태 : 봉인 해제";
    }
    public void SetBulletCount()
    {
        int num;

        if (playerStateInfo.Gun == "Dotted Gun")
            num = 0;
        else if (playerStateInfo.Gun == "Automatic Gun")
            num = 1;
        else if(playerStateInfo.Gun == "Shotgun")
            num = 2;
        else
        {
            NumberOfBullet.text = "남은 탄창 : ∞";
            return;
        }


        NumberOfBullet.text = "남은 탄창 : " + playerStateInfo.NumberOfBullets[num] + " / " + playerStateInfo.TotalNumberOfBullets[num];
    }


    public void OnClickBack()
    {
        if (GameManager.gm.GameOver)
            return;
        ButtonAudio.Play();
        EscapeButton.SetActive(!EscapeButton.activeSelf);
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }
    public void OnClickReset()
    {
        ButtonAudio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnClickExit()
    {
        ButtonAudio.Play();
        Application.Quit();
    }
}
