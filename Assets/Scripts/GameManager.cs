using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int maxGauge = 100; // 최대 라이프 게이지
    public int currentGauge; // 현재 게이지 수치
    public GaugeBar gaugeBar; // 실제 라이프 게이지 오브젝트의 스크립트와 연결시킬 매개체

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
        gaugeBar.SetMaxGauge(maxGauge);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TestTakeDamage(20);
        }
    }

    void TestTakeDamage(int damage)
    {
        currentGauge += damage;
        gaugeBar.SetGauge(currentGauge);
    }

    // 상자가 생성되면 AddCrates 함수를 호출하여 UI에 현재 생성되어 있는 상자들의 총 개수를 갱신시킬 예정. 
    public void AddCrates()
    {
        cratesNum++;
        cratesText.text = "남은 상자 : " + cratesNum;
    }

    // 상자가 파괴되면 남은 상자 개수를 -1하고 UI에 갱신.
    public void DestroyCrates()
    {
        cratesNum--;
        cratesText.text = "남은 상자 : " + cratesNum;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo, int extraCurrentAmmo, int extraMaxAmmo)
    {
        if (currentAmmo < 0 || extraCurrentAmmo < 0)
            return;

        currentAmmoText.text = currentAmmo + " / " + maxAmmo;
        extraAmmoText.text = extraCurrentAmmo + " / " + extraMaxAmmo;
    }
}
