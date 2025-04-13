using UnityEngine;

/// <summary>
/// Представляет карточку в игре.
/// </summary>
public class Card : MonoBehaviour
{
    /// <summary>
    /// Ранг карты (значение от 2 до 9).
    /// </summary>
    public int rank; // Ранг карты
    public int manaCost = 1; // Стоимость карты по мана

    /// <summary>
    /// Присваивает карте случайный ранг в диапазоне от 2 до 9 и обновляет её визуальное представление.
    /// </summary>  
    public void AssignRandomRank()
    {
        rank = Random.Range(2, 10);
        gameObject.name = "Card " + rank;
        UpdateCardVisual();
    }

    /// <summary>
    /// Обновляет визуальное представление карты, изменяя текстовое отображение ранга.
    /// </summary>
    private void UpdateCardVisual()
    {
        TextMesh text = GetComponentInChildren<TextMesh>();
        if (text != null)
        {
            text.text = rank.ToString();
        }
    }
}
