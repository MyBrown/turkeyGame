using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed;

    void Start()
    {
        speed = 3;
    }
    
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        speed = speed + 0.00001f;
    }
}
