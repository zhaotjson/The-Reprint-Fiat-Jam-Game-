using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded()) {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                rb.AddForce(new Vector2(-1, 1) * speed, ForceMode2D.Impulse);
                renderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                rb.AddForce(new Vector2(1, 1) * speed, ForceMode2D.Impulse);
                renderer.flipX = false;
            }
        }
    }

    bool IsGrounded()
    {
        return rb.velocity.y == 0;
    }
}
