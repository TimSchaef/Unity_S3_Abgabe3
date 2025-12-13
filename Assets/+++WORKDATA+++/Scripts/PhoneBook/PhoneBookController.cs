using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBookController : MonoBehaviour
{
    private List<string> nameList = new List<string>();

    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform parentText;

    [SerializeField] private TMP_InputField inputfield;
    [SerializeField] private Button buttonAddNameToList;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonAddNameToList.onClick.AddListener(AddNameToList);
        
        nameList.Add("Alex");
        nameList.Add("Mia");
        nameList.Add("John");
        nameList.Add("Luna");
        nameList.Add("Ethan");
        nameList.Add("Ava");
        nameList.Add("Kai");
        nameList.Add("Zara");
        nameList.Add("Leo");
        nameList.Add("Nora");

        UpdateListView();
    }

    void AddNameToList()
    {
        if (!string.IsNullOrEmpty(inputfield.text))
        {
            nameList.Add(inputfield.text);
            inputfield.text = "";
            UpdateListView();
        }
    }

    void UpdateListView()
    {
        foreach (Transform transformTextElement in parentText)
        {
            Destroy(transformTextElement.gameObject);
        }
        
        foreach (string stringName in nameList)
        {
            GameObject gameObject = Instantiate(textPrefab, parentText);
            gameObject.GetComponent<TextMeshProUGUI>().text = stringName;
        }
    }
}
