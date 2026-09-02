using UnityEngine;


[CreateAssetMenu(fileName = "SymbolSO", menuName = "Calculations/SymbolSO", order = 1)]

public class SymbolSO : ScriptableObject
{
  public string Name;

  public string operation;
  public GameObject symbolGO;
}
