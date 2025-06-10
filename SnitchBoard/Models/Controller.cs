namespace SnitchBoard.Models;

public class Controller
{
    private readonly ReportDal _dal;

    //====================================
    public Controller()
    {
        _dal = new ReportDal();
    }

    //--------------------------------------------------------------
    public void Run()
    {
        bool exit = false;

        while (!exit)
        {
            ShowMainMenu();
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    HandleAddReport();
                    break;

                case "6":
                    Console.WriteLine("Exiting...");
                    _dal.CloseConnection();
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    //--------------------------------------------------------------
    private void ShowMainMenu()
    {
        Console.WriteLine("\n--- Main Menu ---");
        Console.WriteLine("1. Add agent report");
        Console.WriteLine("6. Exit");
    }

    //--------------------------------------------------------------
    private void HandleAddReport()
    {
        Report report = Report.CreateFromInput();
        _dal.AddReportByNames(report.GetSplitParts());
        Console.WriteLine("Report added successfully.");
    }
}