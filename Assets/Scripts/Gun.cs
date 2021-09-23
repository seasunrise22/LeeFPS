using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate;
    float nextTimeToFire = 0f;
    int bulletsPerMags = 300;    // źâ�� �Ѿ� ��
    /*int bulletsLeft = 200;      // ��ü ���� �Ѿ� ��*/
    int currentBullets;         // ���� ���� �Ѿ� ��

    Animator animator;
    public Camera fpsCam;

    public ParticleSystem muzzleFlash; // �ѱ� ����Ʈ
    public GameObject impactEffect; // ��ź ����Ʈ

    public AudioSource gunAudio;

    /*public float impactForce = 100f;*/

    // public���� ������ ������ ����� ���ÿ� �ʱ�ȭ�� �Ǿ��� �� ġ����,
    // inspectorâ������ ���� �켱�Ǿ� ����Ǵ� �� ����...
    // inspectorâ���� Reset�� �ϸ� ������, �׷��� ����� �� ������Ʈ���� �˴� Ż���ϹǷ�,
    // Start���� ���� �ʱ�ȭ���־� Play�� ������ ���ÿ� ����� ������ ������ �ʱ�ȭ ��ų ����.
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
            if (animInfo.IsName("DrawGun")) // �� ���� �� �� �� ��� �ϱ� ���� ��ġ
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

            /* ���� ���ڿ� ����ȿ�� �ַ��� �߰� �ϴ���
             * if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce); */

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
        currentBullets--;
        /*Debug.Log("currentBullets : " + currentBullets);*/                
    }
}
