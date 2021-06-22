using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionMarkerUI : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Image markerImage;

    [SerializeField]
    private Image lockImage;

    private CharacterSelectionMenuUI menu;
    private bool initializing;
    private bool initialized;

    public bool IsLockIn { get; private set; }
    public bool IsPlayerIn { get { return player.HasController; } }

    private void Awake()
    {
        menu = GetComponentInParent<CharacterSelectionMenuUI>();
        markerImage.gameObject.SetActive(false);
        lockImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsPlayerIn == false)
            return;
        if (!initializing)
            StartCoroutine(Initialize());

        if (!initialized)
            return;

        if(!IsLockIn)
        {
            if (player.Controller.horizontal > 0.5f)
            {
                MoveToCharacterPanel(menu.RightPanel);
            }
            else if (player.Controller.horizontal < -0.5f)
            {
                MoveToCharacterPanel(menu.LeftPanel);
            }
            if (player.Controller.attackPressed)
            {
                StartCoroutine(LockCharacter());
            }
        }
        else
        {
            if(player.Controller.attackPressed)
            {
                menu.TryStartGame();
            }
        }
        //check for player controlls + selection + locking the character
    }

    private IEnumerator LockCharacter()
    {
        lockImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        IsLockIn = true;
       // markerImage.gameObject.SetActive(false);
    }


    private void MoveToCharacterPanel(CharacterSelectionPanelUI panel)
    {
        transform.position = panel.transform.position;
        player.CharacterPrefab = panel.CharacterPrefab;
    }

    private IEnumerator Initialize()
    {
        initializing = true;
        MoveToCharacterPanel(menu.LeftPanel);

        //waiting to avoid launching this twice if the player hits the button once and update detects it 2 times
        yield return new WaitForSeconds(0.5f);
        markerImage.gameObject.SetActive(true);
        initialized = true; 
    }
}
