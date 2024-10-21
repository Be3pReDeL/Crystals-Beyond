using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("UI/Buttons/Load Scene")]
public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(int index) => SceneManager.LoadScene(index);

    public void LoadRelativeScene(int index) => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
}
