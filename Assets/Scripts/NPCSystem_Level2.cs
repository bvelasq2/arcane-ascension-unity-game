using UnityEngine;
using TMPro;

public class NPCSystem : MonoBehaviour
{
    private bool playerDetection = false;
    private bool dialogueShown = false;

    [Header("UI References")]
    public GameObject talkPromptUI;      // "Press F to Talk" text
    public GameObject dialogueBoxUI;     // Background panel
    public TMP_Text dialogueText;        // TMP text inside dialogue panel

    [Header("Dialogue")]
    [Tooltip("Set the dialogue text for this NPC. Leave empty if you want to use the text already on the TextMeshPro component.")]
    public string npcDialogue;

    void Start()
    {
        // Ensure both UI elements are hidden at the start
        if (talkPromptUI != null) talkPromptUI.SetActive(false);
        if (dialogueBoxUI != null) dialogueBoxUI.SetActive(false);
    }

    void Update()
    {
        if (playerDetection && Input.GetKeyDown(KeyCode.F))
        {
            if (!dialogueShown)
            {
                ShowDialogue();
            }
        }

        // Optionally, close dialogue with Space
        if (dialogueShown && Input.GetKeyDown(KeyCode.Space))
        {
            HideDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (dialogueBoxUI != null)
            dialogueBoxUI.SetActive(true);

        if (talkPromptUI != null)
            talkPromptUI.SetActive(false);

        // Update the TMP text only if a dialogue string is provided.
        if (dialogueText != null && !string.IsNullOrEmpty(npcDialogue))
        {
            dialogueText.text = npcDialogue;
        }

        dialogueShown = true;
    }

    private void HideDialogue()
    {
        if (dialogueBoxUI != null)
            dialogueBoxUI.SetActive(false);

        dialogueShown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetection = true;
            if (talkPromptUI != null)
                talkPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetection = false;
            if (talkPromptUI != null)
                talkPromptUI.SetActive(false);

            HideDialogue(); // Hide dialogue if player walks away
        }
    }
}
