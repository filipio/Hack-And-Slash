using UnityEngine;

public class HealOnHit : MonoBehaviour
{
    [SerializeField]
    private int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        var character = other.GetComponent<Character>();
        if (character != null)
        {
            character.Heal(healAmount);
            gameObject.SetActive(false);

        }
    }

}
