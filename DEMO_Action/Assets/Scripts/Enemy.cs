using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D, E, F, G };
    public Type enemyType;

    public AudioSource batSound;
    public AudioSource chaseSound;
    public AudioSource dieSound;

    GameManager gameManager;

    public int score;
    public int killScore;
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameObject[] items;
    public GameObject hitEffect;
    public GameObject dieEffect;

    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public bool isBoss;

    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;

    Vector3 lookVec;

    

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    public void ChaseStart()
    {
        if (!isDead)
        {
            isChase = true;
            anim.SetBool("isWalk", true);
            chaseSound.Play();
        }
    }

    public void ChaseStop()
    {
        isChase = false;
        anim.SetBool("isWalk", false);
    }

    void Update()
    {
        if (nav.enabled && enemyType != Type.E)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        if (!isDead && enemyType != Type.E)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = .75f;
                    targetRange = .1f;
                    break;
                case Type.B:
                    targetRadius = .5f;
                    targetRange = .4f;
                    break;
                case Type.C:
                    targetRadius = .5f;
                    targetRange = 1f;
                    break;
                case Type.D:
                    targetRadius = .5f;
                    targetRange = 8f;
                    break;
                case Type.E:
                    break;
                case Type.F:
                    targetRadius = .5f;
                    targetRange = 20f;
                    break;
                case Type.G:
                    targetRadius = 1f;
                    targetRange = 1f;
                    break;
            }
            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.1f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.D:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward *18, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.E:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.F:
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                lookVec = new Vector3(h, 0, v) * 2f;
                transform.LookAt(target.position + lookVec);

                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 10;

                yield return new WaitForSeconds(.5f);
                break;
            case Type.G:
                yield return new WaitForSeconds(0.1f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee" && !isDead)
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            batSound.Play();
            //넉백
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec));
        }
        else if (other.tag == "Bullet" && !isDead)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            //넉백
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
        }
        
    }
    IEnumerator OnDamage(Vector3 reactVec)
    {
        Instantiate(hitEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * (-15), ForceMode.Impulse);
            Player player = target.GetComponent<Player>();
            player.score += score;
        }
        else 
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 14;
            isDead = true;
            dieSound.Play();
            isChase = false;
            isBoss = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            Player player = target.GetComponent<Player>();
            player.score += killScore;

            int ranItems = Random.Range(0, 10);
            Instantiate(items[ranItems], transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * (-20), ForceMode.Impulse);

            Invoke("Die", 4);
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Instantiate(dieEffect, transform.position, Quaternion.identity);
    }
}
