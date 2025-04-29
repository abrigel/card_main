using UnityEngine;
using UnityEngine.EventSystems;

public class CardData : MonoBehaviour, IPointerClickHandler
{
    public enum Owner { Player, Enemy }
    public Owner owner;

    public int manaCost;
    public int attack;
    public int health;

    public bool isPlaced = false;
    public bool hasAttackedThisTurn = false;
    public bool isPlayerCard = true;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        var cardCanvas = gameObject.AddComponent<Canvas>();
        cardCanvas.overrideSorting = true;
        cardCanvas.sortingOrder = 10;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        owner = isPlayerCard ? Owner.Player : Owner.Enemy;
    }


    public void GenerateRandomStats()
    {
        manaCost = Random.Range(1, 5);
        attack = Random.Range(1, 5);
        health = Random.Range(2, 8);
    }

    public void SetSelected(bool selected)
    {
        if (rend == null) return;
        rend.material.color = selected ? Color.yellow : originalColor;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(name + " получил " + damage + " урона. Осталось HP: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log(name + " уничтожен.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsPlayerTurn())
            return;

    }
}
