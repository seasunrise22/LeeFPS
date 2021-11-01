using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateGenerator : MonoBehaviour
{
    public static CrateGenerator instance;

    public GameObject[] setCrates; // 상자 프리팹을 넣을 배열 변수
    public List<GameObject> existCrates; // 생성 된 상자를 넣을 리스트. 크기를 정하지 않을 것이므로 List 사용.

    float nextGenTime;
    public float genRate = 4f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 CrateGenerator가 존재합니다!");
            Destroy(gameObject);
        }    
    }

    private void Start()
    {
        existCrates = new List<GameObject>(); 
    }    

    private void FixedUpdate()
    {      
        if(Time.time > nextGenTime)
        {
            GenerateCrate();
            nextGenTime = Time.time + genRate;
        }   
    }

    void GenerateCrate()
    {
        // 위치 랜덤 생성
        Vector3 randomPos = new Vector3(
            Random.Range(-8f, 15f), // minX, maxX
            Random.Range(25f, 30f), // minY, maxY
            Random.Range(3f, 25f)); // minZ, maxZ

        // 회전 랜덤 생성
        Vector3 randomRot = new Vector3(
            Random.Range(0f, 45f), // minX, maxX
            Random.Range(0f, 45f), // minY, maxY
            Random.Range(0f, 45f)); // minZ, maxZ

        existCrates.Add(Instantiate(setCrates[Random.Range(0, setCrates.Length)], randomPos, Quaternion.Euler(randomRot)));
    }
}
