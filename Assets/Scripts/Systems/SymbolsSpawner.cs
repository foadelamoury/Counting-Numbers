using UnityEngine;

public class SymbolsSpawner : MonoBehaviour
{
    [Header("Symbols to Spawn")]
    [Tooltip("List of Symbol Scriptable Objects")]
    public SymbolSO[] constructiveSymbolsToSpawn; // addition and multiplication symbols

    [Header("Symbols to Spawn")]
    [Tooltip("List of Symbol Scriptable Objects")]
    public SymbolSO[] ReductiveSymbolsToSpawn;  // subtraction and division symbols

    [Header("Spawn Settings")]
    public float spawnInterval = 2.5f; // Symbols spawn a bit slower
    public float spawnWidth = 8f;

    private bool resultBiggerThan;

    public PlayerEquation playerEquation;

    void Awake()
    {
       playerEquation.OncurrentResultUpdated += CheckCurrentResult;
    }
    

    private void CheckCurrentResult(bool isGreater)
    {
        resultBiggerThan = isGreater;
        Debug.Log("Current result is " + (resultBiggerThan ? "greater" : "less") + " than the target answer.");
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSymbol), 1f, spawnInterval);
    }

    private void SpawnSymbol()
    {
SymbolSO randomSymSO;

        if(resultBiggerThan)
        {
             randomSymSO = ReductiveSymbolsToSpawn[Random.Range(0, ReductiveSymbolsToSpawn.Length)];
        }
        else
        {
            randomSymSO = constructiveSymbolsToSpawn[Random.Range(0, constructiveSymbolsToSpawn.Length)];
        }

        if (randomSymSO != null && randomSymSO.symbolGO != null)
        {
            Vector3 spawnPosition = new Vector3(
                transform.position.x + Random.Range(-spawnWidth, spawnWidth), 
                transform.position.y, 
                transform.position.z
            );

            GameObject clone = Instantiate(randomSymSO.symbolGO, spawnPosition, Quaternion.identity);
            // Destroys the clone automatically after 7 seconds
            Destroy(clone, 7f);
        }
    }
}
