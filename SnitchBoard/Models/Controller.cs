namespace SnitchBoard.Models; 
public class Controller
    {
        private readonly ReportDal _dal = new ReportDal();

        //====================================
        public Controller()
        {
            RunMenu();
        }

        //--------------------------------------------------------------
        public void RunMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Add agent");
                Console.WriteLine("2. Delete agent");
                Console.WriteLine("3. Change agent location");
                Console.WriteLine("4. Change agent status");
                Console.WriteLine("5. Get all agents");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice (1-6): ");

                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        InsertReport();
                        break;

                    case "2":
                        break;

                    case "3":
                        break;

                    case "4":
                        break;

                    case "5":

                        break;

                    case "6":
                        _dal.CloseConnection();
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                        break;
                }
            }
        }

        //--------------------------------------------------------------
        private void InsertReport()
        {
            Report report = new Report();

            do
            {
                report.ReadInput();
                if (!report.IsValid())
                {
                    Console.WriteLine("Invalid format. Please use: Your Name | Reported Name | The Report");
                }
            } while (!report.IsValid());

            string[] parts = report.GetSplitParts();
            _dal.AddReportByNames(parts);

            Console.WriteLine("✅ Report added successfully.");
        }

    }

