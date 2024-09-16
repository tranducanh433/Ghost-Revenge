using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] Animator trapAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("HumanPlayer") || collision.CompareTag("Enemy"))
        {
            trapAnim.SetTrigger("up");
            collision.GetComponent<Enemy>().Death();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
