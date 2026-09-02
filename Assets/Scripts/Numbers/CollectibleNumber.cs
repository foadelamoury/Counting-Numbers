using UnityEngine;

public class CollectibleNumber : MonoBehaviour
{
    [Tooltip("The Scriptable Object data for this number.")]
    public NumbersSO numberData;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckAndCollect(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckAndCollect(collision);
    }

    private void CheckAndCollect(Collider2D col)
    {
        // Use GetComponentInParent because the player's colliders might be on child objects!
        PlayerEquation equation = col.GetComponentInParent<PlayerEquation>();
        if (equation != null)
        {
            equation.CollectNumber(numberData);
            Destroy(gameObject); 
        }
    }
}
