using UnityEngine;

public class Blaster : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Vector2 direction;
    Vector2 spawnPosition;
    public int speed = 20;
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }
    private void Update() {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
    }
    private void FixedUpdate() {
        Vector2 position = rigidbody.position;
        position += direction.normalized * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);
    }
    public void Respawn()
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}
