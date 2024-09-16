using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyArea : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<GhostPlayer>().Death();
        }
        if (collision.CompareTag("HumanPlayer"))
        {
            collision.GetComponent<Enemy>().ReleaseControl();
        }
    }
}
