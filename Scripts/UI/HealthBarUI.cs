using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;

    private Character currentCharacter;
    private float maxFill = 1f;

    private void Awake()
    {
        var player = GetComponentInParent<Player>();
        player.OnCharacterChanged += Player_OnCharacterChanged;
        gameObject.SetActive(false);
        
    }

    private void Player_OnCharacterChanged(Character character)
    {
        currentCharacter = character;
        currentCharacter.OnHealthChanged += HandleHealthChanged;
        currentCharacter.OnDied += CurrentCharacter_OnDied;
        gameObject.SetActive(true);
    }

    private void CurrentCharacter_OnDied(IDie character)
    {
        character.OnHealthChanged -= HandleHealthChanged;
        character.OnDied -= CurrentCharacter_OnDied;
        foregroundImage.fillAmount = maxFill;
        gameObject.SetActive(false);
        currentCharacter = null;

    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        float percent = (float)currentHealth / (float)maxHealth;
        foregroundImage.fillAmount = percent;
    }
}
