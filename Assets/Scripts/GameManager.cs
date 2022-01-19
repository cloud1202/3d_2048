/* 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject board;
    public GameObject gameMenu;
    public GameObject uiMenu;
    public Text scoreTxt;

    // if game restart a value true
    public bool isReset;

    bool isPause = true, isGame = false;

    int scoreCount = 0;


#if UNITY_WEBGL
    public static string webplayerQuitURL = "https://cloud1202.github.io/3D_2048/";
#endif

    // Start is called before the first frame update
    void Start()
    {
        if (isReset)
        {
            OnClickStartBtn();
        }
    }
    public void GameEnd()
    {
        gameMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnClickStartBtn()
    {
        Time.timeScale = 1;
        if (!isGame)
        {
            uiMenu.SetActive(true);
            board.SetActive(true);
            this.board.GetComponent<BoardManager>().GameStart();
            isGame = true;
        }
        else if (isGame || !isPause)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void OnClickExitBtn()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
    }

    public void OnClickPauseBtn()
    {
        if (isPause)
        {
            gameMenu.SetActive(true);

            Time.timeScale = 0;
            isPause = false;
        }
        else
        {
            gameMenu.SetActive(false);

            Time.timeScale = 1;
            isPause = true;
        }
    }
    public void AddScore(int boxIndex)
    {
        scoreCount += (int)Mathf.Pow(2.0f, (float)boxIndex);
        scoreTxt.text = scoreCount + "";
    }
}
