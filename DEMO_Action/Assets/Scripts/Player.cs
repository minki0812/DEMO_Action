using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public Camera followCamera;

    public AudioSource moveSound;
    public AudioSource reloadSound;
    public AudioSource hitSound;

    public GameObject hitEffect;

    public GameObject lv2Potal;
    public GameObject lv3Potal;
    public GameObject lv4Potal;

    public int ammo;
    public int coin;
    public int health;
    public int score;
    public int gasoline = 0;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    float hAxis;
    float vAxis;

    bool wDown;
    //bool jDown;
    bool fDown;
    bool rDown;
    bool iDown;

    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;

    //bool isJump;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;
    bool isNPC;
    public bool isDead;

    Vector3 moveVec;
    public GameManager gameManager;
    Bullet enemyBullet;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;
    GameObject nearObject;
    public Weapon equipWeapon;

    int equipWeaponIndex = -1;
    float fireDelay;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        PlayerPrefs.SetInt("MaxScoree", 6542100);
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        //Jump();
        Attack();
        Reload();
        Swap();
        Interation();

        if (gasoline >= 2)
        {
            lv2Potal.SetActive(true);
            lv3Potal.SetActive(true);
            lv4Potal.SetActive(true);
        }

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        //jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        sDown4 = Input.GetButtonDown("Swap4");
        sDown5 = Input.GetButtonDown("Swap5");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (!isFireReady || isDead)
            moveVec = Vector3.zero;

        if (!isBorder)
        {
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
            moveSound.Play();
        }
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //키보드
        transform.LookAt(transform.position + moveVec);

        //마우스
        if (fDown && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    //점프
    //void Jump()
    //{
    //    if (jDown && !isJump)
    //    {
    //        rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
    //        isJump = true;
    //    }
    //}

    void Attack()
    {
        if (equipWeapon == null)
            return;
        
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isSwap && !isNPC && !isDead)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }

    }

    void Reload()
    {
        if (isReload)
            return;

        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if (rDown && !isSwap && isFireReady && !isNPC && !isDead)
        {
            //anim.SetTrigger("doReload");
            isReload = true;
            reloadSound.Play();

            Invoke("ReloadOut", 1f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
        {
            return;
        }
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
        {
            return;
        }
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
        {
            return;
        }
        if (sDown4 && (!hasWeapons[3] || equipWeaponIndex == 3))
        {
            return;
        }
        if (sDown5 && (!hasWeapons[4] || equipWeaponIndex == 4))
        {
            return;
        }

        int weaponIndex = -1;
        if (sDown1)
        {
            weaponIndex = 0;
        }
        if (sDown2)
        {
            weaponIndex = 1;
        }
        if (sDown3)
        {
            weaponIndex = 2;
        }
        if (sDown4)
        {
            weaponIndex = 3;
        }
        if (sDown5)
        {
            weaponIndex = 4;
        }

        if ((sDown1 || sDown2 || sDown3 || sDown4 || sDown5) && !isNPC && !isDead)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
                equipWeapon.uiImg.gameObject.SetActive(false);
            }

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            equipWeapon.uiImg.gameObject.SetActive(true);

            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        if (iDown && nearObject != null && !isNPC && !isDead)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
            else if (nearObject.tag == "NPC")
            {
                NPC npc = nearObject.GetComponent<NPC>();
                npc.Enter(this);
                isNPC = true;
            }
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * 1, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 1, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    void OnCollisionEnter(Collision collision)
    {
        //점프
        //if (collision.gameObject.tag == "Floor")
        //{
        //    isJump = false;
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Food:
                    score += item.value;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Gasoline:
                    gasoline += item.value;
                    gameManager.GetItem(gasoline);
                    if (gasoline == gameManager.totalItemCount)
                        gameManager.CarActive();
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage && !isDead)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                bool isBossAtk = other.name == "Boss Melee Area";
                StartCoroutine(OnDamage(isBossAtk));
            }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.name == "Lv1Potal")
        {
            gameManager.Lv1Start();
            gameObject.transform.position = new Vector3(0, 0, 0);
        }

        else if (other.gameObject.name == "Lv1 Exit")
        {
            gameManager.Lv1Exit();
            gameObject.transform.position = new Vector3(12, 0.3f, 5);
        }

        else if (other.gameObject.name == "Lv2Potal")
        {
            gameManager.Lv2Start();
            gameObject.transform.position = new Vector3(0, 0, 0);
        }

        else if (other.gameObject.name == "Lv2 Exit")
        {
            gameManager.Lv2Exit();
            gameObject.transform.position = new Vector3(-3, 0.3f, 13);
        }

        else if (other.gameObject.name == "Lv3Potal")
        {
            gameManager.Lv3Start();
            gameObject.transform.position = new Vector3(0, 0, 0);
        }

        else if (other.gameObject.name == "Lv3 Exit")
        {
            gameManager.Lv3Exit();
            gameObject.transform.position = new Vector3(-12, 0.3f, -2);
        }

        else if (other.gameObject.name == "Lv4Potal")
        {
            gameManager.Lv4Start();
            gameObject.transform.position = new Vector3(0, 0, 0);
        }

        else if (other.gameObject.name == "Lv4 Exit")
        {
            gameManager.Lv4Exit();
            gameObject.transform.position = new Vector3(4, 0.3f, -10);
        }
    }

    //플레이어 피격부분
    IEnumerator OnDamage(bool isBossAtk)
    {
        Instantiate(hitEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        isDamage = true;
        hitSound.Play();
        //foreach (MeshRenderer mesh in meshs)
        //{
        //    mesh.material.color = Color.red;
        //}

        if (isBossAtk)
            rigid.AddForce(transform.forward * (-25), ForceMode.Impulse);

        if (health <= 0 && !isDead)
            OnDie();

        yield return new WaitForSeconds(1f);

        isDamage = false;
        //foreach (MeshRenderer mesh in meshs)
        //{
        //    mesh.material.color = Color.white;
        //}

        if (isBossAtk)
            rigid.velocity = Vector3.zero;
    }
    
    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        gameManager.GameOver();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "NPC")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
        else if (other.tag == "NPC")
        {
            NPC npc = nearObject.GetComponent<NPC>();
            //npc.Exit();
            isNPC = false;
            nearObject = null;
        }
    }
}
