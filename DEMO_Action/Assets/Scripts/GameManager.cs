using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalItemCount;
    public GameObject carNPC;

    public GameObject menuCam;
    public GameObject gameCam;
    public GameObject overCam;

    public GameObject title;
    public GameObject city;
    public GameObject Lv1;
    public GameObject Lv2;
    public GameObject Lv3;
    public GameObject Lv4;

    public GameObject cNPC;

    public Enemy enemy;
    public Player player;
    public BossE bossE;
    public GameObject npcP;
    public GameObject npcW;
    public GameObject startZone1;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public GameObject controlPanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreText;
    public Text bestText;

    public Text totalGasolineText;
    public Text playerCountText;

    void Awake()
    {
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
        totalGasolineText.text = " / " + totalItemCount;

        if (PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
    }

    public void ScoreUpdate()
    {
        PlayerPrefs.SetInt("MaxScore", player.score);
    }

    public void GetItem(int count)
    {
        playerCountText.text = count.ToString();
    }

    public void GameStart()
    {
        title.SetActive(false);
        city.SetActive(true);

        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.score > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void CarActive()
    {
        cNPC.SetActive(true);
    }

    public void Ending()
    {
        PlayerPrefs.SetInt("MaxScore", player.score);
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ControlPanel()
    {
        controlPanel.SetActive(true);
    }

    public void ExitControl()
    {
        controlPanel.SetActive(false);
    }

    public void Lv1Start()
    {
        city.SetActive(false);
        Lv1.SetActive(true);
    }
    
    public void Lv1Exit()
    {
        city.SetActive(true);
        Lv1.SetActive(false);
    }

    public void Lv2Start()
    {
        city.SetActive(false);
        Lv2.SetActive(true);
    }

    public void Lv2Exit()
    {
        city.SetActive(true);
        Lv2.SetActive(false);
    }

    public void Lv3Start()
    {
        city.SetActive(false);
        Lv3.SetActive(true);
    }

    public void Lv3Exit()
    {
        city.SetActive(true);
        Lv3.SetActive(false);
    }

    public void Lv4Start()
    {
        city.SetActive(false);
        Lv4.SetActive(true);
    }

    public void Lv4Exit()
    {
        city.SetActive(true);
        Lv4.SetActive(false);
    }

    void LateUpdate()
    {
        scoreTxt.text = string.Format("{0:n0}", player.score);
        playerHealthTxt.text = player.health + " / " + player.maxHealth;

        if (player.equipWeapon == null)
        {
            playerAmmoTxt.text = "- / " + player.ammo;
        }
        else if (player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerAmmoTxt.text = "- / " + player.ammo;
        }
        else
        {
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
