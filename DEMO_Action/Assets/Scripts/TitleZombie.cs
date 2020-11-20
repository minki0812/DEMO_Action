using UnityEngine;
using UnityEngine.AI;

public class TitleZombie : MonoBehaviour
{
    NavMeshAgent zombie = null;
    public Animator anim;
    public GameManager gameManager;

    public Transform[] wayPoint = null;
    int count = 0;

    void MoveToNextWayPoint()
    {
        if (this.gameObject.active == false)
            return;
        if (zombie.velocity == Vector3.zero && gameManager.title == true)
        {
            zombie.SetDestination(wayPoint[count++].position);
            anim.SetBool("isWalk", true);

            if (count >= wayPoint.Length)
            {
                count = 0;
            }
        }
    }

    void Start()
    {
        zombie = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        InvokeRepeating("MoveToNextWayPoint", 0f, 5f);
    }
}
