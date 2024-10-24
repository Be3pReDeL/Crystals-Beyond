using UnityEngine;

public class PlayerPointsController : MonoBehaviour
{
    [SerializeField] private TextController pointsTextController;

    private void Start()
    {
        UpdatePointsUI();
    }

    public void UpdatePointsUI()
    {
        int points = GetPoints(); // Обновляем UI с использованием нового метода
        pointsTextController.SetText(points.ToString());
    }

    public bool TrySpendPoints(int cost)
    {
        int points = GetPoints();

        if (points >= cost)
        {
            SetPoints(points - cost);
            UpdatePointsUI();
            return true;
        }
        return false;
    }

    // Добавляем метод для получения текущих очков
    public int GetPoints()
    {
        return PlayerPrefsController.GetPoints(0); // Получаем очки из PlayerPrefs
    }

    // Метод для установки очков
    public void SetPoints(int points)
    {
        PlayerPrefsController.SetPoints(points);
    }
}