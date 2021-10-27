using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지가 변경될 때 변경될 사항들을 모두 모아 놓은 레벨 매니저.
// 게임 매니저에서 스테이지가 변경되어야 한다고 판단하면 이 오브젝트를 불러와서 레벨 조정후 스테이지를 다시 세팅한다.
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

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

    public void SetStage(bool isLvUp)
    {
        if (isLvUp)
        {
            Debug.Log("스테이지 이동");
        }       
    }
}
