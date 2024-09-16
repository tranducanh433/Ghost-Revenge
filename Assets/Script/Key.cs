using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HumanPlayer") || collision.CompareTag("Enemy"))
        {
            Enemy e = collision.GetComponent<Enemy>();
            if (e.CanGetKey() == true)
            {
                e.GetKey();
                Destroy(gameObject);
            }
        }
    }
}
