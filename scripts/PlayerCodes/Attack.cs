//Attack.cs
//controls player attack behavior, weapon switching, enemy hit detection,
//ultimate activation, and UI updates like stamina and ultimate charge

using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [Header("Core Settings")]
    public Transform cameraTransform;
    public RawImage myRawImage;
    public LayerMask enemyLayer;

    [Header("Attack Settings")]
    public int attackDamage = 10;
    public float attackCooldown = 0.75f;
    private float tempCooldown;
    public float hitDetectionRadius = 1f;
    public Vector3 hitboxOffset = new Vector3(0, -0.5f, 1.5f);

    [Header("UI & Stats")]
    public PlayerHealth playerHealth;
    public Image staminaBar;
    public Image ultBar;
    public GameObject crosshair;
    public GameObject ammoUI; 
    public TextMeshProUGUI ammoText;

    [Header("Weapon Setup")]
    public GameObject[] weapons;
    public GameObject activeWeapon;
    public Animator activeWeaponAnimator;
    private bool isShotgun;
    public shotGunProjectile shotgunProjectileScript;
    public int ammo;

    [Header("Light Effects")]
    public GameObject FL;

    [Header("Camera")]
    public GameObject camera;

    [Header("Weapon Prices")]
    public TextMeshProUGUI weaponPriceFist;
    public TextMeshProUGUI weaponPriceKnife;
    public TextMeshProUGUI weaponPriceHammer;
    public TextMeshProUGUI weaponPriceAxe;
    public TextMeshProUGUI weaponPriceShotgun;
    public TextMeshProUGUI weaponPriceSword;
    public Dictionary<string, int> weaponPriceDict = new Dictionary<string, int>();

    private Camera cam;
    private PlayerMovement playerMovement;
    private float speedTemp;
    private bool isAttacking = false;
    private bool isFull = false;
    public bool isUltimateActive = false;
    public bool NotdoneOnce = true;
    public bool checkProg = true;

    private string[] attackAnimations = { "Attack", "Attack1", "Attack2", "rare", "rareSpecial" };

    void Start()
    {
        cam = camera.GetComponent<Camera>();
        ammo = 2;
        
        //assign default camera transform if not set
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        playerHealth = FindObjectOfType<PlayerHealth>();

        //populate weapon price dictionary
        weaponPriceDict.Add("Fist", 0);
        weaponPriceDict.Add("Knife", 150);
        weaponPriceDict.Add("Hammer", 300);
        weaponPriceDict.Add("Axe", 500);
        weaponPriceDict.Add("Shotgun", 999);
        weaponPriceDict.Add("Sword", 750);

        //update price UI
        UpdateWeaponPriceUI();

        //equip default weapon
        SetActiveWeapon(weapons[0]);

        //initialize ult bar
        ultBar.fillAmount = 0f;
    }

    void Update()
    {
                if (Input.GetKeyDown(KeyCode.C) && !isAttacking)
        {
            if (activeWeaponAnimator != null)
            {
                activeWeaponAnimator.SetTrigger("specialAtt");
            }
        }

        if (isShotgun && !activeWeaponAnimator.GetBool("isReloading") && Input.GetKeyDown(KeyCode.R) && ammo != 2){
            StartCoroutine(Reload());
        }
        //start draining ultimate if full
        if (ultBar.fillAmount >= 1f && !isFull)
        {
            isFull = true;
            StartCoroutine(WaitBeforeDecrease());
        }

        //slowly drain ult bar
        if (ultBar.fillAmount >= 0.05f && !isFull)
            UltDec();

        //activate ultimate
        if (Input.GetKeyDown(KeyCode.X) && ultBar.fillAmount >= 0.95f && !isUltimateActive)
            StartCoroutine(Ultimate());

        if (playerHealth == null) return;

        //perform attack
        if (Input.GetMouseButtonDown(0) && !isAttacking)
            StartCoroutine(PerformAttack());
    }

    public void UpdateWeaponPriceUI()
    {
        //update each weapon price UI label
        if (weaponPriceDict.TryGetValue("Fist", out int priceFist))
            weaponPriceFist.text = "$" + priceFist;

        if (weaponPriceDict.TryGetValue("Knife", out int priceKnife))
            weaponPriceKnife.text = "$" + priceKnife;

        if (weaponPriceDict.TryGetValue("Hammer", out int priceHammer))
            weaponPriceHammer.text = "$" + priceHammer;

        if (weaponPriceDict.TryGetValue("Axe", out int priceAxe))
            weaponPriceAxe.text = "$" + priceAxe;

        if (weaponPriceDict.TryGetValue("Shotgun", out int priceShotgun))
            weaponPriceShotgun.text = "$" + priceShotgun;

        if (weaponPriceDict.TryGetValue("Sword", out int priceSword))
            weaponPriceSword.text = "$" + priceSword;
    }

    void UltDec() => ultBar.fillAmount = Mathf.Clamp(ultBar.fillAmount - 0.00015f, 0f, 1f);

    IEnumerator WaitBeforeDecrease()
    {
        yield return new WaitForSeconds(3f);
        UltDec();
        yield return new WaitForSeconds(1f);
        isFull = false;
    }

    IEnumerator Ultimate()
    {
        //start ultimate
        Debug.Log("Ultimate Activated");
        myRawImage.color = new Color(1f, 0.694f, 0.694f);
        tempCooldown = attackCooldown;
        
        
        if(isShotgun){
            attackCooldown /= 3f; 
            activeWeapon.GetComponent<Animator>().speed = 3f;
        } else{
            attackCooldown /= 2f;
            activeWeapon.GetComponent<Animator>().speed = 2f;
        }

        isUltimateActive = true;

        //play special animation
        if (activeWeaponAnimator != null)
            activeWeaponAnimator.SetTrigger("specialAtt");

        //boost damage and speed
        int originalDamage = attackDamage;
        attackDamage = Mathf.RoundToInt(originalDamage * 1.5f);
        playerMovement = FindObjectOfType<PlayerMovement>();
        speedTemp = playerMovement.speed;

        StartCoroutine(MaintainBuff());

        //drain ultimate bar
        while (ultBar.fillAmount > 0f)
        {
            ultBar.fillAmount = Mathf.Clamp(ultBar.fillAmount - 0.0008f, 0f, 1f);
            yield return null;
        }

        //reset FOV smoothly
        while (cam.fieldOfView > 61)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, Time.deltaTime * 5f);
            yield return null;
        }

        //revert stats
        attackDamage = originalDamage;
        playerMovement.speed = speedTemp;
        isUltimateActive = false;
        activeWeapon.GetComponent<Animator>().speed = 1f;
        attackCooldown = tempCooldown;
        myRawImage.color = Color.white;

        Debug.Log("Ultimate Ended");
    }

    IEnumerator MaintainBuff()
    {
        //apply visual and speed buffs
        while (ultBar.fillAmount > 0f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 85f, Time.deltaTime * 5f);
            FindObjectOfType<PlayerMovement>().RefillStamina();
            yield return null;
            playerMovement.speed = 45f;
            if(isShotgun){ammo = 99; ammoText.text = ammo +"/99";}
        }
        if(isShotgun){ammo = 2; ammoText.text = "0" + ammo +"/02";}
    }
    private IEnumerator Reload(){
        Debug.Log("Reloading");
        activeWeaponAnimator.SetBool("isReloading", true);
            activeWeaponAnimator.Play("Reloading");
            yield return new WaitForSeconds(0.2f);
            ammo = 2;
            ammoText.text = ("0" + ammo + "/02");
            activeWeaponAnimator.SetBool("isReloading", false);
            yield return new WaitForSeconds(0.3f);
            yield break;
    }
    IEnumerator PerformAttack()
    {
       
        if (activeWeapon == null) yield break;

        isAttacking = true;

        //play random attack animation
        if (activeWeaponAnimator != null && ammo >= 1)
        {
        string selectedAttack;
        int randomChoice = Random.Range(0, 100);
        if (randomChoice == 0)
        {
            if(isShotgun){
                selectedAttack = attackAnimations[4];
                activeWeaponAnimator.Play(selectedAttack);
                shotgunProjectileScript.ShootBullet();
                yield return new WaitForSeconds(0.10f);
                shotgunProjectileScript.ShootBullet(); 
                yield return new WaitForSeconds(0.10f);
                shotgunProjectileScript.ShootBullet(); 
                yield return new WaitForSeconds(0.10f);
                shotgunProjectileScript.ShootBullet(); 
                yield return new WaitForSeconds(0.10f);
                shotgunProjectileScript.ShootBullet(); 
                yield return new WaitForSeconds(0.10f);
            }else{
                selectedAttack = attackAnimations[4];
                activeWeaponAnimator.Play(selectedAttack);
                DealDamageToClosestEnemy();
                yield return new WaitForSeconds(0.10f);
                DealDamageToClosestEnemy();
                yield return new WaitForSeconds(0.10f);
                DealDamageToClosestEnemy();
                yield return new WaitForSeconds(0.10f);
                DealDamageToClosestEnemy();
                yield return new WaitForSeconds(0.10f);
                DealDamageToClosestEnemy();
                yield return new WaitForSeconds(0.10f);
            }
            
        }
        else if(randomChoice == 1)
        {
            selectedAttack = attackAnimations[3];
            activeWeaponAnimator.Play(selectedAttack);
        }else
        {
            selectedAttack = attackAnimations[Random.Range(0, 3)];
            activeWeaponAnimator.Play(selectedAttack);
        }
            
        }

        
        if(isShotgun){
            if (shotgunProjectileScript != null && !activeWeaponAnimator.GetBool("isReloading") && ammo >= 1)
            {
                ammo--;
                ammoText.text = ("0" + ammo + "/02");
                shotgunProjectileScript.ShootBullet();             
                Debug.Log("shotgunProjectileScript.ShootBullet(); worked");
            }
        }else{
            switch (activeWeapon.name)
        {
            case "Fist":
            case "Knife":
            yield return new WaitForSeconds(0.15f);
            break;
            
            case "Hammer":
            case "Axe":
            yield return new WaitForSeconds(0.30f);
            break;
            
            case "Shotgun": break;
            case "Sword":
            yield return new WaitForSeconds(0.24f);
            break;
            
            default: break;
        }
            DealDamageToClosestEnemy();
            switch (activeWeapon.name)
        {
            case "Fist":
            case "Knife":
            yield return new WaitForSeconds(0.10f);
            DealDamageToClosestEnemy();
            break;
            case "Sword":
            yield return new WaitForSeconds(0.20f);
            DealDamageToClosestEnemy();
            break;
            
            default: break;
        }
        }
        yield return new WaitForSeconds(attackCooldown/2);
        isAttacking = false;
    }
    public void ultIncrease(){
        ultBar.fillAmount = Mathf.Clamp(ultBar.fillAmount + 0.07f, 0f, 1f);
    }
    void DealDamageToClosestEnemy()
    {
        //calculate hitbox position
        Vector3 hitboxPosition = cameraTransform.position + cameraTransform.TransformDirection(hitboxOffset);
        Collider[] hitEnemies = Physics.OverlapSphere(hitboxPosition, hitDetectionRadius, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            //get closest enemy
            Collider closestEnemy = hitEnemies.OrderBy(e => Vector3.Distance(hitboxPosition, e.transform.position)).FirstOrDefault();

            if (closestEnemy != null)
            {
                Enemy enemyScript = closestEnemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(attackDamage);
                    Debug.Log("Enemy hit!");

                    //gain ultimate charge if not in ultimate
                    if (!isUltimateActive)
                        ultBar.fillAmount = Mathf.Clamp(ultBar.fillAmount + 0.07f, 0f, 1f);
                }
            }
        }

        checkProg = true;
    }

    public void SetActiveWeapon(GameObject newWeapon)
    {
        if (newWeapon == null) return;

        //disable all weapons
        foreach (GameObject weapon in weapons)
        {
            if (weapon != null) weapon.SetActive(false);
        }

        //set and activate new weapon
        activeWeapon = newWeapon;
        activeWeapon.SetActive(true);
        activeWeaponAnimator = activeWeapon.GetComponent<Animator>();

        //update stats based on weapon
        switch (activeWeapon.name)
        {
            case "Fist": attackDamage = 2; attackCooldown = 0.6f; hitDetectionRadius = 6.5f; crosshair.SetActive(false); isShotgun = false; ammo = 2;ammoUI.SetActive(false); break;
            case "Knife": attackDamage = 7; attackCooldown = 0.8f; hitDetectionRadius = 7f; crosshair.SetActive(false); isShotgun = false; ammo = 2;ammoUI.SetActive(false); break;
            case "Hammer": attackDamage = 20; attackCooldown = 0.6f; hitDetectionRadius = 8f; crosshair.SetActive(false); isShotgun = false; ammo = 2;ammoUI.SetActive(false); break;
            case "Axe": attackDamage = 25; attackCooldown = 1.0f; hitDetectionRadius = 10f; crosshair.SetActive(false); isShotgun = false; ammo = 2;ammoUI.SetActive(false); break;
            case "Shotgun": attackDamage = 0; attackCooldown = 1f; hitDetectionRadius = 0f; crosshair.SetActive(true); isShotgun = true; ammo = 2;ammoUI.SetActive(true); break;
            case "Sword": attackDamage = 25; attackCooldown = 1f; hitDetectionRadius = 9f; crosshair.SetActive(false); isShotgun = false; ammo = 2;ammoUI.SetActive(false); break;
            default: attackDamage = 0; attackCooldown = 0.75f; hitDetectionRadius = 1.0f; crosshair.SetActive(false); isShotgun = false; ammo = 2;ammoUI.SetActive(false); break;
        }
        

    }

    void DisableGameObject(GameObject obj)
    {
        if (obj != null) obj.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        //draw hitbox in scene view
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Vector3 hitboxPosition = cameraTransform.position + cameraTransform.TransformDirection(hitboxOffset);
            Gizmos.DrawWireSphere(hitboxPosition, hitDetectionRadius);
        }
    }
}
