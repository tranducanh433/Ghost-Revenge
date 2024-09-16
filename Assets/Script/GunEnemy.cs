using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : Enemy
{
    [Header("Gun enemy Setting")]
    [SerializeField] AudioClip gunSound;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform shotPoint;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bullet;
    bool canAttack;

    private void Start()
    {
        StartFunc();
    }
    private void Update()
    {
        if(!beControl) FindPlayer();
        UpdateFunc();
    }

    protected override void PlayerAttack()
    {
        audioS.PlayOneShot(gunSound);
        anim.SetBool("Aiming", true);
        anim.SetTrigger("Shoot");
        GameObject b = Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        b.GetComponent<Rigidbody2D>().velocity = shotPoint.right * bulletSpeed;
        Destroy(b, 3);


        Vector2 pos2 = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] enemyRight = Physics2D.LinecastAll(pos2, pos2 + (Vector2)transform.right * 30, enemyLayer);
        RaycastHit2D[] leftRight = Physics2D.LinecastAll(pos2, pos2 - (Vector2)transform.right * 30, enemyLayer);

        foreach(RaycastHit2D enemy in enemyRight)
        {
            GameObject g = enemy.transform.gameObject;
            g.GetComponent<Enemy>().Attendtion(transform);
        }
        foreach (RaycastHit2D enemy in leftRight)
        {
            GameObject g = enemy.transform.gameObject;
            g.GetComponent<Enemy>().Attendtion(transform);
        }
    }

    protected override void EnemyAttack()
    {
        audioS.PlayOneShot(gunSound);
        anim.SetTrigger("Shoot");
        GameObject b = Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        b.GetComponent<Rigidbody2D>().velocity = shotPoint.right * bulletSpeed;
        Destroy(b, 3);
    }

    public void FindPlayer()
    {
        Vector2 pos2 = new Vector2(transform.position.x, transform.position.y);
        canAttack = Physics2D.Linecast(pos2, pos2 + (Vector2)transform.right * 30, playerLayer);
        if (canAttack == true)
        {
            StartCoroutine(WhatSighnalCO());
            StopCoroutine(ChangeMoveDirCO());
            enemyStatus = EnemyStatus.aiming;
            anim.SetBool("Aiming", true);
        }
        else if (!canAttack && enemyStatus == EnemyStatus.aiming && shootDone == true)
        {
            if (stay == false)
                enemyStatus = EnemyStatus.move;
            else
                enemyStatus = EnemyStatus.idle;
            anim.SetBool("Aiming", false);
        }
    }

    public void StartShoot()
    {
        enemyStatus = EnemyStatus.shoting;
        playerStatus = PlayerStatus.attack;
    }
    public void EndShoot()
    {
        enemyStatus = EnemyStatus.aiming;
        playerStatus = PlayerStatus.normal;
        shootDone = true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 pos2 = new Vector2(transform.position.x, transform.position.y);
        Gizmos.DrawLine(pos2, pos2 + (Vector2)transform.right * 30);
        Gizmos.DrawWireCube(transform.position, new Vector2(0.75f, 2));
    }
}
