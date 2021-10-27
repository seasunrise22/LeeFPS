using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �̱������� ����.
// ���� UI ����(���� ����, �Ѿ� ����, ������ ������)
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ���� ����

    private int cratesNum; // �ʵ忡 �����Ǿ� �ִ� ������ ������ ī��Ʈ�� ����.
    public Text cratesText; // ���� �ʵ忡 �����Ǿ� �ִ� ������ ���� ǥ���� ����.

    public Text currentAmmoText; // ���� ���� �Ѿ� ������ ǥ���� UI �ؽ�Ʈ
    public Text extraAmmoText; // ������ �Ѿ� ������ ǥ���� UI �ؽ�Ʈ

    public int maxGauge; // �ִ� ������ ������
    public int currentGauge; // ���� ������ ��ġ
    public GaugeBar gaugeBar; // ���� ������ ������ ������Ʈ�� ��ũ��Ʈ�� �����ų �Ű�ü

    public GameObject gameoverText; // ���ӿ����� Ȱ��ȭ ��ų �ؽ�Ʈ UI ������Ʈ

    int currentStage; // ���� ���������� ������ ����
    public Text stageText; // ���� ���������� ��Ÿ�� �ؽ�ƮUI ������Ʈ

    bool isGameover; // ���ӿ��� �������� �ƴ��� üũ��

    public Text timerText; // Ÿ�̸� ǥ�ÿ� UI ������Ʈ
    public float timeLeft; // ���� �ð�

    public bool isLvUp; // �������� ��ȯ�� Ʈ����

    // ���� ���۰� ���ÿ� �̱����� ����
    private void Awake()
    {
        // �̱��� ���� instance�� ��� �ִ°�?
        if(instance == null)
        {
            // instance�� ��� �ִٸ�(null) �װ��� �ڱ� �ڽ��� �Ҵ�
            instance = this;
        }
        else
        {
            // instance�� �̹� �ٸ� GameManager ������Ʈ�� �Ҵ�Ǿ� �ִ� ���
            // ���� �� �� �̻��� GameManager ������Ʈ�� �����Ѵٴ� �ǹ�
            // �̱��� ������Ʈ�� �ϳ��� �����ؾ� �ϹǷ� �ڽ��� ���� ������Ʈ�� �ı�.
            Debug.LogWarning("���� �� �� �̻��� ���� �Ŵ����� �����մϴ�!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentGauge = 0; // ���۰� ���ÿ� ������ 0���� �ʱ�ȭ
        maxGauge = 1000;
        gaugeBar.SetMaxGauge(maxGauge);

        currentStage = 1; // ���� ó�� �����ϸ� ���� �������� 1�� �ʱ�ȭ
        SetStage(currentStage);

        // �ݰ� ���� �� ó�� ToString("0")���� �Ҽ��� �Ʒ��� ������ ������ ����� �ݿø��� ���ڷ� ǥ�õż�
        // �̹����� ������ 30�� �ƴ� 30.5�� �ؾ��� �� �˾Ҵµ� 0.0���� �Ҽ��� �Ʒ����� ǥ���ϴϱ� ���������� 30�� ���� �Ǵ� ��?
        timeLeft = 5f; 
    }

    private void Update()
    {
        if(!isLvUp)
        {
            timerText.text = (timeLeft).ToString("0.00");
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                isLvUp = true;                
                timeLeft = 0f;                
                LevelManager.instance.SetStage(isLvUp);
            }
        }         

        // ���ӿ��� �ƴٸ�
        if(isGameover)
        {
            gameoverText.SetActive(true);
            Time.timeScale = 0;

            // RŰ�� ������ ���� �ٽ� �ҷ����� = ���� �ٽ� ����
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Game");
                Time.timeScale = 1;
            }
        }
    }

    public void AddGauge(int amount)
    {
        currentGauge += amount;
        
        // ���� ���������� ��ġ�� 100�� �Ѱ�ٸ� ���ӿ��� �ؽ�Ʈ�� Ȱ��ȭ�϶�
        if(currentGauge >= maxGauge)
        {
            isGameover = true;            
        }
        gaugeBar.SetGauge(currentGauge);
    }

    // ���ڰ� �����Ǹ� AddCrates �Լ��� ȣ���Ͽ� UI�� ���� �����Ǿ� �ִ� ���ڵ��� �� ������ ���Ž�ų ����. 
    public void AddCrates()
    {
        cratesNum++;
        cratesText.text = "���� ���� : " + cratesNum;
    }

    // ���ڰ� �ı��Ǹ� ���� ���� ������ -1�ϰ� UI�� ����.
    public void DestroyCrates()
    {
        cratesNum--;
        cratesText.text = "���� ���� : " + cratesNum;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo, int extraCurrentAmmo, int extraMaxAmmo)
    {
        if (currentAmmo < 0 || extraCurrentAmmo < 0)
            return;

        currentAmmoText.text = currentAmmo + " / " + maxAmmo;
        extraAmmoText.text = extraCurrentAmmo + " / " + extraMaxAmmo;
    }

    // ���� ���������� �����ϱ� ���� ��ɵ��� ��� �ִ� �Լ�
    void SetStage(int stage)
    {
        stageText.text = "Stage : " + stage;
    }
}
