using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Transform player;
    public GameObject interactUI;
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public GameObject shopUI;
    public waveSpawner waveSpawner; 
    public Shop shopManager;
    public PlayerMovement playerMovement;
    public MouseLook mouseLook;
    public Attack playerAttack;
    public GameObject ESCbtn;
    public float interactRange = 10f;
    public GameObject EndUI;
    public Image imageToFade;
    private CurrencyHolder currencyHolder;
    private int dialogueIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private float tempJmp;
 
    private string[][] dayDialogues = new string[][]
    {
        new string[] 
        { "","How did he not di.. ",
        "Ah, a new face among the rubble. Welcome, welcome.",
    "Survived the fall and the beasts? Impressive indeed.",
    "But don’t mistake survival for success, life runs on a new currency now: skulls.",
    "And lucky you, I just happen to be in the market for them.",
    "...Not much of a talker, are you? Mute, perhaps? Hm. Suits me just fine.", 
    "Think of me as a... career opportunity.",
    "You collect skulls. I give you tools, and share the magic of the Lady with you.",
    "Let’s see if you’re worth the investment, shall we?",
    "Now go. Prove yourself useful."
    ,"I'll tell you more tomorrow, that is if you make it out alive.",
    "..."
        },
        new string[] { "" ,
    "Oh... wasn’t expecting to see you again. You're tougher than the rest of them..",
    "Ah, but now that you've survived your first night, I suppose you’d like to know how all of this came to be.",
    "There was an asteroid, massive. Shattered the planet into countless pieces.",
    "The Lady helped us, these islands were spared destruction... at a cost",
    "Most people didn’t die... they changed. Twisted into monsters wandering their old lives.",
    "Not all. The classy few with top hats? We left flesh behind.",
    "Now we’re skeletons, yes, but thinking ones, sophisticated ones. A gentleman’s curse, if you will.",
    "Naturally, we did what any self respecting, newly immortal individuals would do, we became pirates. There’s still plenty to take, after all.",
    "Yes, I’ve got no legs. You noticed. Had to amputate... diabetes. Tragic, really.",
    "Save your skulls if you like, I'll' pick'em up from your corpse.",
    "..."
        },
        new string[] 
        { "" ,
        "Well, well, if it isn't my favorite skull collector! well... my only one. Not that it matters much, does it?",
    "I've been hearing some unsettling whispers. Other refined pirates claim they’ve seen... things. Strange creatures creeping about.",
    "But never mind all that! It’s time for the best part: skull-counting!",
    "Let's see... wait... THIS is it?!",
    "This can't be right. This isn’t even enough to keep my family... Uh, the crew, my crew, satisfied for a single night!",
    "You might want to step it up tomorrow. If you’re serious about joining my crew.",
    "I must be off. Get what you need, spend what you have, I'll be back tomorrow.",
    "..."
        },
        new string[] { "" ,
       "Ah, there you are! Took your time, didn’t you? Well, good news you’re in luck.",
    "Turns out, we found a rather rare skull among the ones you handed over. Worth twenty times the usual! Imagine that!",
    "That little gem made up for your lack of efforts.",
    "Oh, and rumors are spreading fast. More of those creatures are being sighted. Try not to become their next snack, alright?",
    "Now then, I’m setting sail soon. Grab what you need to, you know, stay alive and all that.",
    "The ship? Oh, right.. still, uh, full. But don’t you worry, I’ll make some space for you in a few days. I promise.",
    "..."
         },
        new string[] {"" ,
        "Greetinngs again, good to see you alive" ,
        "I gotta get going fast today. Just grab what you need and leave" ,
        "We'll talk more tommorow." ,
        "...",
        },
        // Capitalist Skeleton
        new string[] {"" ,
        "Finally, you show up. I was starting to worry about my profi.. you.",
    "Listen, I’ve been thinking... this whole occasional skull collecting thing? It’s cute. Adorable, really. But it’s not enough.",
    "You see, Lady is demanding more, and what she wants, we deliver. Otherwise, this little safe zone might not be so safe anymore.",
    "So, I’ve decided, CONGRATULATIONS! Skull collecting is now your FULL TIME occupation. No vacations, no sick days, just good ol’ fashioned survival.",
    "Isn't that exciting? Do you know how much more skulls we'll fin...",
    "Oh, don’t give me that look. Do you think I like doing this? Sailing between islands, bartering with skeletons, keeping my crew in line?",
    "Now, you best pick up the pace. More skulls, better deals, and soon you’ll get a real spot on my ship, we'll go see the main town and everything.",
    "Go on, then. Get your supplies and get hunting. We both have obligations, don’t we?",
    "..."
    
         },
        new string[] {"" ,
        "There you are. I was beginning to think you had developed delusions of independence. ",
    "Look around you. This world isn't kind to those who hesitate. The peasants who lived here? Gone.",
    "The monsters who wander aimlessly? people who failed to adapt.",
    "But us? We are survivors. We are thinkers. And we both know what it takes.",
    "Now, I will admit something to you. There is no crew. There never was. I sail alone, scavenging what I can, negotiation with the living, looting the dead.",
    "And you? My most profitable investment yet, I kept you alive for this. ",
    "The skull trade is the only thing keeping us from fading into obscurity.",
    "The Lady demands them, and so long as we deliver, the light remains.",
    "Two more days. That is all I ask. Two more days of unwavering dedication, and I promise, we shall leave this forsaken rock and sail to the main town, where the real people still remain.",
    "But until then, I expect results. No more excuses. No more hesitation.",
    "You will bring me skulls. You will do your part. ",
    "Now, go. Take what you need. Make yourself useful. And remember, only two more days. Do not disappoint me.",
    "..."
         },
        new string[] {"" ,
        "I was wondering if you’d forgotten your responsibilities.",
    "Let me make it clear...",
    "This is not a partnership. It never was. This is your job to collect skulls.",
    "I am the one keeping you alive. You should be grateful.",
    "Oh, what’s that look for? Yes, you work a 9 to 5 for a talking skeleton. So what?",
    "You do your job, and I allow you to keep breathing. Seems like a fair trade to me.",
    "Tomorrow is the last day. A special day. The most important one yet.",
    "I expect you to bring me every last skull you can carry.",
    "I need them. And so do you. Because after tomorrow, we finally leave this wretched island.",
    "So take care of yourself, will you? It would be such a shame if you suddenly became... replaceable.",
    "Now go. Bring me what I need.",
    "..."
         },
        new string[] {"" ,
          "Ah, there you are. I wasn’t sure you'd make it, but here we are, a mountain of skulls and a dream nearly fulfilled.",
    "Thanks to you, I can finally afford my masterpiece, a ship so grand it’ll make the sea weep. Silk-lined quarters, gold sails, even left-side steering—refined to the last detail.",
    "And yes... The Lady is real. Very real. She's the reason we're not both ash and bone.",
    "She demands skulls, and you delivered. That’s why I kept you alive. That’s why I fed you lies about crew and cause.",
    "But now? Now I have what I need. The ship. The offering. And your part in this... is nearly complete.",
    "What’s that look? Betrayal? Please. You were never a partner. You were a vessel. A tool.",
    "What's that? You think you can walk away? You think Lady will simply let you leave? And I thought I was braindead.",
    "Come now, join me to the mainland. There, you'll be sold, yes, sold, to the finest traders as a premier skull collector in service of The Lady.",
    "Some day you might get a ship of yours too.",
    "You should feel honored. Your work will help sustain her collection, her balance... her patience.",
    "And I’d think twice about refusing, when the Lady is angered, the world breaks. You don’t want that, do you?",
    "I'll give you one chance to use your brain and think about it..",
    "..."},
             new string[] { 
                "",
            "Have you decided?",
            "Join us or end it all."
         },
         new string[] { 
            "",
            "You've learned after all.. Come now we'll be living this wreck of a planet."
         },
         new string[] { 
            "",
            "Wh..",
            "What?",
            "I refuse to believe...",
            "I refuse to believe how selfish one can be..",
            "You moron...",
            "...",
            "I've died once, what's another time.."
         }
         
    };

    void Start()
    {
        nameText.text = "Sophisticated Skeleton";
        waveSpawner = FindObjectOfType<waveSpawner>(); 
        tempJmp = playerMovement.jumpHeight;
        currencyHolder = FindObjectOfType<CurrencyHolder>();
    }

    void Update()
    {
        if (player == null || dialogueUI == null || interactUI == null || waveSpawner == null) return;

        float distanceToPlayer = (transform.position - player.position).sqrMagnitude;
        bool inRange = distanceToPlayer <= interactRange * interactRange;

        interactUI.SetActive(inRange);

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
     IEnumerator FadeToTransparentBlack(float duration)
    {
        Color startColor = imageToFade.color;
        Color endColor = new Color(0, 0, 0, 1); // transparent black

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            imageToFade.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }

        imageToFade.color = endColor;
    }
    public void EndingJoin()
    {
        waveSpawner.currWave = 11;
        Debug.Log(waveSpawner.currWave);
        ESCbtn.SetActive(false);
            dialogueUI.SetActive(true);
            interactUI.SetActive(false);
            isDialogueActive = true;
            dialogueIndex = 0;
            TogglePlayerControls(false);
            NextDia();
        
    }

    public void EndingDeny()
    {
        waveSpawner.currWave = 12;
        Debug.Log(waveSpawner.currWave);
        ESCbtn.SetActive(false);
            dialogueUI.SetActive(true);
            interactUI.SetActive(false);
            isDialogueActive = true;
            dialogueIndex = 0;
            TogglePlayerControls(false);
            NextDia();
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
                if(waveSpawner.currWave == 9 || waveSpawner.currWave == 10 || waveSpawner.currWave == 11 || waveSpawner.currWave == 12){
                    dialogueUI.SetActive(false);
                    TogglePlayerControls(true);
                    ESCbtn.SetActive(true);
                    isDialogueActive = false;
                    if(waveSpawner.currWave == 10){
                        EndUI.SetActive(true);
                        TogglePlayerControls(false);
                        ESCbtn.SetActive(false);
                        isDialogueActive = true;
                        Cursor.visible = true;
                    }
                    if(waveSpawner.currWave == 11){
                          StartCoroutine(FadeToTransparentBlack(2f));
                        SceneManager.LoadScene(3);
                    }
                    if(waveSpawner.currWave == 12){
                          StartCoroutine(FadeToTransparentBlack(2f));
                        SceneManager.LoadScene(2);
                    }
                    waveSpawner.currWave = 10;
                    return;
                }
                currencyHolder.AddSkulls(100);
                shopManager.RefreshShop();
                dialogueUI.SetActive(false);
                shopUI.SetActive(true);
                isDialogueActive = false;
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
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    private void TogglePlayerControls(bool enable)
    {
        if (playerMovement != null) playerMovement.enabled = enable;
        if (mouseLook != null) mouseLook.enabled = enable;
        if (playerAttack != null) playerAttack.enabled = enable;

        Cursor.visible = !enable;
        Cursor.lockState = enable ? CursorLockMode.Locked : CursorLockMode.None;
    }
}