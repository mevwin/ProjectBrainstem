using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShrineLevelList", menuName = "Scriptable Objects/ShrineLevelList")]
public class ShrineLevelList : ScriptableObject
{
    [SerializeField] private List<string> list = new();
    

    public string GetLevelName(int index)
    {
        return list[index];
    }

    public int GetSize()
    {
        return list.Count;
    }
    
}
