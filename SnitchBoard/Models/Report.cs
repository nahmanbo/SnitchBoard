namespace SnitchBoard.Models;

public class Report
{
    public (string FirstName, string LastName)? ReporterName { get; }
    public (string FirstName, string LastName) ReportedName { get; }
    public string? ReporterIdNumber { get; }
    public string Text { get; }

    //====================================
    // Constructor for reporter using ID
    public Report(string reporterIdNumber, (string, string) reported, string text)
    {
        ReporterName = null;
        ReportedName = reported;
        ReporterIdNumber = reporterIdNumber;
        Text = text;
    }

    //====================================
    // Constructor for reporter using full name
    public Report((string, string) reporter, (string, string) reported, string text)
    {
        ReporterName = reporter;
        ReportedName = reported;
        ReporterIdNumber = null;
        Text = text;
    }

    //====================================
    // Empty constructor for controller-based creation
    public Report()
    {
        // empty
    }

    //--------------------------------------------------------------
    // Prompts user to enter name or ID
    public string GetUserNameOrId()
    {
        Console.Write("Enter name or Id: ");
        return Console.ReadLine()!.Trim();
    }

    //--------------------------------------------------------------
    // Creates a report using a validated reporter ID
    public static Report CreateReportWithValidId(string validatedId)
    {
        Console.Write("Enter the name of the person you're reporting: ");
        string reportedPersonName = Console.ReadLine()!.Trim();

        Console.Write("Enter your report: ");
        string reportText = Console.ReadLine()!.Trim();

        (string FirstName, string LastName) reported = SplitFullName(reportedPersonName);

        return new Report(validatedId, reported, reportText);
    }

    //--------------------------------------------------------------
    // Creates a report using the reporter's name
    public static Report CreateReportWithName(string reporter)
    {
        Console.Write("Enter the name of the person you're reporting: ");
        string reportedPersonName = Console.ReadLine()!.Trim();

        Console.Write("Enter your report: ");
        string reportText = Console.ReadLine()!.Trim();

        (string FirstName, string LastName) reported = SplitFullName(reportedPersonName);
        (string FirstName, string LastName) reporterName = SplitFullName(reporter);

        return new Report(reporterName, reported, reportText);
    }

    //--------------------------------------------------------------
    // Splits full name into first and last name
    private static (string FirstName, string LastName) SplitFullName(string fullName)
    {
        string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (nameParts.Length == 0)
            return (string.Empty, string.Empty);

        string firstName = nameParts[0];
        string lastName = string.Join(" ", nameParts.Skip(1));

        return (firstName, lastName);
    }

    //--------------------------------------------------------------
    // Returns a string representation of the report
    public override string ToString()
    {
        string reporterInfo = ReporterIdNumber != null
            ? $"Reporter ID: {ReporterIdNumber}"
            : $"Reporter name: {ReporterName?.FirstName} {ReporterName?.LastName}";

        return $"{reporterInfo}\n" +
               $"Reported: {ReportedName.FirstName} {ReportedName.LastName}\n" +
               $"Report: {Text}";
    }
}
