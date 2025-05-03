using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    private CardData cardData;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardData = GetComponent<CardData>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        startPosition = transform.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var gm = GameManager.Instance;

            var targetCard = hit.collider.GetComponent<CardData>();
            if (cardData.isPlaced && targetCard != null && targetCard.owner == CardData.Owner.Enemy)
            {
                if (cardData.transform.parent == gm.player.defenseZone)
                {
                    Debug.Log("Карта в зоне защиты и не может атаковать");
                    ReturnCard();
                    return;
                }

                if (cardData.hasAttackedThisTurn)
                {
                    Debug.Log("Эта карта уже атаковала в этом ходу");
                    ReturnCard();
                    return;
                }

                bool survived = AttackController.Instance.PerformAttack(cardData, targetCard);
                if (survived)
                {
                    ReturnCard();
                }
                return;
            }

            var zone = hit.collider.GetComponent<ZoneHandler>();
            if (zone != null && gm.IsPlayerTurn() && !cardData.isPlaced && gm.player.mana >= cardData.manaCost)
            {
                var oldZone = originalParent.GetComponent<ZoneHandler>();
                if (oldZone != null)
                {
                    oldZone.RemoveCard(cardData);
                }
                else
                {
                    Debug.LogWarning("originalParent не содержит ZoneHandler, возможно визуальный баг.");
                }

                zone.AddCard(cardData);
                cardData.isPlaced = true;

                gm.player.mana -= cardData.manaCost;
                gm.player.UpdatePlayerStats();

                // Показать информацию о карте в консоли
                ShowCardStatsInConsole();

                return;
            }
        }

        // Вернуть на исходную позицию, если не попали в зону
        ReturnCard();
    }

    private void ReturnCard()
    {
        transform.position = startPosition;
        transform.SetParent(originalParent, false);
    }

    // Метод для вывода в консоль силы атаки и здоровья карты
    private void ShowCardStatsInConsole()
    {
        Debug.Log($"Карта {cardData.name}: Атака = {cardData.attack}, Здоровье = {cardData.health}");
    }
}

/*
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    private CardData cardData;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardData = GetComponent<CardData>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        startPosition = transform.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var gm = GameManager.Instance;

            var targetCard = hit.collider.GetComponent<CardData>();
            if (cardData.isPlaced && targetCard != null && targetCard.owner == CardData.Owner.Enemy)
            {
                if (cardData.transform.parent == gm.player.defenseZone)
                {
                    Debug.Log("Карта в зоне защиты и не может атаковать");
                    ReturnCard();
                    return;
                }

                if (cardData.hasAttackedThisTurn)
                {
                    Debug.Log("Эта карта уже атаковала в этом ходу");
                    ReturnCard();
                    return;
                }

                bool survived = AttackController.Instance.PerformAttack(cardData, targetCard);
                if (survived)
                {
                    ReturnCard();
                }
                return;
            }

            var zone = hit.collider.GetComponent<ZoneHandler>();
            if (zone != null && gm.IsPlayerTurn() && !cardData.isPlaced && gm.player.mana >= cardData.manaCost)
            {
                var oldZone = originalParent.GetComponent<ZoneHandler>();
                if (oldZone != null)
                {
                    oldZone.RemoveCard(cardData);
                }
                else
                {
                    Debug.LogWarning("originalParent не содержит ZoneHandler, возможно визуальный баг.");
                }

                zone.AddCard(cardData);
                cardData.isPlaced = true;

                gm.player.mana -= cardData.manaCost;
                gm.player.UpdatePlayerStats();
                return;
            }
        }

        // Вернуть на исходную позицию, если не попали в зону
        ReturnCard();
    }

    private void ReturnCard()
    {
        transform.position = startPosition;
        transform.SetParent(originalParent, false);
    }
}
*/