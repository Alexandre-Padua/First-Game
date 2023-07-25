using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed;
    [HideInInspector] public Rigidbody2D rig;
    [HideInInspector] public Vector2 direction;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
    }

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
}
