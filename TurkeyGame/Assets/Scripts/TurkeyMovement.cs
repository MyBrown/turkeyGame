using UnityEngine;
using UnityEngine.InputSystem;

public class TurkeyMovement : MonoBehaviour
{
    public float jumpforce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpforce);
        }
        
    }
}
