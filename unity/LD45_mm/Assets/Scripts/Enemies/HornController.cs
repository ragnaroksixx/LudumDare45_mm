using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornController : MonoBehaviour
{
    enum Mode {
        Patrol,
        newGoal,
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

    public GameObject[] waypoints;
    private int activeWaypoint = -1;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        this.mode = Mode.newGoal ;
        this.executionTime = this.cycleTime;
        this.fireTimer = this.fireDelay;
        this.currentHealth = this.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.health < 0.0f) this.mode = Mode.Dying;

        switch(mode){
            case Mode.Patrol:
                Vector3 goal = this.waypoints[this.activeWaypoint].transform.position;
                if (V3Equal(this.transform.position, goal))
                    this.mode = Mode.newGoal;
                float rot_z = Mathf.Atan2(PlayerMovement.instance.transform.position.y, PlayerMovement.instance.transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                Vector3 vector = goal - this.transform.position;
                vector = vector / Mathf.Sqrt(vector.x*vector.x + vector.y*vector.y);
                this.rb.velocity = vector * this.speed;
                break;
            case Mode.newGoal:
                this.rb.velocity = new Vector2(0, 0);
                this.activeWaypoint += 1;
                if(activeWaypoint >= this.waypoints.Length) 
                {
                    this.mode = Mode.Dying;
                    break;
                }
                this.mode = Mode.Patrol;
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

     public bool V3Equal(Vector3 a, Vector3 b){
        return Vector3.SqrMagnitude(a - b) < 0.1;
    }
}
