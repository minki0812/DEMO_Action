using UnityEngine;

public class Lv1 : MonoBehaviour
{
    public GameManager gameManager;
    Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.Lv1Start();
            other.gameObject.transform.position = new Vector3(0, 0, 0);
        }
    }
}
