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


    public PlayerEquation playerEquation;

    private NumberState state;

    void Awake()
    {
        playerEquation.OncurrentResultUpdated += CheckCurrentResult;
    }


    private void CheckCurrentResult(NumberState state)
    {

        this.state = state;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSymbol), 1f, spawnInterval);
    }

    private void SpawnSymbol()
    {
        SymbolSO randomSymSO;

        if (state == NumberState.biggerThanTarget)
        {
            randomSymSO = ReductiveSymbolsToSpawn[Random.Range(0, ReductiveSymbolsToSpawn.Length)];
        }
        else if (state == NumberState.smallerThanTarget)
        {
            randomSymSO = constructiveSymbolsToSpawn[Random.Range(0, constructiveSymbolsToSpawn.Length)];
        }
        else if (state == NumberState.negative)
        {
            randomSymSO = constructiveSymbolsToSpawn[0];
        }
        else
        {
            // If the state is equal to the target, do not spawn any symbols
            return;
        }

        if (randomSymSO != null && randomSymSO.symbolGO != null)
        {
            Vector3 spawnPosition = new Vector3(
                transform.position.x + Random.Range(-spawnWidth, spawnWidth),
                transform.position.y,
                transform.position.z
            );

            GameObject clone = Instantiate(randomSymSO.symbolGO, spawnPosition, Quaternion.identity);
            Destroy(clone, 7f);
        }
    }
}
