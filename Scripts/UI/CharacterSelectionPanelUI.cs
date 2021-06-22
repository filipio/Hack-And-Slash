using UnityEngine;

public class CharacterSelectionPanelUI : MonoBehaviour
{
    [SerializeField]
    private Character characterPrefab;

    public Character CharacterPrefab { get { return characterPrefab; } }

}
