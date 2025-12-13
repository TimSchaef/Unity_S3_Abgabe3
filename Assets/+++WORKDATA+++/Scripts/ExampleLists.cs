using System.Collections.Generic;
using UnityEngine;

public class ExampleLists : MonoBehaviour
{
    //a list must be initilised -> new List<type> OR via the Inspector with [SerializeField]
    private List<string> stringList = new List<string>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start!");
        
        //get the length of a list: list.Count
        Debug.Log("Listenlänge: " + stringList.Count);
        
        //add an element at the end of the list: list.Add(element)
        stringList.Add("Banane");
        stringList.Add("Apfel");
        stringList.Add("Birne");
        stringList.Add("Apfel");
        Debug.Log("Listenlänge: " + stringList.Count);

        //running through a list with a foreach-loop
        foreach (string stringObst in stringList)
        {
            Debug.Log(stringObst);
        }

        //remove the first element found: list.Remove(element)
        stringList.Remove("Apfel");
        
        //remove an element at a specific index in the list: list.RemoveAt(index)
        stringList.RemoveAt(0);
        
        Debug.Log("Listenlänge: " + stringList.Count);
        
        foreach (string stringObst in stringList)
        {
            Debug.Log(stringObst);
        }

        //you can easily find out, if an element is in the list: list.Contains(element) -> returns true OR false
        if (stringList.Contains("Apfel"))
        {
            Debug.Log("Es ist mindestens ein Apfel übrig");
        }
    }
}
