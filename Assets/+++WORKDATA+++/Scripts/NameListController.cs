using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NameListController : MonoBehaviour
{
    private List<string> listNames = new List<string>();

    [SerializeField] private GameObject prefabListElement;
    [SerializeField] private Transform parentList;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button buttonAddName;

// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonAddName.onClick.AddListener(AddNewName);
        
        listNames.Add("Alex");
        listNames.Add("Mia");
        listNames.Add("John");
        listNames.Add("Luna");
        listNames.Add("Ethan");
        listNames.Add("Ava");
        listNames.Add("Kai");
        listNames.Add("Zara");
        listNames.Add("Leo");
        listNames.Add("Nora");

        UpdateListView();
    }

    //die Funktion wird ausgeführt, wenn der Button gedrückt wird
    void AddNewName()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            listNames.Add(inputField.text);
            inputField.text = "";
            UpdateListView();
        }
    }

    void UpdateListView()
    {
        //Erst löschen wir alle Objekte aus der ListView -> damit ist sie leer und wir können sie neu füllen
        foreach (Transform trans in parentList)
        {
            Destroy(trans.gameObject);
        }
        
        //hier erstellen wir für jedes Element in der Liste (listName) ein Objekt (das vorher gespeicherte Prefab)
        //dann holen wir uns mit GetComponent die TextComponente (TextMeshProUGUI) und setzten den Text auf den gerade
        //'aktiven' String aus unserer Liste
        foreach (string listName in listNames)
        {
            //Debug.Log(listName);
            GameObject listElement = Instantiate(prefabListElement, parentList);
            listElement.GetComponent<TextMeshProUGUI>().text = listName;
        }
    }
}
