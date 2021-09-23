using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public GameObject particlePrefab;

    private void Start()
    {
        // 태그로 구별하여 서로 다른 체력을 갖도록 지시.
        if (gameObject.transform.tag == "Crate")
            health = 50f;
        if (gameObject.transform.tag == "CrateBig")
            health = 100f;

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
}
