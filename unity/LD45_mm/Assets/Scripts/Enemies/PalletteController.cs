using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletteController : MonoBehaviour
{
    enum Mode {
        Attack,
        SuperShot,
        Traverse,
        Hurt,
        Dying
    };
    Rigidbody2D rb;
    public Animator anim;
    
    public GameObject explosionPrefab;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public float fallMult = 2.5f;
    public Transform bottomOffset;
    
    public bool isFacingRight = true;
    public float speed = 5.0f;
    int attackCount;
    // Time that it takes the enemy to do its overall idle and evade cycle 
    public float cycleTime = 5.0f;
    public float fireDelay = .5f;
    private float fireTimer;
    // Time left in an execution loop
    private float executionTime;
    public GameObject bulletPrefab;
    public GameObject superShotPrefab;
    public Transform shootPosition;
    public float bulletSpeed = 150.0f;
    public float bulletLife = 1;
    private bool canBeHit;
    Mode mode;
    AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        this.rb=GetComponent<Rigidbody2D>();
        this.audioSources = GetComponents<AudioSource>();
        this.mode = Mode.Traverse;
        this.executionTime = this.cycleTime;
        this.fireTimer = this.fireDelay;
        this.attackCount = 0;
        this.canBeHit = true;
        this.isFacingRight = false;
        FaceDirection(this.isFacingRight);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        isGrounded = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
        
        switch(mode){
            case Mode.Traverse:
                if (this.executionTime > 0)
                {
                    this.rb.velocity = isFacingRight ? (new Vector2(this.speed * 2, this.rb.velocity.y)) : (new Vector2(-this.speed *2, this.rb.velocity.y));
                } 
                else 
                {
                    anim.SetBool("isShooting", true);
                    this.isFacingRight = !this.isFacingRight;
                    FaceDirection(isFacingRight);
                    this.executionTime = this.cycleTime;
                    this.mode = (attackCount < 1) ? Mode.Attack : Mode.SuperShot;
                }
                break;
            case Mode.Hurt:
                if (this.executionTime > 2)
                {
                    anim.SetBool("isHurt", true);
                } 
                else 
                {
                    anim.SetBool("isHurt", false);
                    this.mode = Mode.Traverse;
                    this.canBeHit = true;
                }
                break;
            case Mode.Attack:
                this.rb.velocity = new Vector2(0, this.rb.velocity.y);
                if(this.executionTime > this.cycleTime * 4)
                {
                    this.rb.velocity = new Vector2(0, this.speed);
                    Fire(); 
                }
                else if(this.executionTime > 0)
                {
                   Fire(); 
                }
                else {
                    this.attackCount += 1;
                    this.executionTime = this.cycleTime;
                    this.mode = Mode.Traverse;
                    anim.SetBool("isShooting", false);
                } 
                break;
            case Mode.SuperShot:
                this.rb.velocity = new Vector2(0, 0);
                if(this.executionTime < 3) 
                {
                    SuperShot();
                    this.attackCount = 0;
                    this.executionTime = this.cycleTime;
                    this.mode = Mode.Traverse;
                    anim.SetBool("isShooting", false);
                }
                break;
            case Mode.Dying:
                this.rb.velocity = new Vector2(0, 0);
                ParticleSystem p = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                anim.SetBool("isDead", true);
                if(this.executionTime < 3) 
                {
                    Pose pose = new Pose(transform.position, Quaternion.identity);
                    ModuleSystem.instance.Spawn(typeof(FullSightModule), pose);
                    this.audioSources[2].Play();
                    Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }

        if (!isGrounded)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
        }

        this.fireTimer -= Time.deltaTime;
        this.executionTime -= Time.deltaTime;
    }

    void FaceDirection(bool right)
    {
        isFacingRight = right;
        transform.localEulerAngles = new Vector3(0, right ? 0 : 180, 0);
    }

    void Fire()
    {
        if (this.fireTimer < 0.0f) 
        {
            Rigidbody2D bullet = GameObject.Instantiate(bulletPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
            bullet.AddForce((isFacingRight ? new Vector2(1, 0) : new Vector2(-1, 0)) * bulletSpeed);
            Destroy(bullet.gameObject, bulletLife); 
            this.fireTimer = this.fireDelay;
            audioSources[1].Play(1);
        }
    }
    void SuperShot()
    {
        Rigidbody2D bullet = GameObject.Instantiate(superShotPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
        bullet.AddForce((PlayerMovement.instance.transform.position - shootPosition.position) * bulletSpeed);
        Destroy(bullet.gameObject, bulletLife); 
        audioSources[1].Play(0);
    }

    public bool TakeDamage(){
        if(canBeHit && (this.mode == Mode.Traverse || this.mode == Mode.Attack))
        {
            this.mode = Mode.Hurt;
            this.canBeHit = false;
            return true;
        } 
        else
        {
            return false;
        }
    }
    public void Death()
    {
        audioSources[0].Play(0);
        this.executionTime = this.cycleTime;
        this.mode = Mode.Dying;
    }
}
