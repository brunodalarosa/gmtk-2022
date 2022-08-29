using Data;

namespace Manager.Interface
{
    public interface ILevelManager
    {
        bool GamePaused { get; }
        void UiCounterNoUse(UiElementType counterDodge);
        void UpdateUI(UiElementType counterAttack, float value);
        void RemoveAndDestroyEnemy(EnemyData enemyData);
        void UpdatePlayerHp(int value);
        void AddD6ToPlayer();
        void LeaveDiceMenu();
        void ApplyDiceRoll(RollType rollType, int adjustedValue, int rolledValue);
        float AnimateGodDiceRoll(int rolledValue);
        void AddNewEnemy(EnemyData enemy);
    }
}