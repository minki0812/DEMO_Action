using UnityEngine;
using UnityEngine.AI;

public class CarMove : MonoBehaviour
{
    public Transform target;
    NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Invoke("NavActive", 1f);
    }

    void NavActive()
    {
        nav.SetDestination(target.position);
    }
}
