using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public GameObject endGroup;
    public Text scoreText;
    public Text newRecord;

    private void Start()
    {
        Invoke("EndGroupActive", 5f);
    }

    public void EndGroupActive()
    {
        endGroup.SetActive(true);
        int maxScore = PlayerPrefs.GetInt("MaxScore");
        scoreText.text = maxScore.ToString();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
