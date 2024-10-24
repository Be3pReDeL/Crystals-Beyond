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
        int points = PlayerPrefsController.GetPoints(0);
        pointsTextController.SetText(points.ToString());
    }

    public bool TrySpendPoints(int cost)
    {
        int points = PlayerPrefsController.GetPoints(0);

        if (points >= cost)
        {
            PlayerPrefsController.SetPoints(points - cost);
            UpdatePointsUI();
            return true;
        }
        return false;
    }
}
