using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator anim;

    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;
    public string[] talkData;
    public Text talkText;

    Player enterPlayer;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.down*350;
    }

    public void Exit()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
        Instantiate(itemObj[index], itemPos[index].position , itemPos[index].rotation);
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(50f);

        talkText.text = talkData[0];
    }
}
