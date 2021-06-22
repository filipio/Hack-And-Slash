using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;

    public event Action<Character> OnCharacterChanged = delegate { };

    public Controller Controller { get; private set; }
    private PlayerTextUI uiPlayerText;
    private float respawnDelay = 5f;

    public int PlayerNumber {get {return playerNumber;}}
    public bool HasController { get { return Controller != null; } }

    public Character CharacterPrefab { get; set; }

    private void Awake()
    {
        uiPlayerText = GetComponentInChildren<PlayerTextUI>();
    }

    public void InitializePlayer(Controller controller)
    {
        Controller = controller;

        gameObject.name = string.Format("Player {0} - {1}", playerNumber, controller.gameObject.name);

        uiPlayerText.HandlePlayerInitialized(playerNumber);
    }

    public void SpawnCharacter()
    {
        var character = CharacterPrefab.Get<Character>(Vector3.zero, Quaternion.identity);
        character.SetController(Controller);
        character.OnDied += Character_OnDied;
        OnCharacterChanged(character);
    }

    private void Character_OnDied(IDie character)
    {
        
        character.OnDied -= Character_OnDied;
        character.gameObject.SetActive(false);
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnCharacter();
    }
}
