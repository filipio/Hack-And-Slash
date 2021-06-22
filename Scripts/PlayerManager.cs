using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player[] players;
    public static PlayerManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        players = FindObjectsOfType<Player>();
    }
    public void AddPlayerToGame(Controller controller)
    {
        var firstNoneActivePlayer = players
            .OrderBy(t => t.PlayerNumber)
            .FirstOrDefault(t => t.HasController == false);

        firstNoneActivePlayer.InitializePlayer(controller);
    }

    public void SpawnPlayerCharacters()
    {
        foreach(var player in players)
        {
            if(player.HasController && player.CharacterPrefab != null)
            {
                player.SpawnCharacter();
            }
        }
    }
}
