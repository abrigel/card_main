using UnityEngine;

public class Card : MonoBehaviour
{
    public int rank; // Ранг карты

    public void AssignRandomRank()
    {
        rank = Random.Range(2, 10); // Ранг от 2 до 9
        gameObject.name = "Card " + rank;
        UpdateCardVisual();
    }

    private void UpdateCardVisual()
    {
        TextMesh text = GetComponentInChildren<TextMesh>(); // Находим текст на карте
        if (text != null)
        {
            text.text = rank.ToString(); // Отображаем ранг
        }
    }
}
