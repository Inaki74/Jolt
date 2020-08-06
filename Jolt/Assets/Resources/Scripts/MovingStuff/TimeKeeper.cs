using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _endPoint;
    [SerializeField]
    private float _speed = 10.0f;

    private Vector2 _currentDirection;

    private Rigidbody2D Rb;

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.gravityScale = 0f;
        _currentDirection = Vector2.right;
    }

    private void Update()
    {
        if (transform.position.x > _endPoint.position.x)
        {
            _currentDirection = Vector2.left;
        }

        if(transform.position.x < _startPoint.position.x)
        {
            _currentDirection = Vector2.right;
        }

        Vector2 decidedVector = _currentDirection * _speed;

        //if (Rb.velocity.x != decidedVector.x)
            Rb.velocity = decidedVector;
    }
}
