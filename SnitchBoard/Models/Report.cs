namespace SnitchBoard.Models;

public class Report
{
    private string _rawInput;

    //====================================
    public Report()
    {
        _rawInput = "";
    }

    //-------------------------------------------------------
    public void ReadInput()
    {
        Console.WriteLine("Insert Report (format: Your Name | Reported Name | The Report):");
        _rawInput = Console.ReadLine()?.Trim() ?? "";
    }

    //-------------------------------------------------------
    public bool IsValid()
    {
        string[] parts = _rawInput.Split('|');
        return parts.Length == 3;
    }

    //-------------------------------------------------------
    public string[] GetSplitParts()
    {
        return _rawInput.Split('|').Select(p => p.Trim()).ToArray();
    }
}