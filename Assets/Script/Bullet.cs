using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("HumanPlayer"))
        {
            collision.GetComponent<Enemy>().Death(transform.position);
            Destroy(gameObject);
        }

        if (!collision.CompareTag("Player") && !collision.CompareTag("None"))
        {
            Destroy(gameObject);
        }
    }
}
