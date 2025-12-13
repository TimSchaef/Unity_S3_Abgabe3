using UnityEngine;

public class ArraysAndLists : MonoBehaviour
{
    [SerializeField] private string searchString;
    [SerializeField] private string[] myManyStrings;
    
    private string[] stringArray;
    
    [SerializeField] private string myString;
    [SerializeField] private int myInteger = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(myString);

        // int length = 4;
        // stringArray = new string[length];
        // stringArray[0] = "Hallo!";
        // stringArray[1] = "Hallo1!";
        // stringArray[2] = "Hallo2!";
        // stringArray[3] = "Hallo3!";
        
        Debug.Log("Length of myManyStrings: " + myManyStrings.Length);
        
        // Debug.Log(myManyStrings[0]);
        // Debug.Log(myManyStrings[1]);
        // Debug.Log(myManyStrings[2]);

        int searchCounter = 0;
        
        //simple for-loop
        // for (int i = 0; i < myManyStrings.Length; i++)
        // {
        //     //Debug.Log(myManyStrings[i]);
        //     if (myManyStrings[i] == searchString)
        //     {
        //         //Debug.Log("Index " + i + " ist ein/e " + searchString);
        //         searchCounter++;
        //     }
        // }
        
        //for-each-loop
        //a foreach loop runs through every element of a collection (e.g. list or array)
        //for each element the code below gets executed
        //the foreach loop saves the current element in an own local variable
        //we can name it as we want, but it needs to be the same type (e.g. string, int, GameObject etc.) as the elements
        //  in the collection
        //we can use this variable in the loop as we like (e.g. compare it with 'if', modify it or print it with Debug.Log)
        foreach (string s in myManyStrings)
        {
            //Debug.Log(s);
            if (s == searchString)
            {
                //Debug.Log("Index " + i + " ist ein/e " + searchString);
               searchCounter++;
            }
        }
        
        Debug.Log("There are " + searchCounter + " " + searchString + " in the array");
        
        // if (myInteger > 0)
        // {
        //     Debug.Log("MyInt is larger than 0");
        // }else if (myInteger < 0)
        // {
        //     Debug.Log( "MyInt is smaller than 0");
        // }
        // else
        // {
        //     Debug.Log("MyInt is equal 0");
        // }
        //
        // //Einen For-Loop erstellen, welcher 10 mal durchlaufen wird
        // //und immer den momentanen index in die Console schreibt
        // for (int i = 0; i < 10; i++)
        // {
        //     Debug.Log("Loop is running " + i);
        // }
        
        Debug.Log("Start finished");
    }
}
