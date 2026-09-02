using UnityEngine;

public class BounceAndDestroy : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    


    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision   )
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
        Invoke(nameof(SetTriggerTrue), 1f);
        }
    }
        
    void SetTriggerTrue()
    {
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Ground"))
        {
        Destroy(gameObject);
        }    
    }
}
