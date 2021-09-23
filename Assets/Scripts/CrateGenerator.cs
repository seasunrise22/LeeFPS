using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateGenerator : MonoBehaviour
{
    public GameObject[] crates;  

    float nextGenTime;
    float genRate = 3f;

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

        Instantiate(crates[Random.Range(0, crates.Length)], randomPos, Quaternion.Euler(randomRot));
    }
}
