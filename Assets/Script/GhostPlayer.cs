using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    [Header("Ghost Setting")]
    [SerializeField] GameObject selectBox;
    [SerializeField] float ghostSpeed;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector2 boxSize;

    Vector2 moveInput;

    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        selectBox = GameObject.FindGameObjectWithTag("SelectBox");
    }

    void Update()
    {
        Movement();
        SelectEnemy();
    }

    void Movement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        rb.MovePosition(rb.position + moveInput * ghostSpeed * Time.fixedDeltaTime);
    }

    void SelectEnemy()
    {
        Collider2D selectedEnemy = Physics2D.OverlapBox(transform.position, boxSize, 0, enemyLayer);
        if(selectedEnemy != null)
        {
            selectBox.transform.position = selectedEnemy.transform.position;
        }
        else
        {
            selectBox.transform.position = new Vector2(0, 100);
        }

        if (Input.GetKeyDown(KeyCode.E) && selectedEnemy != null)
        {
            anim.SetTrigger("Joined");
            Enemy enemy = selectedEnemy.GetComponent<Enemy>();
            enemy.TakeControl();
            gameObject.tag = "None";
            gameObject.layer = 0;
            Follow follow = gameObject.AddComponent<Follow>();
            follow.SetTarget(selectedEnemy.transform);
            selectBox.transform.position = new Vector2(0, 100);
            Destroy(this);
        }
    }

    public void Death()
    {
        anim.SetTrigger("Joined");
        StartCoroutine(DeathCO());
    }

    IEnumerator DeathCO()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        GetComponentInParent<LevelManager>().DeathSignal();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
