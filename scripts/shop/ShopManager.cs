using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    //shop ui elements
    public GameObject shopUI;
    public GameObject timerUI;
    public Button button1;
    public Button button2;
    public TMP_Text button1Text;
    public TMP_Text button2Text;
    public Button button1passive;
    public Button button2passive;
    public TMP_Text button1Textpassive;
    public TMP_Text button2Textpassive;
    public TMP_Text abilityTimerText;
    [SerializeField] private GameObject ESCbtn;

    //player-related components
    public PlayerMovement playerMovement;
    public MouseLook mouseLook;
    public Attack playerAttack;
    public DayCycle dayCycle;
    private CurrencyHolder currencyHolder;

    //ability lists
    public List<GameObject> QAB = new List<GameObject>();
    public List<GameObject> EAB = new List<GameObject>();
    public List<GameObject> PAB = new List<GameObject>();

    //selected abilities
    private GameObject selectedAbility1;
    private GameObject selectedAbility2;
    private GameObject selectedAbility1P;
    private GameObject selectedAbility2P;

    //currently active abilities
    private GameObject activeQAbility;
    private GameObject activeEAbility;

    public ShopMoveAway shopMoveAway;
    
    private GameObject player;
    public int refreshCost = 100;

    void Start()
    {
        shopMoveAway = FindObjectOfType<ShopMoveAway>();
        //setup references
        player = GameObject.FindWithTag("Player");
        currencyHolder = FindObjectOfType<CurrencyHolder>();
        dayCycle = FindObjectOfType<DayCycle>();

        //connect buttons to methods
        button1.onClick.AddListener(BuyAbility1);
        button2.onClick.AddListener(BuyAbility2);
        button1passive.onClick.AddListener(BuyAbility1P);
        button2passive.onClick.AddListener(BuyAbility2P);

        //disable all abilities
        DisableAllAbilities(QAB);
        DisableAllAbilities(EAB);
        DisableAllAbilities(PAB);

        //hide shop
        CloseShop();

        //give starting skulls
        currencyHolder.AddSkulls(refreshCost);

        //initial shop roll
        RefreshShop();
    }

    //disables all gameobjects in a list
    private void DisableAllAbilities(List<GameObject> list)
    {
        foreach (GameObject ability in list)
        {
            if (ability != null)
                ability.SetActive(false);
        }
    }

    //opens or closes the shop and disables controls
    public void ToggleShop()
    {
        bool isOpening = !shopUI.activeSelf;
        ESCbtn.SetActive(!isOpening);
        shopUI.SetActive(isOpening);
        Cursor.visible = isOpening;
        Cursor.lockState = isOpening ? CursorLockMode.None : CursorLockMode.Locked;
        

        if (playerMovement != null) playerMovement.enabled = !isOpening;
        if (mouseLook != null) mouseLook.enabled = !isOpening;
        if (playerAttack != null) playerAttack.enabled = !isOpening;

        if (isOpening)
            RefreshShop();
    }

    //picks new random abilities for shop
    public void RefreshShop()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        //remove skulls or exit
        if (currencyHolder == null || !currencyHolder.RemoveSkulls(refreshCost))
            return;

        //filter out active abilities
        List<GameObject> availableQAB = QAB.FindAll(a => !a.activeSelf);
        List<GameObject> availableEAB = EAB.FindAll(a => !a.activeSelf);
        List<GameObject> availablePAB = PAB.FindAll(a => !a.activeSelf);

        //exit if not enough left
        if (availableQAB.Count == 0 || availableEAB.Count == 0 || availablePAB.Count < 2)
            return;

        //pick random ones
        selectedAbility1 = availableQAB[Random.Range(0, availableQAB.Count)];
        selectedAbility2 = availableEAB[Random.Range(0, availableEAB.Count)];
        selectedAbility1P = availablePAB[Random.Range(0, availablePAB.Count)];

        //make sure second passive is different
        GameObject secondPassive;
        do
        {
            secondPassive = availablePAB[Random.Range(0, availablePAB.Count)];
        } while (secondPassive == selectedAbility1P);
        selectedAbility2P = secondPassive;

        //set button texts
        button1Text.text = selectedAbility1.name;
        button2Text.text = selectedAbility2.name;
        button1Textpassive.text = selectedAbility1P.name;
        button2Textpassive.text = selectedAbility2P.name;
    }

    //purchases first q ability
    public void BuyAbility1()
    {
        if (activeQAbility != null)
            activeQAbility.SetActive(false);

        selectedAbility1.SetActive(true);
        activeQAbility = selectedAbility1;
        ToggleShop();
        StartCoroutine(shopMoveAway.MoveSequence());
        dayCycle.SetNight();
    }

    //purchases second e ability
    public void BuyAbility2()
    {
        if (activeEAbility != null)
            activeEAbility.SetActive(false);

        selectedAbility2.SetActive(true);
        activeEAbility = selectedAbility2;
        ToggleShop();
        StartCoroutine(shopMoveAway.MoveSequence());
        dayCycle.SetNight();
    }

    //purchases first passive
    public void BuyAbility1P()
    {
        selectedAbility1P.SetActive(true);
        ToggleShop();
        StartCoroutine(shopMoveAway.MoveSequence());
        dayCycle.SetNight();
    }

    //purchases second passive
    public void BuyAbility2P()
    {
        selectedAbility2P.SetActive(true);
        ToggleShop();
        StartCoroutine(shopMoveAway.MoveSequence());
        dayCycle.SetNight();
    }

    //closes the shop manually
    public void CloseShop()
    {
        shopUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerMovement != null) playerMovement.enabled = true;
        if (mouseLook != null) mouseLook.enabled = true;
        if (playerAttack != null) playerAttack.enabled = true;
    }
}
