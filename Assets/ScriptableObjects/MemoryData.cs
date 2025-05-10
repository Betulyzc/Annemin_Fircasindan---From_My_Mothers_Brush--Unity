using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryData", menuName = "Scriptable Objects/MemoryData")]
public class MemoryData : ScriptableObject
{
    public List<MemoryEntry> entries;
}

[System.Serializable]
public class MemoryEntry
{
    public Sprite image;
    [TextArea] public string caption;
}
