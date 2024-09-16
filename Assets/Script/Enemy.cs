using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus
{
    idle,
    move,
    aiming,
    shoting,
    death
}

public enum PlayerStatus
{
    normal,
    attack
}

public class Enemy : MonoBehaviour
{
    [Header("Moverment")]
    [SerializeField] protected bool stay;
    [SerializeField] protected float speed = 5;
    [SerializeField] Transform platformCheck;
    [SerializeField] protected bool isRight;
    [SerializeField] LayerMask laderLayer;
    bool beJoinedLately = false;
    bool disableGravity = false;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] float jumpGravity;
    [SerializeField] float fallGravity;
    private bool isGround;

    [Header("Attack System")]
    [SerializeField] float cdTimeEnemy;
    [SerializeField] float cdTimePlayer;
    float cd;
    protected bool shootDone = false;

    [Header("Other")]
    [SerializeField] bool useGun = true;
    [SerializeField] AudioClip deathSound;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] GameObject key;
    [SerializeField] GameObject keyEffect;
    [SerializeField] GameObject ghostPlayer;
    [SerializeField] GameObject joinedSignal;
    [SerializeField] GameObject whatSighnal;
    protected EnemyStatus enemyStatus;
    protected PlayerStatus playerStatus;
    protected bool beControl;

    protected float moveInput;
    bool canGetOut;

    Rigidbody2D rb;
    SpriteRenderer sr;
    protected AudioSource audioS;
    protected Animator anim;

    //Function
    protected void StartFunc()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();

        cd = cdTimeEnemy;
        enemyStatus = EnemyStatus.move;
        playerStatus = PlayerStatus.normal;
        SetStartDir();
    }
    protected void UpdateFunc()
    {
        isGround = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        if (beControl == true)
        {
            PlayerControl();
            PlayerAttackSystem();
        }
        else
        {

            if (stay == true)
            {
                anim.SetBool("Move", false);
                EnemyAttackSystem();
            }
            else
            {
                EnemyMovement();
                EnemyIdle();
                EnemyAttackSystem();
            }
        }
        PlayerGravityController();
    }

    //Enemy System
    void SetStartDir()
    {
        if (isRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (isRight == false)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    protected virtual void EnemyMovement()
    {
        if (enemyStatus == EnemyStatus.move)
        {
            anim.SetBool("Move", true);

            bool checkEnd = Physics2D.Linecast(platformCheck.position, new Vector2(platformCheck.position.x, platformCheck.position.y - 1), groundLayer);
            if (!checkEnd)
            {
                StartCoroutine(ChangeMoveDirCO());
            }
            else
            {
                EnemyMove();
            }
        }
    }

    void EnemyMove()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void EnemyIdle()
    {
        if (enemyStatus == EnemyStatus.idle)
        {
            anim.SetBool("Move", false);

            if (beJoinedLately == true && isGround && stay == false)
            {
                beJoinedLately = false;
                SetStartDir();
                enemyStatus = EnemyStatus.move;
            }
        }
    }

    protected IEnumerator ChangeMoveDirCO()
    {
        enemyStatus = EnemyStatus.idle;
        anim.SetBool("Move", false);
        yield return new WaitForSeconds(Random.Range(1f, 2.5f));

        ChangeDir();
    }

    void ChangeDir()
    {
        if (isRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isRight = false;
        }
        else if (isRight == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isRight = true;
        }
        enemyStatus = EnemyStatus.move;
        anim.SetBool("Move", true);
        anim.SetBool("Aiming", false);
    }

    void EnemyAttackSystem()
    {
        if(enemyStatus == EnemyStatus.aiming && useGun == true)
        {
            shootDone = false;
            if (cd <= 0)
            {
                    EnemyAttack();
                    cd = cdTimeEnemy;
            }
            else
            {
                cd -= Time.deltaTime;
            }
        }
    }
    protected virtual void EnemyAttack()
    {

    }

    public void Attendtion(Transform target)
    {
        StopCoroutine(ChangeMoveDirCO());

        bool barrier = Physics2D.Raycast(transform.position, target.position, 0, groundLayer);

        if(barrier == false)
        {
            if (target.position.x > transform.position.x && isRight == false && target != transform)
            {
                ChangeDir();
                StartCoroutine(WhatSighnalCO());
            }
            if (target.position.x < transform.position.x && isRight == true && target != transform)
            {
                ChangeDir();
                StartCoroutine(WhatSighnalCO());
            }
        }
    }

    protected IEnumerator WhatSighnalCO()
    {
        whatSighnal.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        whatSighnal.SetActive(false);
    }

    //player system
    void PlayerControl()
    {
        if(playerStatus == PlayerStatus.normal)
        {
            anim.SetBool("Aiming", false);
            moveInput = Input.GetAxisRaw("Horizontal");

            if (moveInput != 0)
            {
                anim.SetBool("Move", true);
            }
            else
            {
                anim.SetBool("Move", false);
            }
            PlayerMove(moveInput);

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        bool lader = Physics2D.OverlapBox(transform.position, new Vector2(0.75f, 2), 0, laderLayer);
        if(lader == true && Input.GetKey(KeyCode.W))
        {
            rb.velocity = Vector2.zero;
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            disableGravity = true;
        }
        else
        {
            disableGravity = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && enemyStatus != EnemyStatus.death)
        {
            ReleaseControl();
        }
    }

    void PlayerAttackSystem()
    {
        if (cd <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                PlayerMove(0);
                playerStatus = PlayerStatus.attack;
                PlayerAttack();
                cd = cdTimePlayer;
            }
        }
        else
        {
            cd -= Time.deltaTime;
        }
    }

    protected virtual void PlayerAttack()
    {

    }

    //Ghost System
    public void TakeControl()
    {
        joinedSignal.SetActive(true);
        gameObject.tag = "HumanPlayer";
        gameObject.layer = 7;
        beControl = true;
        beJoinedLately = true;
        canGetOut = false;

        cd = 0;

        playerStatus = PlayerStatus.normal;
        anim.SetBool("Aiming", false);
        StartCoroutine(CanGetOutCO());
    }
    IEnumerator CanGetOutCO()
    {
        yield return new WaitForSeconds(0.05f);
        canGetOut = true;
    }
    public void ReleaseControl()
    {
        if (canGetOut == true)
        {
            joinedSignal.SetActive(false);
            gameObject.tag = "Enemy";
            gameObject.layer = 3;
            Instantiate(ghostPlayer, transform.position, Quaternion.identity, transform.parent);
            enemyStatus = EnemyStatus.idle;
            beControl = false;
            disableGravity = false;
            cd = cdTimeEnemy;
            PlayerMove(0);
        }
    }

    //Other

    void PlayerGravityController()
    {
        if(disableGravity == false)
        {
            if (rb.velocity.y < 0 && groundLayer != 4)
            {
                rb.gravityScale = fallGravity;
            }
            else
            {
                rb.gravityScale = jumpGravity;
            }
        }
        else
        {
            rb.gravityScale = 0;
        }
    }
    public void PlayerMove(float moveInput)
    {
        if (moveInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    public void Jump()
    {
        if (isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    public bool CanGetKey()
    {
        return !keyEffect.activeSelf;
    }
    public void GetKey()
    {
        keyEffect.SetActive(true);
    }

    public void UseKey()
    {
        keyEffect.SetActive(false);
    }

    public virtual void Death(Vector2 bulletPos)
    {
        enemyStatus = EnemyStatus.death;
        StartCoroutine(DeathCO());
    }
    public void Death()
    {
        enemyStatus = EnemyStatus.death;
        StartCoroutine(DeathCO());
    }

    protected IEnumerator DeathCO()
    {
        audioS.PlayOneShot(deathSound);
        if(key != null && keyEffect.activeSelf == true) Instantiate(key, transform.position, Quaternion.identity, transform.parent);
        gameObject.tag = "None";
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(0.05f);
        LevelManager level = GetComponentInParent<LevelManager>();
        if (level != null)
            level.DeathSignal();
        Destroy(gameObject);
    }
}
