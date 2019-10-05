using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerShoot : MonoBehaviour
{
    public KeyCode shootKey;
    public GameObject bulletPrefab;
    public Transform shootPosition;
    public float bulletSpeed;
    public float bulletLife = 1;
    public float chargeTime = 1;
    public float minChargeTime = .1f;
    float chargeTrack;
    bool isChargeShooting;

    public ParticleSystem chargeParticles;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shootKey) && ModuleSystem.instance.HasModule<ChargeGunModule>() && ModuleSystem.instance.HasModule<GunModule>())
        {
            chargeTrack = 0;
        }
        else if (Input.GetKey(shootKey) && ModuleSystem.instance.HasModule<ChargeGunModule>() && ModuleSystem.instance.HasModule<GunModule>())
        {
            chargeTrack += Time.deltaTime;
            if (chargeTrack >= minChargeTime)
            {
                isChargeShooting = true;
                EmissionModule em = chargeParticles.emission;
                em.enabled = true;
            }
        }
        else if (Input.GetKeyUp(shootKey) && ModuleSystem.instance.HasModule<GunModule>())
        {
            if (chargeTrack >= chargeTime)
                Shoot(2);
            else
                Shoot();
        }
    }

    void Shoot(float mod = 1)
    {
        Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
        bullet.transform.localScale *= mod;
        bullet.AddForce(shootPosition.right * bulletSpeed);
        Destroy(bullet.gameObject, bulletLife);
        isChargeShooting = false;
        EmissionModule em = chargeParticles.emission;
        em.enabled = false;
    }
}
