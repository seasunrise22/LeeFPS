using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateGenerator : MonoBehaviour
{
    public static CrateGenerator instance;

    public GameObject[] setCrates; // ���� �������� ���� �迭 ����
    public List<GameObject> existCrates; // ���� �� ���ڸ� ���� ����Ʈ. ũ�⸦ ������ ���� ���̹Ƿ� List ���.

    float nextGenTime;
    public float genRate = 4f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("���� �� �� �̻��� CrateGenerator�� �����մϴ�!");
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
        // ��ġ ���� ����
        Vector3 randomPos = new Vector3(
            Random.Range(-8f, 15f), // minX, maxX
            Random.Range(25f, 30f), // minY, maxY
            Random.Range(3f, 25f)); // minZ, maxZ

        // ȸ�� ���� ����
        Vector3 randomRot = new Vector3(
            Random.Range(0f, 45f), // minX, maxX
            Random.Range(0f, 45f), // minY, maxY
            Random.Range(0f, 45f)); // minZ, maxZ

        existCrates.Add(Instantiate(setCrates[Random.Range(0, setCrates.Length)], randomPos, Quaternion.Euler(randomRot)));
    }
}
