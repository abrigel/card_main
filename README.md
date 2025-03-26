# 3D Card Game

## Описание
Проект представляет собой карточную 3D-игру, разработанную в Unity с использованием C#.
Игроки могут взаимодействовать с картами в виртуальном пространстве, используя основные механики, реализованные в скриптах.

## Требования
- Unity 2021.3 или новее
- .NET Framework 4.7.1+
- Git для работы с репозиторием

## Установка
1. Клонируйте репозиторий:
   ```sh
   git clone https://github.com/username/3D-Card-Game.git
   ```
2. Откройте проект в Unity.
3. Соберите и запустите сцену **MainScene**.

## Основные скрипты
- **CardSelector.cs** - отвечает за выбор, перемещение и размещение карт в игровых зонах, контролируя их положение и ограничения.
- **Player.cs** - управляет картами в руке игрока, добавляя их, распределяя в сетке и корректируя их положение и поворот.
- **Card.cs** отвечает за назначение случайного ранга карте и обновление её визуального отображения.
- **DeckManager.cs** - отвечает за создание и раздачу карт из колоды в руку игрока, контролируя их количество и расположение.
- **ZoneHandler.cs** - позволяет размещать выбранную карту в зону при клике, вызывая соответствующий метод у CardSelector.

## Видеодемонстрация
Ссылка на демонстрацию работы игры: [Видео](https://drive.google.com/drive/folders/1b2fhO02u1cf6lPFzBMvorJcRvNaVhovj)


