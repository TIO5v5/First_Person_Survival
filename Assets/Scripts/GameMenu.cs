using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public void Start()
    {
				// Перевірка активності в ієрархії об'єктів і вимикання його
				// Так як при старті ми не повині бачити меню
        if (pauseMenu.activeInHierarchy) 
        {
            PauseOff();
        }
    }

    public void Update()
    {
				//Якщо ми натискаємо Escape, то в залежності
				//Активний він в ієрархії чи ні, ми вмикаємо
				//або вимикаємо паузу
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeInHierarchy)
            {
                PauseOff();
            }
            else
            {
                PauseOn();
            }
        }
    }

		// Перезавантажити поточний рівень
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

		// Зупинити час гри, тоді всі об'єкти перестануть рухатися
		// Та активувати меню
    public void PauseOn()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

		// Відновити час гри, тоді всі об'єкти почнуть рухатися
		// Та відключити меню
    public void PauseOff()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

		// Завантажити сцену по назві Menu
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
