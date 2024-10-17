using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead : MonoBehaviour
{
    public float speed;
    public int startingPoints;
    public Transform[] points;
    public GameObject[] pointObj;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var o in pointObj)
        {
            o.transform.SetParent(null);
        }

        transform.position = points[startingPoints].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[index].position) < 0.2f)
        {
            index++;
            if (index == points.Length)
            {
                index = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[index].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        other.transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointObj[0].transform.position, 0.2f);
            Gizmos.DrawWireSphere(pointObj[1].transform.position, 0.2f);
            Gizmos.DrawLine(pointObj[0].transform.position, pointObj[1].transform.position);
        }
    }
}