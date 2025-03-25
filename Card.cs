using UnityEngine;

public class Card : MonoBehaviour
{
    public int rank; // Ранг карты

    public void AssignRandomRank()
    {
        rank = Random.Range(2, 10);
        gameObject.name = "Card " + rank;
        UpdateCardVisual();
    }

    private void UpdateCardVisual()
    {
        TextMesh text = GetComponentInChildren<TextMesh>();
        if (text != null)
        {
            text.text = rank.ToString();
        }
    }
}
