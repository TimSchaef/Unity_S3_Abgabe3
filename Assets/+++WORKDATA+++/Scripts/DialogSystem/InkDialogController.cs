using System;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class InkDialogController : MonoBehaviour
{
    //wir müssen die json-Datei, welche bei der ink-Datei liegt, mit dem Script verknüpfen
    //dass können wir über ein TextAsset machen -> mit einem TextAsset kann man verschiedene Text-Dateien in
    //einem Script nutzen (z.B. .txt, .xml, .json)
    [SerializeField] private TextAsset inkAsset;
    
    //eine Story bekommen wir von dem ink-package -> sie ist der Kern und beinhaltet unsere komplette in ink
    //geschriebenen Geschichte/Dialog. Alle Aktionen und Interaktionen mit dieser Geschichte werden über die
    //Story (-Variable) gemacht
    private Story inkStory;
    
    private void Awake()
    {
        //natülich muss die story noch wissen, welchen Inhlat sie nutzen soll - in diesem Fall unsern ink-file
        //über das .text greifen wir auf den Text in den Text-Asset zu
        inkStory = new Story(inkAsset.text);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //damit am Anfang kein leeres Blatt vor uns ist, versuchen wir direkt beim Start und die erste Zeile zu holen
        TryContinueInkStory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //wir haben das weiterführen der Geschichte in eine Funktion ausgelagert, damit wir es von verschiedenen
            //Stellen im Code abrufen können
            TryContinueInkStory();
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            //auch hier haben wir das Auswählen einer Choice in eine Funktion ausgelagert.
            //als Argument der Funktion übergeben wir den Index, welche Choice wir auswählen möchten
            SelectInkChoice(0);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelectInkChoice(1);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SelectInkChoice(2);
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SelectInkChoice(3);
        }
    }

    
    void TryContinueInkStory()
    {
        //wenn die nächste Zeile bereitsteht, dann weiter...
        if (inkStory.canContinue)
        {
            Debug.Log(inkStory.Continue());
            
            //falls unser jetziger Abschnitt Tags hat, bitte entsprechend handeln
            if (inkStory.currentTags.Count > 0)
            {
                foreach (string currentTag in inkStory.currentTags)
                {
                    if (currentTag == "Alex")
                    {
                        Debug.Log("Alex is speaking");
                        //setze den Sprite von Alex
                    }else if (currentTag == "Mia")
                    {
                        //setze den Sprite von Mia
                        Debug.Log("Mia is speaking");
                    }
                }
            }
        }

        //falls es jetzt gerade mögliche Choices gibt, dann diese durchgehen und ins Log ausgeben
        if (inkStory.currentChoices.Count > 0)
        {
            for (int i = 0; i < inkStory.currentChoices.Count; i++)
            {
                Choice currentChoice = inkStory.currentChoices[i];
                Debug.Log("Choice " + i + ": " + currentChoice.text);
            }
        }
    }

    void SelectInkChoice(int choiceIndex)
    {
        if (inkStory.currentChoices.Count > 0)
        {
            inkStory.ChooseChoiceIndex(choiceIndex);
            TryContinueInkStory();
        }
    }

    void JumpToInkKnot()
    {
        inkStory.ChoosePathString("Neustadt.Bibliothek"); //Das ist ein Beispiel und in unserem ink-script nicht funktional
        TryContinueInkStory();
    }
}
