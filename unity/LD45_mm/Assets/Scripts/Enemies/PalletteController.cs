using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletteController : MonoBehaviour
{
    enum Mode {
        Attack,
        SuperShot,
        Traverse
    };
    Rigidbody2D rb;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public float fallMult = 2.5f;
    public Transform bottomOffset;
    
    public bool isFacingRight = true;
    public float health = 100.0f;
    private float currentHealth;
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
    public float bulletSpeed = 500.0f;
    public float bulletLife = 1;
    Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        this.mode = Mode.Traverse;
        this.executionTime = this.cycleTime;
        this.fireTimer = this.fireDelay;
        this.attackCount = 0;
        this.currentHealth = this.health;
    }

    // Update is called once per frame
    void Update()
    {
        
        isGrounded = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);

        switch(mode){
            case Mode.Traverse:
                if (this.executionTime > 0)
                {
                    this.rb.velocity = isFacingRight ? (new Vector2(this.speed * 2, this.rb.velocity.y)) : (new Vector2(-this.speed * 2, this.rb.velocity.y));
                } 
                else 
                {
                    this.rb.velocity = new Vector2(0, this.speed);
                    this.isFacingRight = !this.isFacingRight;
                    FaceDirection(isFacingRight);
                    this.executionTime = this.cycleTime;
                    this.mode = (attackCount < 2) ? Mode.Attack : Mode.SuperShot;
                }
                break;
            case Mode.Attack:
                if(this.executionTime > 4)
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
                } 
                break;
            case Mode.SuperShot:
                this.rb.velocity = new Vector2(0, 0);
                if(this.executionTime < 0) 
                {
                    SuperShot();
                    this.attackCount = 0;
                    this.executionTime = this.cycleTime;
                    this.mode = Mode.Traverse;
                }
                break;
            default:
                break;
        }

        if (!isGrounded)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
        }

        if(this.currentHealth <= 0.0f) Death();
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
            bullet.AddForce((PlayerMovement.instance.transform.position - shootPosition.position) * new Vector2(1, 0) * bulletSpeed);
            Destroy(bullet.gameObject, bulletLife); 
            this.fireTimer = this.fireDelay;
        }
    }
    void SuperShot()
    {
        Rigidbody2D bullet = GameObject.Instantiate(superShotPrefab, shootPosition.position, shootPosition.rotation).GetComponent<Rigidbody2D>();
        bullet.AddForce((PlayerMovement.instance.transform.position - shootPosition.position) * bulletSpeed);
        Destroy(bullet.gameObject, bulletLife); 
    }

    void Death()
    {
        // todo: trigger death animation
        Object.Destroy(this.gameObject);
    }
}
