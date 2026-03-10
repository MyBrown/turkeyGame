using UnityEngine;
public class TurkeyMovement : MonoBehaviour
{
    public float jumpforce;
    private float extraJump;
    public LayerMask groundLayer;
    private Rigidbody2D turkeyRigidbody;
    private BoxCollider2D turkeyCollider;
    public BoxCollider2D floorCollider;
    public BoxCollider2D shelfCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turkeyRigidbody = GetComponent<Rigidbody2D>();
        turkeyCollider = GetComponent<BoxCollider2D>();
        
    }
    private bool isTouchingShelf = false;

    void Update()
    {
        bool touchingFloor = turkeyCollider.IsTouching(floorCollider);

        if (touchingFloor || isTouchingShelf)
        {
            extraJump = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (touchingFloor)
            {
                // Normal full jump from floor
                turkeyRigidbody.linearVelocity = UnityEngine.Vector2.up * jumpforce;
            }
            else if (isTouchingShelf)
            {
                // Half force jump from shelf
                turkeyRigidbody.linearVelocity = UnityEngine.Vector2.up * (jumpforce / 2);
                extraJump = 0;
            }
            else if (extraJump > 0)
            {
                // Double jump in air
                turkeyRigidbody.linearVelocity = UnityEngine.Vector2.up * jumpforce;
                extraJump--;
            }
        }
    }
}