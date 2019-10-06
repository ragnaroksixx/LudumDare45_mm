using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornController : MonoBehaviour
{
    enum Mode {
        TraverseVertical,
        TraverseLeft,
        TraverseRight,
        Attack,
        Dying
    };
    Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isLocatedBottom = false;
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
    Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        this.mode = Mode.TraverseVertical;
        this.executionTime = this.cycleTime;
        this.fireTimer = this.fireDelay;
        this.currentHealth = this.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.health < 0.0f) this.mode = Mode.Dying;

        switch(mode){
            case Mode.TraverseVertical:
                if (this.executionTime > 0)
                {
                    this.rb.velocity = this.isLocatedBottom ? new Vector2(0.0f, this.speed) : new Vector2(0.0f, -this.speed);
                    Fire();
                } 
                else 
                {
                    this.isLocatedBottom = true;
                    this.executionTime = this.cycleTime;
                    if (isFacingRight)
                    {
                        this.mode = Mode.TraverseRight;
                    }
                    else
                    {
                        this.mode = Mode.TraverseLeft;
                    }
                }
                break;
            case Mode.TraverseRight:
                if (this.executionTime > 0)
                {
                    this.rb.velocity = this.isLocatedBottom ? new Vector2(this.speed, this.speed) : new Vector2(this.speed, -this.speed);
                } 
                else 
                {
                    isFacingRight = !isFacingRight;
                    FaceDirection(isFacingRight);
                    this.isLocatedBottom = !this.isLocatedBottom;
                    this.executionTime = this.cycleTime;
                    this.fireTimer = this.fireDelay;
                    this.mode = Mode.TraverseVertical;
                }
                break;
            case Mode.TraverseLeft:
                if (this.executionTime > 0)
                {
                    this.rb.velocity = this.isLocatedBottom ? new Vector2(-this.speed, this.speed) : new Vector2(-this.speed, -this.speed);
                } 
                else 
                {
                    isFacingRight = !isFacingRight;
                    FaceDirection(isFacingRight);
                    this.isLocatedBottom = !this.isLocatedBottom;
                    this.executionTime = this.cycleTime;
                    this.fireTimer = this.fireDelay;
                    this.mode = Mode.TraverseVertical;
                }
                break;
            case Mode.Attack:
                this.rb.velocity = new Vector2(0, 0);
                if(this.executionTime < 0)
                {
                    SuperShot();  
                    this.executionTime = this.cycleTime;
                    
                } 
                break;
            case Mode.Dying:
                if (this.executionTime > 0) {
                    this.rb.velocity = new Vector2(0, (float)(this.speed * -.3));
                } 
                else
                {
                    Object.Destroy(this.gameObject);
                }
                break;
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
}
