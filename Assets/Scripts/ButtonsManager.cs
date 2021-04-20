using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
	private void Update()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void LoadGameScreen()
	{
		SceneManager.LoadScene(Constants.SceneGame, LoadSceneMode.Single);
	}

	public void LoadOptionsScreen()
	{
		SceneManager.LoadScene(Constants.SceneGameOptions, LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadGameMenuScreen()
	{
		SceneManager.LoadScene(Constants.SceneGameMenu, LoadSceneMode.Single);
	}

	public void LoadInstructionsScreen()
	{
		SceneManager.LoadScene(Constants.SceneGameInstructions, LoadSceneMode.Single);
	}
}