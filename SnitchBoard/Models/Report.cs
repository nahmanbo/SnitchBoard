using System;
using System.Linq;

namespace SnitchBoard.Models;

public class Report
{
    public (string FirstName, string LastName) ReporterName { get; }
    public (string FirstName, string LastName) ReportedName { get; } 
    public string Text { get; }

    //====================================
    public Report((string, string) reporter, (string, string) reported, string text)
    {
        ReporterName = reporter;
        ReportedName = reported;
        Text = text;
    }
    
    //====================================
    public Report()
    {
        Report created = CreateFromInput();
        ReporterName = created.ReporterName;
        ReportedName = created.ReportedName;
        Text = created.Text;
    }

    //--------------------------------------------------------------
    public Report CreateFromInput()
    {
        string input;
        string[] parts;

        do
        {
            input = ReadInput();
            parts = SplitInput(input);

            if (!IsValid(parts))
                Console.WriteLine("Invalid format. Please use: Your Name | Reported Name | The Report");

        } while (!IsValid(parts));

        return CreateReportFromParts(parts);
    }

    //--------------------------------------------------------------
    private string ReadInput()
    {
        Console.WriteLine("Insert Report (format: Your Name | Reported Name | The Report):");
        return Console.ReadLine()!;
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
    private Report CreateReportFromParts(string[] parts)
    {
        (string FirstName, string LastName) reporter = SplitFullName(parts[0].Trim());
        (string FirstName, string LastName) reported = SplitFullName(parts[1].Trim());
        string text = parts[2].Trim();

        return new Report(reporter, reported, text);
    }
    
    //--------------------------------------------------------------
    private (string FirstName, string LastName) SplitFullName(string fullName)
    {
        string[] nameParts = fullName.Split(' ');
        
        if (nameParts.Length == 0)
            return (string.Empty, string.Empty);
        
        string firstName = nameParts[0];
        string lastName = string.Join(" ", nameParts.Skip(1));
        
        return (firstName, lastName);
    }
    
    //--------------------------------------------------------------
    public object[] GetSplitParts()
    {
        return new object[] { ReporterName, ReportedName, Text };
    }
}