using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 난이도에 맞는 스테이지 세팅.
// UI 오브젝트 자체는 GameManager에 연결되어 있다. GameManager를 경유하여 UI를 갱신할 것.
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // for 싱글턴

    public GameObject interTimerObj; // 스테이지 사이 타이머 표시할 게임 오브젝트
    public Text interTimerText; // 스테이지 넘어갈 때 표시할 타이머
    float interTime; // 타이머에 표시할 남은 시간    

    public int stageNum; // 현재 스테이지 넘버를 저장할 변수
    public Text stageText; // 현재 스테이지를 나타낼 텍스트UI 오브젝트
    
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
        interTime = 3f;

        stageNum++; 
        stageText.text = "Stage : " + stageNum;
    }

    public void SetStage(bool isLvUp)
    {
        if (isLvUp)
        {
            // 스테이지가 바뀌면 기존에 존재하던 상자는 모두 파괴.
            for (int i = 0; i < CrateGenerator.instance.existCrates.Count; i++)
                CrateGenerator.instance.existCrates[i].GetComponent<Target>().Die();

            // 타이머 표시
            StartCoroutine("interTimerCoroutine");
        }       
    }

    // 스테이지 넘어갈 때 표시할 타이머. 3초.
    IEnumerator interTimerCoroutine()
    {
        CrateGenerator.instance.isGen = false; // 상자 생성 중단.

        interTimerObj.SetActive(true);

        interTimerText.text = (interTime).ToString("0"); // 3
        yield return new WaitForSeconds(1f);

        interTime--;
        interTimerText.text = (interTime).ToString("0"); // 2
        yield return new WaitForSeconds(1f);

        interTime--;
        interTimerText.text = (interTime).ToString("0"); // 1
        yield return new WaitForSeconds(1f);

        interTimerObj.SetActive(false);
        interTime = 3f;
        
        CrateGenerator.instance.genRate = CrateGenerator.instance.genRate / 1.2f; // 상자 떨어지는 간격 조정
        CrateGenerator.instance.Start(); // isGen = true로 해서 상자 생성 재개, existCrates 리스트 초기화.

        // 게임 시간 다시 흐르도록.
        GameManager.instance.timeLeft = 10f;
        GameManager.instance.isLvUp = false;

        // 스테이지 UI 갱신.
        stageNum++;
        stageText.text = "Stage : " + stageNum;
    }
}
