using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
 /*   // Start is called before the first frame update
    void Start()
    {
        StartFunc();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFunc();
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (beControl == false)
        {
            float distence = Vector2.Distance(transform.position, target.position);
            if (distence > 4)
            {
                if (isRight)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    anim.SetBool("Move", true);
                }
                else
                {
                    transform.Translate(-Vector2.right * speed * Time.deltaTime);
                    anim.SetBool("Move", true);
                }
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }
    }

    public override void Death(Vector2 bulletPos)
    {
        if((transform.position.x < bulletPos.x && isRight == true) 
            || (transform.position.x > bulletPos.x && isRight == false))
        base.Death(bulletPos);
    }*/
}
