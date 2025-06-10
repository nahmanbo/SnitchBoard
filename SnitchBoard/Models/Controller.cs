namespace SnitchBoard.Models;

public class Controller
{
    private readonly DabManager _dal;

    //====================================
    public Controller()
    {
        _dal = new DabManager();
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
        Console.WriteLine("6. Exit");
    }

    //--------------------------------------------------------------
    private void HandleAddReport()
    {
        Report reportCreator = new Report();
        Report report = reportCreator.CreateFromInput();
        _dal.AddReport(report);
        Console.WriteLine("Report added successfully.");
    }
}