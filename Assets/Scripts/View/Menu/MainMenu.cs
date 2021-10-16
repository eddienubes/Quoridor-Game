using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quoridorgame.View
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayAgainstPlayer()
        {
            SceneManager.LoadScene(3);
        }

        public void PlayAgainstAI()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Debug.Log("Quit successfully!");
            Application.Quit();
        }
    }

}
