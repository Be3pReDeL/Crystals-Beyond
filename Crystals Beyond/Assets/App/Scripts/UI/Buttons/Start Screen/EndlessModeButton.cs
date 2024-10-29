using UnityEngine;

public class EndlessModeButton : MonoBehaviour
{
    public void EndlessModeButtonClick()
    {
        // Устанавливаем режим игры в "endless"
        PlayerPrefsController.SetGameMode(GameController.GameMode.Endless);

        // Загружаем следующую сцену
        LoadSceneButton.LoadRelativeScene(1);
    }
}
