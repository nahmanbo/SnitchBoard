namespace SnitchBoard.Models;

public class Report
{
    public (string FirstName, string LastName)? ReporterName { get; } 
    public (string FirstName, string LastName) ReportedName { get; } 
    public string? ReporterIdNumber { get; } 
    public string Text { get; }

    //====================================
    public Report(string reporterIdNumber, (string, string) reported, string text)
    {
        ReporterName = null;
        ReportedName = reported;
        ReporterIdNumber = reporterIdNumber;
        Text = text;
    }
    
    //====================================
    public Report((string, string) reporter, (string, string) reported, string text)
    {
        ReporterName = reporter;
        ReportedName = reported;
        ReporterIdNumber = null;
        Text = text;
    }
    
    //====================================
    public Report()
    {
        // Empty constructor for cases where the controller handles the creation
    }

    //--------------------------------------------------------------
    public string GetUserNameOrId()
    {
        Console.Write("Enter name or Id: ");
        return Console.ReadLine()!.Trim();
    }
    
    //--------------------------------------------------------------
    public static Report CreateReportWithValidId(string validatedId)
    {
        Console.Write("Enter the name of the person youre reporting: ");
        string reportedPersonName = Console.ReadLine()!.Trim();
        
        Console.Write("Enter your report: ");
        string reportText = Console.ReadLine()!.Trim();
        
        (string FirstName, string LastName) reported = SplitFullName(reportedPersonName);
        
        return new Report(validatedId, reported, reportText);
    }
    
    //--------------------------------------------------------------
    public static Report CreateReportWithName(string reporterName)
    {
        Console.Write("Enter the name of the person you're reporting: ");
        string reportedPersonName = Console.ReadLine()!.Trim();
        
        Console.Write("Enter your report: ");
        string reportText = Console.ReadLine()!.Trim();
        
        (string FirstName, string LastName) reporter = SplitFullName(reporterName);
        (string FirstName, string LastName) reported = SplitFullName(reportedPersonName);
        
        return new Report(reporter, reported, reportText);
    }

    
    //--------------------------------------------------------------
    private static (string FirstName, string LastName) SplitFullName(string fullName)
    {
        string[] nameParts = fullName.Split(' ');
        
        if (nameParts.Length == 0)
            return (string.Empty, string.Empty);
        
        string firstName = nameParts[0];
        string lastName = string.Join(" ", nameParts.Skip(1));
        
        return (firstName, lastName);
    }
    
    //--------------------------------------------------------------
    public override string ToString()
    {
        string reporterInfo = ReporterIdNumber != null 
            ? $"Reporter ID: {ReporterIdNumber}" 
            : $"Reporter: {ReporterName?.FirstName} {ReporterName?.LastName}";
            
        return $"{reporterInfo}\n" +
               $"Reported: {ReportedName.FirstName} {ReportedName.LastName}\n" +
               $"Report: {Text}";
    }
}