using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public GameObject particlePrefab;
    bool isEntered; // �浹������ �� ���� ����Ű�� ���ؼ�.
    int crateGauge; // ���ں� �������� ä�� ��

    private void Start()
    {
        // �±׷� �����Ͽ� ���� �ٸ� ü���� ������ ����.
        if (gameObject.transform.tag == "Crate")
        {
            health = 50f;
            crateGauge = 100;
        }
            
        if (gameObject.transform.tag == "CrateBig")
        {
            health = 100f;
            crateGauge = 150;
        }            

        // GameManager�� AddCrates �Լ��� ������� ���� �ʵ忡 ���� �� ���� ���ڸ� +1 �� �� UI�� ǥ��.
        GameManager.instance.AddCrates();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        // ���ڰ� �ı��Ǹ� GameManager�� DestroyCrates �Լ��� ȣ����� ���� ���� ������ -1�ϰ� UI�� ����.
        GameManager.instance.DestroyCrates();
    }

    // ���ڰ� �ٴڿ� ����� �� ���� �������� ��½�Ű�� ����.
    private void OnCollisionEnter(Collision collision)
    {
        // ���ڰ� �ٴڿ� ������
        if(collision.gameObject.tag == "Ground" && !isEntered)
        {
            isEntered = true;
            GameManager.instance.AddGauge(crateGauge);
        }
    }
}
