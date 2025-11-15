using UnityEngine;
using UnityEngine.InputSystem;

public class TurkeyMovement : MonoBehaviour
{
    public float jumpforce;
    private float extraJump;
    private bool isGrounded;
    private Rigidbody2D turkeyRigidbody;
    private BoxCollider2D turkeyCollider;
    public BoxCollider2D floorCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turkeyRigidbody = GetComponent<Rigidbody2D>();
        turkeyCollider = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (turkeyCollider.IsTouching(floorCollider))
        {
            extraJump = 1;
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && turkeyCollider.IsTouching(floorCollider))
        {
            turkeyRigidbody.linearVelocity = Vector2.up * jumpforce;
        }    
        else if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
        {
            turkeyRigidbody.linearVelocity = Vector2.up * jumpforce;
            extraJump--;
        }
    }
}
