using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate;
    float nextTimeToFire = 0f;
    int maxAmmo = 40;           // źâ �뷮
    int currentAmmo;            // ���� źâ�� �ִ� �Ѿ� ��
    int extraCurrentAmmo = 40;  // ���� �������� ������ �Ѿ�
    int extraMaxAmmo = 200;     // ������ źâ �뷮
    bool isReloading = false;

    Animator animator;
    public Camera fpsCam;

    public ParticleSystem muzzleFlash; // �ѱ� ����Ʈ
    public GameObject impactEffect; // ��ź ����Ʈ

    public AudioSource gunAudio; // �ѿ� �޸� AudioSource = ī��Ʈ������
    public AudioClip shootAudio; // �� �߻� �Ҹ�
    public AudioClip reloadAudio; // ������ �Ҹ�

    // public���� ������ ������ ����� ���ÿ� �ʱ�ȭ�� �Ǿ��� �� ġ����,
    // inspectorâ������ ���� �켱�Ǿ� ����Ǵ� �� ����...
    // inspectorâ���� Reset�� �ϸ� ������, �׷��� ����� �� ������Ʈ���� �˴� Ż���ϹǷ�,
    // Start���� ���� �ʱ�ȭ���־� Play�� ������ ���ÿ� ����� ������ ������ �ʱ�ȭ ��ų ����.
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

        // R��ư ������� ������ �ǵ���
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
