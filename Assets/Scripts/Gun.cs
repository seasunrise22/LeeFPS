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

    public AudioSource gunAudio; // 총에 달린 AudioSource = 카세트테이프
    public AudioClip shootAudio; // 총 발사 소리
    public AudioClip reloadAudio; // 재장전 소리

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
        GameManager.instance.UpdateAmmo(currentAmmo, maxAmmo, extraCurrentAmmo, extraMaxAmmo);

        if (isReloading)
            return;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {            
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }            

        // R버튼 누를경우 재장전 되도록
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo || currentAmmo <= 0)
        {
            StartCoroutine(Reload());
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
        currentAmmo = maxAmmo;
    }
}
