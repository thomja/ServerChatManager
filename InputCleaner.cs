using System;

public class InputCleaner
{
    public InputCleaner()
    {
        return;
    }

    public string StringCleaner(string currentString)
    {
        currentString.Replace("Recieving: ", "");
        return currentString;
        
    }
}
