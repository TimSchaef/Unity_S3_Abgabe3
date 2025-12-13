using UnityEngine;

public class ArrayAndLists : MonoBehaviour
{
    [SerializeField] private string searchString;
    [SerializeField] private string[] manyStrings;
    //we can set the length of the array with initializing it = new type(eg string) [length]
    private string[] manyStringsInScript = new string[4];
    
    [SerializeField] string myString = "Hallo";
    [SerializeField] private int myInteger = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int length = 5;
        manyStringsInScript = new string[length];
        manyStringsInScript[0] = "Hallo";
        manyStringsInScript[1] = "Hallo1";
        manyStringsInScript[2] = "Hallo2";
        manyStringsInScript[3] = "Hallo3";
        manyStringsInScript[4] = "Hallo4";
        
        Debug.Log(myString);
        
        //we can access elements of an array with writing its name than [] and
        //in the [] the index of the wanted element 
        //Debug.Log(manyStrings[0]);
        //Debug.Log(manyStrings[1]);
        //Debug.Log(manyStrings[2]);
        
        //how long is our array?
        Debug.Log("Länge Array manyStrings: " + manyStrings.Length);

        int searchCounter = 0;
        
        // for (int i = 0; i < manyStrings.Length; i++)
        // {
        //     //Debug.Log(manyStrings[i]);
        //     if (manyStrings[i] == searchString)
        //     {
        //         //Debug.Log("Element " + i + " is a " + searchString);
        //         searchCounter++;
        //     }
        // }
        
        foreach (string stringElement in manyStrings)
        {
            //Debug.Log(stringElement);
            if (stringElement == searchString)
            {
                searchCounter++;
            }
        }
        
        Debug.Log("In my array are " + searchCounter + " " + searchString);
        
        //
        // //ist mein Integer größer 0 oder kleiner gleich 0
        // if (myInteger > 0)
        // {
        //     Debug.Log("Larger than zero");
        // }
        // else
        // {
        //     Debug.Log("smaller or equals zero");
        // }
        //
        // //schleife (FOR-Loop) soll 10 mal durchlaufen werden und den jeweiligen index ausgeben (debug)
        // for (int i = 0; i < 10; i++)
        // {
        //     Debug.Log("Loop is running " + i);
        // }
        
        Debug.Log("Start finished");
    }
}
