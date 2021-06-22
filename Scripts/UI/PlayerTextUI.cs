using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerTextUI : MonoBehaviour
{
    private TextMeshProUGUI tmText;
    private Animator textAnimator;
    // Start is called before the first frame update

    private void Awake()
    {
        tmText = GetComponent<TextMeshProUGUI>();
        textAnimator = GetComponent<Animator>();
    }

    internal void HandlePlayerInitialized(int playerNumber)
    {
        tmText.text = string.Format("PLAYER {0} JOINED", playerNumber);
        textAnimator.SetTrigger("PlayerJoined");
        StartCoroutine(ClearTextAfterDelay());
    }

    IEnumerator ClearTextAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        tmText.text = string.Empty;
    }
}
