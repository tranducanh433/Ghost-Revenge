using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    [SerializeField] GameObject explodeEffect;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float explodeRadius;
    [SerializeField] AudioClip explodeSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("boom");
            StartCoroutine(TriggerCO());
        }
    }

    IEnumerator TriggerCO()
    {
        yield return new WaitForSeconds(0.95f);
        GetComponent<AudioSource>().PlayOneShot(explodeSound);
        yield return new WaitForSeconds(0.05f);
        Instantiate(explodeEffect, transform.position, Quaternion.identity);
        Collider2D[] target = Physics2D.OverlapCircleAll(transform.position, explodeRadius, enemyLayer);
        foreach (Collider2D enemy in target)
        {
            enemy.GetComponent<Enemy>().Death();
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
