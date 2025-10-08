using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "ScriptableObjects/Achievement")]
public class MemoryAchievement : ScriptableObject
{
    [field: SerializeField] public Memory Memory { get; private set; }
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    public string UniqueTag { get; private set; }


    public void SetUniqueTag(string uniqueTag = "NULL")
    {
        if (uniqueTag != "NULL")
            UniqueTag = uniqueTag;
    }
}