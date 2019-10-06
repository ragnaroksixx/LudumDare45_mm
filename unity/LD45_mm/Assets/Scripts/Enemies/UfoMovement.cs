using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoMovement : MonoBehaviour
{
    enum Mode {
        Attack,
        Evade,
        Hover,
        Dying
    };
    AudioSource[] audioSources;
    public GameObject explosionPrefab;
    Rigidbody2D rb;
    public float health = 100.0f;
    private float currentHealth;
    public float speed = 5.0f;
    public float fireDelay = 1.0f;
    private float fireTimer;
    // Time that it takes the enemy to do its overall idle and evade cycle 
    public float cycleTime = 5.0f;
    public float attackTime = 3.0f;
    // Time left in an execution loop
    private float executionTime;
    public GameObject bulletPrefab;
    public GameObject superShotPrefab;
    public Transform shootPosition;
    public float bulletSpeed = 1000.0f;
    public float bulletLife = 1;
    private bool deathSoundPlayed = false;
    Mode mode;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        this.audioSources = GetComponents<AudioSource>();
        this.mode = Mode.Hover;
        this.executionTime = this.cycleTime;
        this.fireTimer = this.fireDelay;
        this.currentHealth = this.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.currentHealth <= 0.0f) this.mode = Mode.Dying;

        switch(mode){
            case Mode.Hover:
                if (this.executionTime > (this.cycleTime * .5))
                {
                    this.rb.velocity = new Vector2(this.speed, 0.0f);
                } 
                else if (this.executionTime > 0)
                {
                    this.rb.velocity = new Vector2(-this.speed, 0.0f);
                } 
                else 
                {
                    this.mode = Mode.Attack;
                    this.executionTime = this.cycleTime;
                }
                Fire();
                break;
            case Mode.Attack:
                this.rb.velocity = new Vector2(0, 0);
                if(this.executionTime < this.cycleTime * .3)
                {
                    SuperShot();  
                    this.executionTime = this.cycleTime;
                    this.mode = Mode.Evade;
                } 
                break;
            case Mode.Evade:
                if(this.executionTime > (this.cycleTime * .8)) 
                {
                    this.rb.velocity = new Vector2(this.speed, -this.speed * 2);
                }
                else if (this.executionTime > (this.cycleTime * .6))
                {
                    this.rb.velocity = new Vector2(this.speed, this.speed * 2);
                }
                else if (this.executionTime > (this.cycleTime * .4))
                {
                    this.rb.velocity = new Vector2(-this.speed, -this.speed  * 2);
                }
                else if (this.executionTime > (this.cycleTime * .2))
                {
                    this.rb.velocity = new Vector2(-this.speed, this.speed  * 2 );
                }
                else 
                {
                    this.executionTime = this.cycleTime;
                    this.mode = Mode.Hover;
                }
                break;
            case Mode.Dying:
                if(!deathSoundPlayed)
                {
                    this.audioSources[2].Play(0);
                    this.deathSoundPlayed = true;

                }

                if (this.executionTime > this.cycleTime * .5) {
                    this.rb.velocity = new Vector2(0, (float)(this.speed * -.6));
                    ParticleSystem p = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                    this.audioSources[1].Play(0);
                } 
                else
                {
                    this.audioSources[2].Play(0);
                    Object.Destroy(this.gameObject);
                }
                break;
        }
        this.fireTimer -= Time.deltaTime;
        this.executionTime -= Time.deltaTime;

        // Animation update
        anim.SetFloat("Health", this.health);
    }

    void Fire()
    {
        if (this.fireTimer < 0.0f) 
        {
            Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
            bullet.AddForce((PlayerMovement.instance.transform.position - shootPosition.position) * bulletSpeed);
            Destroy(bullet.gameObject, bulletLife); 
            this.fireTimer = this.fireDelay;
            this.currentHealth -= (this.health * .1f);
            anim.SetBool("isShooting", true);
            this.audioSources[1].Play(0);
        }
        else
        {
            anim.SetBool("isShooting", false);
        }
    }
    void SuperShot()
    {
        this.audioSources[1].Play(0);
        Rigidbody2D bullet = GameObject.Instantiate(superShotPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
        bullet.AddForce((PlayerMovement.instance.transform.position - shootPosition.position) * bulletSpeed);
        Destroy(bullet.gameObject, bulletLife); 
        this.health -= (this.health * .33f);
        anim.SetBool("isShooting", true);
    }
}
