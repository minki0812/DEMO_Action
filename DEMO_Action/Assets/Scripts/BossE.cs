using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossE : Enemy
{
    public GameObject cureEffect;
    public GameObject monster;
    public Transform summonPointA;
    public Transform summonPointB;
    public bool isLook;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 4);
        switch (ranAction)
        {
            case 0:
                //디펜스
                StartCoroutine(Defend());
                break;
            case 1:
                //치료
                StartCoroutine(Cure());
                break;
            case 2:
            case 3:
                break;
        }
    }

    IEnumerator Defend()
    {
        anim.SetTrigger("doDefend");
        yield return new WaitForSeconds(3f);

        StartCoroutine(Think());
    }

    IEnumerator Summon()
    {
        anim.SetTrigger("doSummon");
        yield return new WaitForSeconds(3f);
        GameObject instantMonsterA = Instantiate(monster, summonPointA.position, summonPointA.rotation);
        GameObject instantMonsterB = Instantiate(monster, summonPointB.position, summonPointB.rotation);

        yield return new WaitForSeconds(3f);

        StartCoroutine(Think());
    }

    IEnumerator Cure()
    {
        anim.SetTrigger("doCure");
        Instantiate(cureEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        yield return new WaitForSeconds(3f);

        StartCoroutine(Think());
    }
}
