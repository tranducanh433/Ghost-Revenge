using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    Transform target;
    float size = 1;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, 10f * Time.deltaTime);

            size -= Time.deltaTime * 4f;
            transform.localScale = new Vector3(size, size, 1);

            if(size <= 0.1) Destroy(gameObject);
        }
    }
}
