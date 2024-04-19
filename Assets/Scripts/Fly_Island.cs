using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Island : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 1f;
    public float Максимальная_высота = 10f;
    public float Минимальная_высота = 0f;

    // Start is called before the first frame update
    void Start()
    {
        direction.y = 1;
        Минимальная_высота = transform.position.y;
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        if (transform.position.y > Максимальная_высота) direction = direction * -1;
        if (transform.position.y<Минимальная_высота) direction=direction*-1;
    }
}
