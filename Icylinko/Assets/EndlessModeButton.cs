using UnityEngine;

public class EndlessModeButton : MonoBehaviour
{
    public void EndlessModeButtonClick()
    {
        // Устанавливаем режим игры в "endless"
        PlayerPrefsController.SetGameMode(GameController.GameMode.endless);

        // Загружаем следующую сцену
        LoadSceneButton.LoadRelativeScene(1);
    }
}
