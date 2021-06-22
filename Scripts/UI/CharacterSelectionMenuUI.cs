using TMPro;
using UnityEngine;

public class CharacterSelectionMenuUI : MonoBehaviour
{
    [SerializeField]
    private CharacterSelectionPanelUI leftPanel;

    [SerializeField]
    private CharacterSelectionPanelUI rightPanel;

    [SerializeField]
    TextMeshProUGUI startGameText;

    private CharacterSelectionMarkerUI[] markers;
    private bool startEnabled;

    public CharacterSelectionPanelUI LeftPanel { get { return leftPanel; } }
    public CharacterSelectionPanelUI RightPanel { get { return rightPanel; } }

    private void Awake()
    {
        markers = GetComponentsInChildren<CharacterSelectionMarkerUI>();
    }

    private void Update()
    {
        int playerCount = 0;
        int lockedCount = 0;
        foreach(var marker in markers)
        {
            if (marker.IsPlayerIn)
                playerCount++;
            if (marker.IsLockIn)
                lockedCount++;
        }

        startEnabled = playerCount > 0 && playerCount == lockedCount;
        startGameText.gameObject.SetActive(startEnabled);

        
    }

    public void TryStartGame()
    {
        if(startEnabled)
        {
            GameManager.Instance.Begin();
            gameObject.SetActive(false);
        }
    }
}
