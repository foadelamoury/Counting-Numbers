using UnityEngine;

public class CollectibleSymbol : MonoBehaviour
{
    [Tooltip("The Scriptable Object data for this symbol.")]
    public SymbolSO symbolData;

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
        if (col.CompareTag("Player"))
        {
            if (PlayerEquation.Instance != null)
            {
                PlayerEquation.Instance.CollectSymbol(symbolData);
                Destroy(gameObject); 
            }
        }
    }
}
