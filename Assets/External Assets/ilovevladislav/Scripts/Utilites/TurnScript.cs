using UnityEngine;


    public class TurnScript : MonoBehaviour
    {
        [Tooltip("Rotation speed in degrees per second.")]
        public float rotationSpeed = 90f;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Set the angular velocity for proper physics rotation
                rb.angularVelocity = rotationSpeed;
            }
        }

        private void Update()
        {
            // Fallback for non-rigidbody objects
            if (rb == null)
            {
                transform.Rotate(new Vector3(0, 0, 1) * (rotationSpeed * Time.deltaTime));
            }
        }
    }

