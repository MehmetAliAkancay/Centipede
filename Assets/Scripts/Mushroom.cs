using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public Sprite[] states;
    private SpriteRenderer spriteRenderer;
    private int health;
    public int point = 1;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = states.Length;
    }
    public void Heal()
    {
        health = states.Length;
        spriteRenderer.sprite = states[0];
    }
    private void TakeDamage(int amaount)
    {
        health -= amaount;

        if(health > 0)
        {
            spriteRenderer.sprite = states[states.Length - health];
        }
        else
        {
            Destroy(gameObject);
            GameManager.instance.IncreaseScore(point);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            TakeDamage(1);
        }
    }
}
