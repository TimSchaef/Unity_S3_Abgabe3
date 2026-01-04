using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class InkDialogue : MonoBehaviour
{
    [Header("Ink (wird vom QuestGiver gesetzt)")]
    [SerializeField] private TextAsset inkJSONAsset;

    [Header("UI Root")]
    [SerializeField] private GameObject panelRoot;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Continue Button (extra)")]
    [SerializeField] private GameObject continueRoot; // optional parent
    [SerializeField] private Button continueButton;

    [Header("Single Choice Button")]
    [SerializeField] private GameObject choiceRoot; // optional parent
    [SerializeField] private Button choiceButton;
    [SerializeField] private TextMeshProUGUI choiceButtonText;

    private Story story;
    private bool dialogueActive;

    public bool IsActive => dialogueActive;

    private void Awake()
    {
        if (panelRoot != null) panelRoot.SetActive(false);

        if (speakerText != null) speakerText.text = "";
        if (dialogueText != null) dialogueText.text = "";

        HideChoice();
        HideContinue();

        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(Continue);
        }

        if (choiceButton != null)
        {
            choiceButton.onClick.RemoveAllListeners();
            choiceButton.onClick.AddListener(OnClickChoice);
        }
    }

    public void SetInk(TextAsset ink) => inkJSONAsset = ink;

    public void StartDialogue()
    {
        if (dialogueActive) return;

        if (inkJSONAsset == null)
        {
            Debug.LogError("InkDialogue: Kein Ink JSON Asset zugewiesen!");
            return;
        }

        story = new Story(inkJSONAsset.text);
        dialogueActive = true;

        if (panelRoot != null) panelRoot.SetActive(true);

        if (speakerText != null) speakerText.text = "";
        if (dialogueText != null) dialogueText.text = "";

        HideChoice();
        ShowNext();
    }

    // Wird vom extra Continue-Button aufgerufen
    public void Continue()
    {
        if (!dialogueActive || story == null) return;

        // Wenn Choice da ist: NICHT weiterklicken, nur Choice-Button zulassen
        if (HasChoices())
        {
            RenderSingleChoice();
            return;
        }

        ShowNext();
    }

    private void ShowNext()
    {
        HideChoice();

        string nextLine = GetNextNonEmptyLine();

        if (nextLine != null)
        {
            ParseAndDisplayLine(nextLine);

            if (HasChoices())
            {
                RenderSingleChoice();
                HideContinue();
            }
            else
            {
                ShowContinue();
            }

            return;
        }

        if (HasChoices())
        {
            RenderSingleChoice();
            HideContinue();
            return;
        }

        EndDialogue();
    }

    private string GetNextNonEmptyLine()
    {
        while (story.canContinue)
        {
            string raw = story.Continue();
            if (raw == null) continue;

            string trimmed = raw.Trim();
            if (!string.IsNullOrEmpty(trimmed))
                return trimmed;
        }

        return null;
    }

    private void ParseAndDisplayLine(string raw)
    {
        // "Speaker: Text" oder ohne ":" -> Speaker leer
        string speaker = "";
        string text = raw;

        int idx = raw.IndexOf(':');
        if (idx > 0 && idx < raw.Length - 1)
        {
            speaker = raw.Substring(0, idx).Trim();
            text = raw.Substring(idx + 1).Trim();
        }

        if (speakerText != null) speakerText.text = string.IsNullOrEmpty(speaker) ? "" : $"[{speaker}]";
        if (dialogueText != null) dialogueText.text = text;
    }

    private bool HasChoices()
    {
        return story != null && story.currentChoices != null && story.currentChoices.Count > 0;
    }

    private void RenderSingleChoice()
    {
        if (!HasChoices())
        {
            HideChoice();
            return;
        }

        var choice = story.currentChoices[0]; // single-choice: erste Choice
        if (choiceButtonText != null)
            choiceButtonText.text = choice.text.Trim();

        if (choiceRoot != null) choiceRoot.SetActive(true);
        if (choiceButton != null) choiceButton.gameObject.SetActive(true);

        if (choiceButton != null) choiceButton.interactable = true;
    }

    private void HideChoice()
    {
        if (choiceButton != null) choiceButton.gameObject.SetActive(false);
        if (choiceRoot != null) choiceRoot.SetActive(false);
    }

    private void ShowContinue()
    {
        if (continueRoot != null) continueRoot.SetActive(true);
        if (continueButton != null) continueButton.gameObject.SetActive(true);
        if (continueButton != null) continueButton.interactable = true;
    }

    private void HideContinue()
    {
        if (continueButton != null) continueButton.gameObject.SetActive(false);
        if (continueRoot != null) continueRoot.SetActive(false);
    }

    private void OnClickChoice()
    {
        if (!dialogueActive || story == null) return;
        if (!HasChoices()) return;

        story.ChooseChoiceIndex(0);

        HideChoice();
        ShowNext();
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        story = null;

        HideChoice();
        HideContinue();

        if (panelRoot != null) panelRoot.SetActive(false);
        if (speakerText != null) speakerText.text = "";
        if (dialogueText != null) dialogueText.text = "";
    }
}







