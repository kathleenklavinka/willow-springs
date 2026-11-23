using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 1.5f;

    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x >= startPos.x + moveDistance)
            {
                movingRight = false;
                Flip(); // panggil fungsi flip
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x <= startPos.x - moveDistance)
            {
                movingRight = true;
                Flip(); // panggil fungsi flip
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;        // ubah arah skala X
        transform.localScale = scale;
    }
}
