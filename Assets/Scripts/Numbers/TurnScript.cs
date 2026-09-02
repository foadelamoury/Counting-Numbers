using UnityEngine;

namespace CountingNumbers
{
    
    public class TurnScript : MonoBehaviour
    {
        [Tooltip("Rotation speed in degrees per second.")]
        public float rotationSpeed = 90f;

        [Tooltip("Axis of rotation. For 2D games, this is usually the Z axis (0, 0, 1).")]
        public Vector3 rotationAxis = new Vector3(0, 0, 1);

        private void Update()
        {
            // Rotate the object around its center
            transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
        }
    }
}
