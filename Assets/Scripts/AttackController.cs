using UnityEngine;

public class AttackController : MonoBehaviour
{
    public static AttackController Instance;

    private CardData selectedAttacker;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartTurn()
    {
        selectedAttacker = null;
        // Не нужен глобальный флаг, каждая карта сбрасывает его сама при старте хода игрока/врага
    }

    public void SelectAttacker(CardData attacker)
    {
        if (attacker == null || attacker.hasAttackedThisTurn)
            return;

        selectedAttacker = attacker;
        Debug.Log("Selected attacker: " + attacker.name);
    }

    public void SelectTarget(CardData target)
    {
        if (selectedAttacker == null || target == null)
            return;

        PerformAttack(selectedAttacker, target);
    }

    public void AttackEnemyDirectly()
    {
        if (selectedAttacker == null)
            return;

        if (GameManager.Instance.enemy.defenseZone.childCount > 0)
        {
            Debug.Log("There are defense cards. First destroy them!");
            return;
        }

        Debug.Log("Attacking enemy directly!");
        GameManager.Instance.enemy.TakeDamage(selectedAttacker.attack);
        selectedAttacker.hasAttackedThisTurn = true;
        selectedAttacker.SetSelected(false);
        selectedAttacker = null;
    }

    public bool PerformAttack(CardData attacker, CardData target)
    {
        if (attacker == null || target == null) return false;

        Debug.Log($"{attacker.name} attacks {target.name}");

        target.TakeDamage(attacker.attack);
        attacker.TakeDamage(target.attack);

        attacker.hasAttackedThisTurn = true;
        attacker.SetSelected(false);
        selectedAttacker = null;

        return attacker.health > 0;
    }
}
