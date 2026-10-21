using UnityEngine;

public class SpinningNumbersOnDisplay : MonoBehaviour
{
    [Header("Number Setup")]
    public Transform[] numbers;     // Drag your 0 through 9 GameObjects here
    public float spinSpeed = 5f;    // How fast the numbers scroll upwards

    public bool isSpinning = true;

    private BoxCollider2D boxCollider2D;
    private float topBound;
    private float bottomBound;

    private void Start()
    {
        // 1. Get the 2D Collider
        boxCollider2D = GetComponent<BoxCollider2D>();

        // 2. Calculate bounds in LOCAL space using the collider's offset and size
        topBound = boxCollider2D.offset.y + (boxCollider2D.size.y / 2f);
        bottomBound = boxCollider2D.offset.y - (boxCollider2D.size.y / 2f);
    }

    void Update()
    {
        if (!isSpinning) return;

        for (int i = 0; i < numbers.Length; i++)
        {
            // Move each number upwards
            numbers[i].localPosition += Vector3.up * spinSpeed * Time.deltaTime;

            // If the number goes higher than the top boundary, wrap it to the bottom
            if (numbers[i].localPosition.y >= topBound)
            {
                // Calculate any extra distance it traveled past the boundary to prevent jitter
                float overshoot = numbers[i].localPosition.y - topBound;

                // Teleport the number back to the bottom
                numbers[i].localPosition = new Vector3(
                    numbers[i].localPosition.x,
                    bottomBound + overshoot,
                    numbers[i].localPosition.z
                );
            }
        }
    }

    public void StopSpinning()
    {
        isSpinning = false;
    }
}