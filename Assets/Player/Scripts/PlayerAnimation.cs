using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement playerMove;
    private Animator anim;

    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerMove.direction.sqrMagnitude > 0)
        {
            anim.SetInteger("transition", 1);
        }
        else
        {
            anim.SetInteger("transition", 0);
        }

        if (playerMove.direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (playerMove.direction.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }
}
