using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int maxGauge = 100; // �ִ� ������ ������
    public int currentGauge; // ���� ������ ��ġ
    public GaugeBar gaugeBar; // ���� ������ ������ ������Ʈ�� ��ũ��Ʈ�� �����ų �Ű�ü

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
}
