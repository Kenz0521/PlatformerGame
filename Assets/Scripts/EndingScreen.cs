using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreen : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
