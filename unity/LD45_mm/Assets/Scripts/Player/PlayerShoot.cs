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
    public Color chargeColor;
    int d;
    public ParticleSystem chargeParticles;

    // Start is called before the first frame update
    void Start()
    {
        MainModule mm = chargeParticles.main;
        mm.duration = chargeTime / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shootKey) && ModuleSystem.instance.HasModule<ChargeGunModule>())
        {
            chargeTrack = 0;
        }
        else if (Input.GetKey(shootKey) && ModuleSystem.instance.HasModule<ChargeGunModule>())
        {
            chargeTrack += Time.deltaTime;
            if (chargeTrack >= minChargeTime && !isChargeShooting)
            {
                isChargeShooting = true;
                chargeParticles.Play();

            }
        }
        else if (Input.GetKeyUp(shootKey) && (ModuleSystem.instance.HasModule<GunModule>() || ModuleSystem.instance.HasModule<ChargeGunModule>()))
        {
            if (chargeTrack >= chargeTime && ModuleSystem.instance.HasModule<GunModule>())
                Shoot(1.5f);
            else
                Shoot();
        }
    }

    void Shoot(float mod = 1)
    {
        Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
        bullet.transform.localScale *= mod;
        bullet.AddForce(shootPosition.right * bulletSpeed * mod);
        Destroy(bullet.gameObject, bulletLife * mod);
        isChargeShooting = false;
        chargeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (mod > 1)
        {
            bullet.GetComponent<PlayerBullet>().SetColor(chargeColor);
            bullet.GetComponent<PlayerBullet>().Damage = 4;
        }
    }

}
