using UnityEngine;

public class TurkeyMovement : MonoBehaviour
{
public Sprite aliveTurkey;
public Sprite cookedTurkey;

// Jump Settings
public float jumpforce = 8f;
public int maxExtraJumps = 1;
private int extraJumpsRemaining;
private bool jumpPressed;
private bool isGrounded;
private bool isDead = false;

// Layer Detection
public LayerMask groundLayer;
// Collider objects and j
private Rigidbody2D turkeyRigidbody;
private BoxCollider2D turkeyCollider;
private SpriteRenderer spriteRenderer;
private Animator turkeyAnimator;
void Start()
{
    // Assigns rigidbodies to the components 
    turkeyRigidbody = GetComponent<Rigidbody2D>();
    turkeyCollider = GetComponent<BoxCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    turkeyAnimator = GetComponent<Animator>();
    extraJumpsRemaining = maxExtraJumps;
}

void Update()
{
    // Checks for input 
    if (isDead) return;
    if (Input.GetKeyDown(KeyCode.Space))
        jumpPressed = true;
}

private void OnCollisionEnter2D(Collision2D collision)
{
    if (isDead) return;
    Debug.Log("Turkey hit: " + collision.gameObject.tag);
    if (collision.gameObject.CompareTag("Obstacle"))
    {
        Debug.Log("Obstacle hit! Calling GameOver...");
        isDead = true;
        Time.timeScale = 0f; 
        GameManager.Instance.TriggerGameOver();
        Destroy(gameObject);
    }
    if (collision.gameObject.CompareTag("Fire")) // ← NEW fire death
        {
            Debug.Log("Fire hit! Showing cooked sprite...");
            Time.timeScale = 0f;
            isDead = true;
            StartCoroutine(FireDeathSequence());
        }
}

private System.Collections.IEnumerator FireDeathSequence()
    {
        turkeyRigidbody.linearVelocity = Vector2.zero;
        turkeyRigidbody.gravityScale = 0f;
        turkeyAnimator.enabled = false;  
        if (turkeyAnimator != null)
        turkeyAnimator.enabled = false;
        if (cookedTurkey != null)
        spriteRenderer.sprite = cookedTurkey;
            else
            Debug.LogError("cookedTurkey sprite is not assigned!");
        yield return new WaitForSecondsRealtime(2f);    
        GameManager.Instance.TriggerGameOver();
        Destroy(gameObject);
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