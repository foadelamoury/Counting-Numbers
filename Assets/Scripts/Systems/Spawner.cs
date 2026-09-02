using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Items to Spawn")]
    [Tooltip("List of Number Scriptable Objects")]
    public NumbersSO[] numbersToSpawn;
    
    [Tooltip("List of Symbol Scriptable Objects")]
    public SymbolSO[] symbolsToSpawn;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnWidth = 8f; // Random X range between -spawnWidth and +spawnWidth

    private void Start()
    {
        // Start spawning items repeatedly
        InvokeRepeating(nameof(SpawnRandomItem), 1f, spawnInterval);
    }

    private void SpawnRandomItem()
    {
        // 70% chance to spawn a number, 30% chance to spawn a symbol
        bool spawnNumber = Random.value <= 0.7f;
        
        GameObject prefabToSpawn = null;

        if (spawnNumber && numbersToSpawn.Length > 0)
        {
            NumbersSO randomNumSO = numbersToSpawn[Random.Range(0, numbersToSpawn.Length)];
            prefabToSpawn = randomNumSO.numberGO;
        }
        else if (symbolsToSpawn.Length > 0)
        {
            SymbolSO randomSymSO = symbolsToSpawn[Random.Range(0, symbolsToSpawn.Length)];
            prefabToSpawn = randomSymSO.symbolGO;
        }

        // Only spawn if we found a valid prefab
        if (prefabToSpawn != null)
        {
            Vector3 spawnPosition = new Vector3(
                transform.position.x + Random.Range(-spawnWidth, spawnWidth), 
                transform.position.y, 
                transform.position.z
            );

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
