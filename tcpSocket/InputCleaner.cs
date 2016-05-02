using System;

public class InputCleaner
{

    public string StringCleaner(string currentString)
    {
        currentString.Replace("Recieving: ", "");
        return currentString;

    }
}
