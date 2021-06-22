using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void Begin()
    {
        StartCoroutine(BeginGame());
    }

    private IEnumerator BeginGame()
    {
        //??
        // thanks to that we can check if an operation ended -> next lines
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (operation.isDone == false)
            yield return null;
        PlayerManager.Instance.SpawnPlayerCharacters();
        
    }
}
