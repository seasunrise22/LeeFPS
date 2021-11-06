using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate;
    float nextTimeToFire = 0f;
    int maxAmmo = 40;           // 탄창 용량
    int currentAmmo;            // 현재 탄창에 있는 총알 수
    int extraCurrentAmmo = 40;  // 현재 보유중인 여분의 총알
    int extraMaxAmmo = 200;     // 여분의 탄창 용량
    bool isReloading = false;

    Animator animator;
    public Camera fpsCam;

    public ParticleSystem muzzleFlash; // 총구 이펙트
    public GameObject impactEffect; // 피탄 이펙트

    public AudioSource gunAudio;    // 총에 달린 AudioSource = 카세트테이프
    public AudioClip shootAudio;    // 총 발사 소리
    public AudioClip reloadAudio;   // 재장전 소리
    public AudioClip emptyAmmo;     // 총알이 완전히 소진됐을 때 소리

    // public으로 선언한 변수는 선언과 동시에 초기화가 되었다 손 치더라도,
    // inspector창에서의 값이 우선되어 적용되는 것 같다...
    // inspector창에서 Reset을 하면 되지만, 그러면 끌어다 둔 컴포넌트들이 죄다 탈락하므로,
    // Start에서 값을 초기화해주어 Play를 누름과 동시에 변경된 값으로 새로이 초기화 시킬 것임.
    private void Start()
    {
        fireRate = 0.15f;
        animator = gameObject.GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if(Time.timeScale == 1)
        {
            GameManager.instance.UpdateAmmo(currentAmmo, maxAmmo, extraCurrentAmmo, extraMaxAmmo);

            // 제장전 상태
            if (isReloading)
                return;

            // 사격
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                // 테스트용
                /*GameManager.instance.isGameover = true;*/

                // 사격 버튼을 눌렀을 때 현재 총알이 존재한다면
                if (currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + fireRate;
                    Shoot();
                }
                // 사격 버튼을 눌렀을 때 현재 총알이 존재하지 않는다면 
                else if (currentAmmo == 0 && extraCurrentAmmo == 0 && Input.GetButtonDown("Fire1"))
                {
                    gunAudio.clip = emptyAmmo;
                    gunAudio.Play();
                    return;
                }

            }

            // 근접공격
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetTrigger("MeleeAttack");
            }

            // 재장전 관련(R키를 눌렀으면서(AND) 현재 총알과 전체 총알 개수가 맞지 않거나(OR) 현재 총알이 0발일 경우)
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo || currentAmmo <= 0)
            {
                // ~인 와중에 R키를 눌렀는데 여분 총알까지 없다?
                if (Input.GetKeyDown(KeyCode.R) && currentAmmo == 0 && extraCurrentAmmo == 0)
                {
                    gunAudio.clip = emptyAmmo;
                    gunAudio.Play();
                    return;
                }
                // R키를 눌렀는데 현재 총알은 있는데 여분 총알은 없다?
                else if (Input.GetKeyDown(KeyCode.R) && extraCurrentAmmo == 0)
                {
                    gunAudio.clip = emptyAmmo;
                    gunAudio.Play();
                    return;
                }
                // 아무것도 없지만 R키는 누르지 않았다?            
                else if (currentAmmo == 0 && extraCurrentAmmo == 0)
                {
                    return;
                }

                // 이도 저도 아니다 = 여분 총알은 있다. = 재장전 수행.
                StartCoroutine(Reload());
            }
        }        
    }

    void Shoot()
    {      
        gunAudio.clip = shootAudio;
        gunAudio.Play();
        muzzleFlash.Play();        
        animator.SetTrigger("Shoot");
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
        currentAmmo--;
    }

    IEnumerator Reload()
    {
        gunAudio.clip = reloadAudio;
        gunAudio.Play();

        isReloading = true;
        animator.SetBool("isReloading", isReloading);

        yield return new WaitForSeconds(1.6f);

        isReloading = false;
        animator.SetBool("isReloading", isReloading);

        int reloadAmount = maxAmmo - currentAmmo;
        if (reloadAmount > extraCurrentAmmo)
            reloadAmount = extraCurrentAmmo;
        extraCurrentAmmo -= reloadAmount;
        currentAmmo += reloadAmount;
    }
}
