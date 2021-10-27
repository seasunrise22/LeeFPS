using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public GameObject particlePrefab;
    bool isEntered; // 충돌감지를 한 번만 일으키기 위해서.
    int crateGauge; // 상자별 게이지를 채울 양

    private void Start()
    {
        // 태그로 구별하여 서로 다른 체력을 갖도록 지시.
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

        // GameManager의 AddCrates 함수를 실행시켜 현재 필드에 생성 된 상자 숫자를 +1 한 후 UI에 표시.
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
        // 상자가 파괴되면 GameManager의 DestroyCrates 함수를 호출시켜 남은 상자 개수를 -1하고 UI에 갱신.
        GameManager.instance.DestroyCrates();
    }

    // 상자가 바닥에 닿았을 때 상자 게이지를 상승시키기 위함.
    private void OnCollisionEnter(Collision collision)
    {
        // 상자가 바닥에 닿으면
        if(collision.gameObject.tag == "Ground" && !isEntered)
        {
            isEntered = true;
            GameManager.instance.AddGauge(crateGauge);
        }
    }
}
