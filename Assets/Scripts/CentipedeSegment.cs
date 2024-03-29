using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    public SpriteRenderer spriteRenderer {get; private set;}
    public Centipede centipede {get; set;}
    public CentipedeSegment ahead {get; set;}
    public CentipedeSegment behind {get; set;}
    public bool isHead => ahead == null;
    private Vector2 direction = Vector2.right + Vector2.down;
    private Vector2 targetPosition;
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;
    }
    private void Update() {
        if(isHead && Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            UpdateHeadSegment();
        }

        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, centipede.speed * Time.deltaTime);

        Vector2 movementDirection = (targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(movementDirection.y,movementDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Deg2Rad, Vector3.forward);
    }
    public void UpdateHeadSegment()
    {
        Vector2 gridPosition = GridPosition(transform.position);

        targetPosition = gridPosition;
        targetPosition.x += direction.x;

        if(Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, centipede.collisionMask))
        {
            direction.x = -direction.x;

            targetPosition.x = gridPosition.x;
            targetPosition.y = gridPosition.y + direction.y; 

            Bounds homeBounds = centipede.homeArea.bounds;

            // Reverse vertical direction if the segment leaves the home area
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) ||
                (direction.y == -1f && targetPosition.y < homeBounds.min.y))
            {
                direction.y = -direction.y;
                targetPosition.y = gridPosition.y + direction.y;
            }
        }

        if(behind != null){
            behind.UpdateBodySegment();
        }
    }
    private void UpdateBodySegment()
    {
        targetPosition = GridPosition(ahead.transform.position);
        direction = ahead.direction;

        if(behind != null){
            behind.UpdateBodySegment();
        }
    }
    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance.ResetRound();
        }
        if(other.collider.enabled && other.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            other.collider.enabled = false;
            centipede.Remove(this);
        }
    }
}
