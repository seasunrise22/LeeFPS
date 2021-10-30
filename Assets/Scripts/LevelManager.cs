using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 난이도에 맞는 스테이지 세팅.
// UI 오브젝트 자체는 GameManager에 연결되어 있다. GameManager를 경유하여 UI를 갱신할 것.
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // for 싱글턴
    public int stageNum; // 현재 스테이지 넘버를 저장할 변수

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 레벨 매니저가 존재합니다!");
            Destroy(gameObject);
        }   
    }

    private void Start()
    {
        SetStage(true); // 게임이 시작되면 스테이지는 1로 초기화하여 UI에 갱신.
    }

    public void SetStage(bool isLvUp)
    {
        if (isLvUp)
        {
            // 스테이지 UI 갱신
            stageNum++;
            GameManager.instance.stageText.text = "Stage : " + stageNum;

            // 상자 떨어지는 간격 조정
            CrateGenerator.instance.genRate = CrateGenerator.instance.genRate / 1.2f;
        }       
    }
}
