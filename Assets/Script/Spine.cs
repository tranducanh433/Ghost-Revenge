using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("HumanPlayer"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Death();
        }
    }
}
