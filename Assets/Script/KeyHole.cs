using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
    [SerializeField] GameObject objToDestroy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HumanPlayer"))
        {
            Enemy e = collision.GetComponent<Enemy>();
            if (e.CanGetKey() == false)
            {
                e.UseKey();
                Destroy(objToDestroy);
                Destroy(gameObject);
            }
        }
    }
}
