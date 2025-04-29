using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handZone;
    public Transform attackZone;
    public Transform defenseZone;

    public int mana = 0;
    public int maxMana = 10;
    public int hp = 30;

    private List<GameObject> hand = new List<GameObject>();
    private int maxHandSize = 5;
    public static EnemyController Instance;

    private bool hasAttackedThisTurn = false; // Добавили для контроля одной атаки за ход

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartTurn()
    {
        Debug.Log("Враг начинает ход!");

        hasAttackedThisTurn = false; // Сбрасываем перед началом хода
        mana = Mathf.Min(mana + 2, maxMana);

        DrawCards(5 - hand.Count);

        Invoke(nameof(PlayCards), 1f);
    }

    private void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject card = Instantiate(cardPrefab, handZone.position, Quaternion.identity, handZone);
            card.GetComponent<CardData>().GenerateRandomStats();
            hand.Add(card);
        }
    }

    private void PlayCards()
    {
        List<GameObject> cardsToPlay = new List<GameObject>();

        foreach (GameObject cardObj in hand)
        {
            CardData card = cardObj.GetComponent<CardData>();
            if (card.manaCost <= mana)
            {
                Transform zone = Random.value > 0.5f ? attackZone : defenseZone;
                cardObj.transform.SetParent(zone);
                //card.owner = Owner.Enemy; // 👈 (если у тебя есть разделение владельцев!)

                mana -= card.manaCost;
                cardsToPlay.Add(cardObj);
            }
        }

        foreach (var playedCard in cardsToPlay)
            hand.Remove(playedCard);

        // После размещения — красиво расставить карты
        ArrangeCards(attackZone);
        ArrangeCards(defenseZone);

        Invoke(nameof(AttackPlayer), 1f);
    }

    private void ArrangeCards(Transform zone)
    {
        int count = zone.childCount;
        float spacing = 0.15f; // расстояние между картами
        float startX = -(spacing * (count - 1)) / 2f; // начальная точка для выравнивания по центру

        for (int i = 0; i < count; i++)
        {
            Transform card = zone.GetChild(i);
            Vector3 targetPos = new Vector3(0, 0, startX + spacing * i);
            card.localPosition = targetPos;
        }
    }

    private void AttackPlayer()
    {
        if (hasAttackedThisTurn)
        {
            Debug.Log("Враг уже атаковал в этом ходу!");
            Invoke(nameof(EndTurn), 1f);
            return;
        }

        var player = GameManager.Instance.player;

        foreach (Transform enemyCardTransform in attackZone)
        {
            var attacker = enemyCardTransform.GetComponent<CardData>();
            if (attacker != null && !attacker.hasAttackedThisTurn)
            {
                CardData target = null;

                // Ищем защитную карту у игрока
                foreach (Transform def in player.defenseZone)
                {
                    target = def.GetComponent<CardData>();
                    if (target != null)
                        break;
                }

                if (target != null)
                {
                    Debug.Log($"Враг {attacker.name} атакует защитную карту {target.name}");
                    AttackController.Instance.PerformAttack(attacker, target);
                }
                else
                {
                    Debug.Log($"Враг {attacker.name} бьет игрока на {attacker.attack} урона!");
                    player.TakeDamage(attacker.attack);
                }

                attacker.hasAttackedThisTurn = true;
                hasAttackedThisTurn = true; // Запоминаем что враг атаковал
                break; // После одной атаки выходим
            }
        }

        Invoke(nameof(EndTurn), 1f);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log("Игрок победил!");
        }
    }

    private void EndTurn()
    {
        GameManager.Instance.EndEnemyTurn();
        Debug.Log("Враг закончил ход.");
    }
}