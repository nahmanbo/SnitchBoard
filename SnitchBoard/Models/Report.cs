namespace SnitchBoard.Models;

public class Report
{
    public string ReporterName { get; }
    public string ReportedName { get; }
    public string Text { get; }

    //====================================
    private Report(string reporter, string reported, string text)
    {
        ReporterName = reporter;
        ReportedName = reported;
        Text = text;
    }

    //--------------------------------------------------------------
    public static Report CreateFromInput()
    {
        string input;
        string[] parts;

        do
        {
            input = ReadRawInput();
            parts = SplitInput(input);

            if (!IsValid(parts))
                Console.WriteLine("Invalid format. Please use: Your Name | Reported Name | The Report");

        } while (!IsValid(parts));

        return CreateReportFromParts(parts);
    }

//--------------------------------------------------------------
    private static string ReadRawInput()
    {
        Console.WriteLine("Insert Report (format: Your Name | Reported Name | The Report):");
        return Console.ReadLine()?? "";
    }

    //--------------------------------------------------------------
    private static string[] SplitInput(string input)
    {
        return input.Split('|');
    }

    //--------------------------------------------------------------
    private static bool IsValid(string[] parts)
    {
        return parts.Length == 3;
    }

    //--------------------------------------------------------------
    private static Report CreateReportFromParts(string[] parts)
    {
        string reporter = parts[0].Trim();
        string reported = parts[1].Trim();
        string text = parts[2].Trim();

        return new Report(reporter, reported, text);
    }


    //--------------------------------------------------------------
    public string[] GetSplitParts()
    {
        return new[] { ReporterName, ReportedName, Text };
    }
}