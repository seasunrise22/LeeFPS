using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 싱글턴으로 구현.
// 각종 UI 제어(남은 상자, 총알 개수, 라이프 게이지)
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글턴을 할당할 전역 변수

    private int cratesNum; // 필드에 생성되어 있는 상자의 개수를 카운트할 변수.
    public Text cratesText; // 현재 필드에 생성되어 있는 상자의 개수 표시할 변수.

    public Text currentAmmoText; // 현재 남은 총알 개수를 표시할 UI 텍스트
    public Text extraAmmoText; // 여분의 총알 개수를 표시할 UI 텍스트

    public int maxGauge; // 최대 라이프 게이지
    public int currentGauge; // 현재 게이지 수치
    public GaugeBar gaugeBar; // 실제 라이프 게이지 오브젝트의 스크립트와 연결시킬 매개체

    public GameObject gameoverText; // 게임오버시 활성화 시킬 텍스트 UI 오브젝트
    
    public Text bestStage; // 최고 스테이지를 나타낼 UI 오브젝트

    public bool isGameover; // 게임오버 상태인지 아닌지 체크용

    public Text timerText; // 타이머 표시용 UI 오브젝트
    public float timeLeft; // 남은 시간

    public bool isLvUp; // 스테이지 전환용 트리거

    // 게임 시작과 동시에 싱글턴을 구성
    private void Awake()
    {
        // 싱글턴 변수 instance가 비어 있는가?
        if(instance == null)
        {
            // instance가 비어 있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우
            // 씬에 두 개 이상의 GameManager 오브젝트가 존재한다는 의미
            // 싱글턴 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴.
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentGauge = 0; // 시작과 동시에 게이지 0으로 초기화
        maxGauge = 1000;
        gaugeBar.SetMaxGauge(maxGauge);

        //currentStage = 1; // 게임 처음 시작하면 현재 스테이지 1로 초기화
        //SetStage(currentStage);

        // 격겜 만들 때 처럼 ToString("0")으로 소수점 아래는 버리는 식으로 만들면 반올림한 숫자로 표시돼서
        // 이번에도 시작을 30이 아닌 30.5로 해야할 줄 알았는데 0.0으로 소수점 아래까지 표현하니까 정상적으로 30초 부터 되는 듯?
        timeLeft = 3f; 
    }

    private void Update()
    {
        // 타이머 감소 및 다음 스테이지로의 전개. LevelManager.cs와 연결.
        if (!isLvUp)
        {
            timerText.text = (timeLeft).ToString("0.00");
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                isLvUp = true; // 타이머가 0이 되면 if문이 더 이상 동작하지 않게 하기 위해.
                timeLeft = 0f;
                LevelManager.instance.SetStage(isLvUp);
            }
        }

        // 게임오버 트리거가 발동됐을 경우 아래 작업 수행
        if (isGameover)
        {
            gameoverText.SetActive(true);

            // 최고점수 가져오기
            if (PlayerPrefs.GetInt("bestScore") < LevelManager.instance.stageNum)
                PlayerPrefs.SetInt("bestScore", LevelManager.instance.stageNum);
            bestStage.text = "Best Stage : " + PlayerPrefs.GetInt("bestScore");

            Time.timeScale = 0;

            // R키를 눌러서 씬을 다시 불러오라 = 게임 다시 시작
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Game");
                Time.timeScale = 1;
            }
        }
    }

    // 기능 - 현재 게이지의 양 전달 및 게이지UI 갱신, 게임오버 트리거 발동. GaugeBar.cs와 연결. 
    public void AddGauge(int amount)
    {
        currentGauge += amount;

        // 현재 게이지바의 수치가 100을 넘겼다면 게임오버 텍스트를 활성화하라
        if (currentGauge >= maxGauge)
        {
            isGameover = true;
        }
        gaugeBar.SetGauge(currentGauge);
    }

    // UI - 남은 상자 추가
    /*public void AddCrates()
    {
        cratesNum++;
        cratesText.text = "남은 상자 : " + cratesNum;
    }*/

    // UI - 남은 상자 감소
    /*public void DestroyCrates()
    {
        cratesNum--;
        cratesText.text = "남은 상자 : " + cratesNum;
    }*/

    // UI - 총알
    public void UpdateAmmo(int currentAmmo, int maxAmmo, int extraCurrentAmmo, int extraMaxAmmo)
    {
        if (currentAmmo < 0 || extraCurrentAmmo < 0)
            return;

        currentAmmoText.text = currentAmmo + " / " + maxAmmo;
        extraAmmoText.text = extraCurrentAmmo + " / " + extraMaxAmmo;
    }
}
