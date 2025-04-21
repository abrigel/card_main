using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Если планируете использовать UI для кнопки завершения хода

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public enum PlayerTurn { Player1, Player2 }
    public PlayerTurn currentTurn = PlayerTurn.Player1;

    // Поля для маны у обоих игроков
    public int player1MaxMana = 1;
    public int player1CurrentMana = 1;
    public int player2MaxMana = 1;
    public int player2CurrentMana = 1;

    public float roundDelay = 1f; // Задержка между сменой раундов

    public Button endTurnButton; // Кнопка для завершения хода
    public Text manaText;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Подписка на событие кнопки "Закончить ход"
        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(EndTurn);

        // Начинаем первый раунд для первого игрока
        StartNewRound();
    }

    // Обновление текста с текущей маной
    private void UpdateManaUI()
    {
        if (manaText != null)
        {
            int currentMana = (currentTurn == PlayerTurn.Player1) ? player1CurrentMana : player2CurrentMana;
            manaText.text = "Мана: " + currentMana;
        }
    }

    // Начало нового раунда для текущего игрока
    public void StartNewRound()
    {
        if (currentTurn == PlayerTurn.Player1)
        {
            player1MaxMana++;
            player1CurrentMana = player1MaxMana;
            Debug.Log("Раунд игрока 1: Мана увеличена до " + player1CurrentMana);
        }
        else // Player2
        {
            player2MaxMana++;
            player2CurrentMana = player2MaxMana;
            Debug.Log("Раунд игрока 2: Мана увеличена до " + player2CurrentMana);
        }
        UpdateManaUI();
    }

    // Проверка, хватает ли текущей маны для совершения действия
    public bool HasEnoughMana(int amount)
    {
        return currentTurn == PlayerTurn.Player1 ?
            player1CurrentMana >= amount :
            player2CurrentMana >= amount;
    }

    // Расход маны текущим игроком
    public void SpendMana(int amount)
    {
        if (currentTurn == PlayerTurn.Player1)
        {
            player1CurrentMana -= amount;
            Debug.Log("Игрок 1 потратил " + amount + " маны. Осталось " + player1CurrentMana);
        }
        else
        {
            player2CurrentMana -= amount;
            Debug.Log("Игрок 2 потратил " + amount + " маны. Осталось " + player2CurrentMana);
        }
        UpdateManaUI();
    }

    // Завершение хода: переключение игрока и запуск нового раунда
    public void EndTurn()
    {
        Debug.Log("Ход завершён у " + (currentTurn == PlayerTurn.Player1 ? "Игрока 1" : "Игрока 2"));

        // Переключение активного игрока
        currentTurn = (currentTurn == PlayerTurn.Player1) ? PlayerTurn.Player2 : PlayerTurn.Player1;
        // Запуск нового раунда с заданной задержкой
        Invoke(nameof(StartNewRound), roundDelay);
        UpdateManaUI();
    }
}

