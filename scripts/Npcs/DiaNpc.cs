using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DIANPC : MonoBehaviour
{
    public TMP_Text nameText;
    public Transform player;
    public GameObject interactUI;
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public waveSpawner waveSpawner;
    public PlayerMovement playerMovement;
    public MouseLook mouseLook;
    public Attack playerAttack;
    public PlayerHealth playerHP;
    public GameObject ESCbtn;
    public float interactRange = 10f;
    public MoveToPlayer moveScript;

    private int dialogueIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private string[][] dayDialogues = new string[][]
    {
        new string[] {    "",
    "Even the trees have stopped listening.",
    "That fancy douche with the hat? Useless.",
    "Can I follow you? I won’t talk much.",
    "Right.. Ghosts don’t get conversations.",
    "Or maybe you're mute?",
    "Doesn’t matter. I’ll walk with you.",
    "You can hit me in return, to fill whatever's that blue bar above your weird bean shaped head.",
    "It's not like I can feel it",
    "..."
     },
        new string[] { "", 
        "You know stranger, at a time I was a proper someone, ask Milky he knows",
        "Could even say I was the mayor how loved I was.",
        "Oh how I miss SooberEats",
        "..."
         },
        new string[] { "", "It’s been three days since I met you. Honestly, it’s nice to have someone to have a chat with."
        ,"Well, you can’t talk back, so I’m not sure we could call it a ‘chat’, but hey, it works for me.",
        "..."
         },
        new string[] { "", "I still can't figure out who the trader is...",
        
        "He just appeared out of nowhere. Not that I remember anything clearly, mind you. I've been stuck here longer than I can count.",
        "Probably just forgot his own bone structure by now. Only if I knew how he keeps on living.",
         },
        new string[] { "", "You're still alive... that makes one of us.",
        "You've made it this far, so I guess it's only fair I share a bit more about myself... though honestly, all I feel is emptiness. And by emptiness, I mean literally.",
        "The real problem? I can’t even remember much myself. I don’t even know how long it's been since the accident.",
        "All I remember is a bathtub, hot water that I’ll never feel again...",
        "About the mayor part, that wasn’t true at all. There’s still a chance that I actually was a proper someone and can't remember...",
        "That's all I remember, and Milky the IV of course, not sure when he started talking but he's always been there for me... or for the food.",
        "..."
        },
        new string[] { "", "The rain isn’t doing much for my mood...",
        "Still, I guess it’s better than just dust and silence that's been here for... probably long.",
        "I think I still remember how working a full time job felt like... It sucked..",
        "..." },

        new string[] { 
            "",
            "Dust Purple stuff and Rain? at the same time? it's a first in.. well idk how long but still. Wondering how optimized this is",
            "...",
            "They say the Lady keeps the light alive, but all I see is darkness.",
            "I held onto hope when you came along, someone to talk to.",
            "..."

         },
        new string[] { 
            "",
            "If there’s any freedom left, maybe it’s waiting beyond this island... beyond the skulls... beyond this world",
            "Maybe I've been looking for the wrong thing all along.",
            "..."
         },
         new string[] { 
            "",
            "..."
            
         },
        new string[] { 
            "",
            "I'm not sure which side you're gonna choose..",
            "But either way I'm happy our talk lasted as long as it did.",
            "I'll be fine if you leave, probably... will have Milky still.",
            "But..",
            "..do you want to?",
            "..."
            
         },
         new string[] { 
            "",
            "..."
            
         }

    };

    void Start()
    {
        moveScript = GetComponent<MoveToPlayer>();
        nameText.text = "le fantôme";
        waveSpawner = FindObjectOfType<waveSpawner>();
    }

    void Update()
    {
        if (player == null || dialogueUI == null || interactUI == null || waveSpawner == null) return;

        float distanceToPlayer = (transform.position - player.position).sqrMagnitude;
        bool inRange = distanceToPlayer <= interactRange * interactRange;

        if (!isDialogueActive)
        {
            interactUI.SetActive(inRange);
        }

        if (inRange && Input.GetKeyDown(KeyCode.F) && !isDialogueActive)
        {
            ESCbtn.SetActive(false);
            dialogueUI.SetActive(true);
            interactUI.SetActive(false);
            isDialogueActive = true;
            dialogueIndex = 0;
            TogglePlayerControls(false);
            NextDia();
        }
    }

    public void NextDia()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = GetCurrentDialogue()[dialogueIndex];
            isTyping = false;
        }
        else
        {
            dialogueIndex++;

            if (dialogueIndex < GetCurrentDialogue().Length)
            {
                typingCoroutine = StartCoroutine(TypeSentence(GetCurrentDialogue()[dialogueIndex]));
            }
            else
            {
                ESCbtn.SetActive(true);
                dialogueUI.SetActive(false);
                isDialogueActive = false;
                TogglePlayerControls(true);
                 if (moveScript != null)
            {
                moveScript.speed = 35f;
            }
            }
        }
    }

    private string[] GetCurrentDialogue()
    {
        int dayIndex = Mathf.Clamp(waveSpawner.currWave - 1, 0, dayDialogues.Length - 1);
        return dayDialogues[dayIndex];
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.06f);
        }

        isTyping = false;
    }

    private void TogglePlayerControls(bool enable)
    {
        if (playerMovement != null) playerMovement.enabled = enable;
        if (mouseLook != null) mouseLook.enabled = enable;
        if (playerAttack != null) playerAttack.enabled = enable;
        if (playerHP != null) playerHP.enabled = enable;

        Cursor.visible = !enable;
        Cursor.lockState = enable ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
