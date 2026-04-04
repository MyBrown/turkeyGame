using Unity.VisualScripting;
using UnityEngine;

public class TurkeyMovement : MonoBehaviour
{
    // Jump Settings
    public float jumpforce = 8f;
    public int maxExtraJumps = 1;
    private int extraJumpsRemaining;
    private bool jumpPressed;
    private bool isGrounded;

    // Layer Detection
    public LayerMask groundLayer;

    // Collider objects and j
    private Rigidbody2D turkeyRigidbody;
    private BoxCollider2D turkeyCollider;
    void Start()
    {
        // Assigns rigidbodies to the components 
        turkeyRigidbody = GetComponent<Rigidbody2D>();
        turkeyCollider = GetComponent<BoxCollider2D>();
        extraJumpsRemaining = maxExtraJumps;
    }

    void Update()
    {
        // Checks for input 
        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
    {
        GameManager.Instance.TriggerGameOver(); 
        Destroy(gameObject);
    }
    }

    void FixedUpdate()
    {
        bool isGrounded = IsStandingOnLayer(groundLayer);

        if (isGrounded)
        {
            extraJumpsRemaining = maxExtraJumps;    
        }

        if (!jumpPressed)
        {
        return;    
        } 

        Vector2 v = turkeyRigidbody.linearVelocity;

        if (isGrounded)
        {
            v.y = jumpforce;
        }
        else if (extraJumpsRemaining > 0)
        {
            v.y = jumpforce;
            extraJumpsRemaining--;
        }

        turkeyRigidbody.linearVelocity = v;
        jumpPressed = false;
    }

    private bool IsStandingOnLayer(LayerMask mask)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = mask;
        filter.useTriggers = false;

        Collider2D[] results = new Collider2D[8];
        int count = turkeyCollider.Overlap(filter, results);
        return count > 0;
    }
}