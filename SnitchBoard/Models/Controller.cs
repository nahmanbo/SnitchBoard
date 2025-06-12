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
                case "2":
                    _dal.PrintAllGoodReporter();
                    break;
                case "3":
                    _dal.PrintAllDangerousReported();
                    break;
                case "4":
                    Console.WriteLine("Exiting...");
                    _dal.Close();
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
        Console.WriteLine("2. Show all optional agents");
        Console.WriteLine("3. Show all reported dangerous people");
        Console.WriteLine("4. Exit");
    }

    //--------------------------------------------------------------
    private void HandleAddReport()
    {
        Report report = new Report();
        _dal.AddReport(19, report);
        Console.WriteLine("Report added successfully.");
    }
}