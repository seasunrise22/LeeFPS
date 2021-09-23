using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate;
    float nextTimeToFire = 0f;
    int bulletsPerMags = 300;    // 탄창당 총알 수
    /*int bulletsLeft = 200;      // 전체 보유 총알 수*/
    int currentBullets;         // 현재 남은 총알 수

    Animator animator;
    public Camera fpsCam;

    public ParticleSystem muzzleFlash; // 총구 이펙트
    public GameObject impactEffect; // 피탄 이펙트

    public AudioSource gunAudio;

    /*public float impactForce = 100f;*/

    // public으로 선언한 변수는 선언과 동시에 초기화가 되었다 손 치더라도,
    // inspector창에서의 값이 우선되어 적용되는 것 같다...
    // inspector창에서 Reset을 하면 되지만, 그러면 끌어다 둔 컴포넌트들이 죄다 탈락하므로,
    // Start에서 값을 초기화해주어 Play를 누름과 동시에 변경된 값으로 새로이 초기화 시킬 것임.
    private void Start()
    {
        fireRate = 0.15f;
        animator = gameObject.GetComponent<Animator>();
        currentBullets = bulletsPerMags;
        /*Debug.Log("currentBullets : " + currentBullets);*/
    }

    private void Update()
    {
        /*AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 = BaseLayer
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentBullets > 0)
        {
            if (animInfo.IsName("DrawGun")) // 총 뽑을 땐 총 못 쏘도록 하기 위한 조치
                return;

            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }*/
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {            
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }            
    }

    void Shoot()
    {
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
                /*Debug.Log("Target HP is : " + target.health);*/
            }

            /* 맞은 상자에 물리효과 주려면 추가 하던가
             * if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce); */

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
        currentBullets--;
        /*Debug.Log("currentBullets : " + currentBullets);*/                
    }
}
