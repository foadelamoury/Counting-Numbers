using UnityEngine;

public class NumbersSpawner : MonoBehaviour
{
    [Header("Items to Spawn")]
    [Tooltip("List of Number Scriptable Objects")]
    public NumbersSO[] numbersToSpawn;
    
    [Header("Spawn Settings")]
    public float spawnInterval = 1.0f; // Numbers spawn faster
    public float spawnWidth = 8f; 

    private void Start()
    {
        InvokeRepeating(nameof(SpawnNumber), 1f, spawnInterval);
    }

    private void SpawnNumber()
    {
        if (numbersToSpawn == null || numbersToSpawn.Length == 0) return;

        NumbersSO randomNumSO = numbersToSpawn[Random.Range(0, numbersToSpawn.Length)];
        if (randomNumSO != null && randomNumSO.numberGO != null)
        {
            Vector3 spawnPosition = new Vector3(
                transform.position.x + Random.Range(-spawnWidth, spawnWidth), 
                transform.position.y, 
                transform.position.z
            );

            Instantiate(randomNumSO.numberGO, spawnPosition, Quaternion.identity);
        }
    }
}
