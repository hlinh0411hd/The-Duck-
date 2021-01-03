using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float maxSpeed;
    public float stopSpeed;

    public float heightJump;

    private float hor;
    private float dir;
    private int numJump = 0;
    private int maxNumJump = 2;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;
    [SerializeField] private LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if(h > 0) {
            if(dir == -1) {
                hor = -hor;
                transform.localScale = new Vector2(-1, 1);
            }
            dir = 1;
        }
        if(h < 0) {
            if(dir == 1) {
                hor = -hor;
                transform.localScale = new Vector2(1, 1);
            }
            dir = -1;
        }
        if (h == 0) {
            hor -= Time.deltaTime * speed * stopSpeed * (IsGrounded()? 1:2);
            if (hor < 0) {
                hor = 0;
            }
        } else {
            hor += Time.deltaTime * speed;
            if (hor > maxSpeed) {
                hor = maxSpeed;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || (!IsGrounded() && numJump < maxNumJump))) {
            Jump();
        }
    }

    private void FixedUpdate() {
        Vector2 pos = transform.position;
        pos.x += hor * dir;
        transform.position = pos;
    }

    private void Jump() {
        if (rb != null) {
            if(IsGrounded()) {
                numJump = 0;
            }
            numJump += 1;
            rb.velocity = Vector2.up * heightJump;
        }
    }

    private bool IsGrounded() {
        RaycastHit2D rc = Physics2D.CircleCast(circleCollider2D.bounds.center, circleCollider2D.radius, Vector2.down, 0.1f, platformLayerMask);
        return rc.collider != null;
    }

}
