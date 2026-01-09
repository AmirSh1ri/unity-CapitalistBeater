// handles NPC interaction, dialogue progression, and player disabling during conversation

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KittyNPC : MonoBehaviour
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
    private UnityEngine.AI.NavMeshAgent agent;
    public PlayAnim kittyAnim;

    private int dialogueIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private string[][] dayDialogues = new string[][]
    {
        new string[] { "", "Bugger off, twat.", "..." },
        new string[] { "", "What bin did you crawl out from?", "..." },
        new string[] { "", "As long as you keep fantom away from me you can stay.",
            "I told the brat showering was a bloody mistake, he just kept talking.",
            "Come to think of it... it's a blessing you're mute.", "..." },
        new string[] { "", "Needa tell you something...",
            "Whatever you do, don't trust that coin sniffling husk.",
            "Still can't figure how he lives...", "..." },
        new string[] { "", "Didn't expect you to make it this far, getting less irritating by the day.",
            "You're somewhat tolerable now.", "..." },
        new string[] { "", "It's been a while since I’ve endured your presence. Guess it’s time you hear about my origin.",
            "Well... it all started with me and my dad, Fantom. The bloke was a right character.",
            "Then he took a shower and ruined everything. Bloody wanker.",
            "The name? Don’t ask me. I’m half black, so the whole ‘Milky’ thing doesn’t even make sense.",
            "Did you expect me to be some fantasy king? I lick my own butt.", "..." },

        new string[] { 
            "Well that was a hell of a night..",
            "Two more nights? Can't say I won't miss the company..",
            "...Just don’t expect to pet me or some rubbish like that.",
            "..."
         },
        new string[] { 
            "",
            "So the bony bastard gave you the final speech, huh?",
            "Don’t let that bag of bones get in your head...",
            "Not 'cause I care, I just got used to your silent, weird shaped face.",
            "..."
         },
                  new string[] { 
            "",
            "..."
            
         },
        new string[] { 
            "Oh well, The final night...",
            "Thought you'd be dead like 4 days ago.",
            "Can’t say I care what you choose. End the world, bend the knee... whatever suits you.",
            "Fantom'll be alright, I'll be with him, not with a lot of enjoyment.",
            "He deserved better than all this.. this place too.",
            "Guess I will miss having you around after all.",
            "..."

         },
         new string[] { 
            "",
            "..."
            
         }
        
    };

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = 0f;
        kittyAnim = GetComponent<PlayAnim>();
        nameText.text = "Milky the IV";
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

    // triggers next dialogue line or ends dialogue
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

                if (agent != null && kittyAnim != null)
                {
                    kittyAnim.enabled = true;
                    agent.speed = 35f;
                }
            }
        }
    }

    // gets dialogue for current wave
    private string[] GetCurrentDialogue()
    {
        int dayIndex = Mathf.Clamp(waveSpawner.currWave - 1, 0, dayDialogues.Length - 1);
        return dayDialogues[dayIndex];
    }

    // types dialogue letter by letter
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

    // enables/disables player control and locks cursor
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
