using System.Collections.Generic;
using UnityEngine;

public class ExampleList : MonoBehaviour
{
    //[SerializeField] private string[] stringArray;
    
    private List<string> stringList = new List<string>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Erstes Element aus Array: " + stringArray[0]);
        Debug.Log("Listenlänge: " + stringList.Count);
        
        stringList.Add("Apfel");
        stringList.Add("Birne");
        stringList.Add("Apfel");
        stringList.Add("Kirsche");
        
        Debug.Log("Listenlänge: " + stringList.Count);
        foreach (string str in stringList)
        {
            Debug.Log(str);
        }

        stringList.Remove("Apfel");
        
        //entfernt immer das erste Element in der Liste
        stringList.RemoveAt(0);
        
        //entfernt immer das letzte Element in der Liste
        stringList.RemoveAt(stringList.Count-1);

        Debug.Log("Listenlänge: " + stringList.Count);
        foreach (string str in stringList)
        {
            Debug.Log(str);
        }
        
        //Debug.Log("Erstes Element aus Liste: " + stringList[0]);
    }
}
