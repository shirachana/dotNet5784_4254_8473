
using BO;
namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static void Main(string[] args)
        {
            try
            {
                DisplayMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
                DisplayMainMenu();
            }

        }
        static void DisplayMainMenu()
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();

            do
            {
                Console.WriteLine("Main Menu - Select an entity you want to check:");
                Console.WriteLine("1. Dependency");
                Console.WriteLine("2. Engineer");
                Console.WriteLine("3. Task");
                Console.WriteLine("0. Exit");
                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        DisplayEntityMenuE("Engineer");
                        break;
                    case 2:
                        DisplayEntityMenuT("Task");
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (true);
        }

        static void DisplayEntityMenuE(string entityName)
        {
            while (true)
            {
                Console.WriteLine($"{entityName} Menu - Select a method to execute:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read by ID");
                Console.WriteLine("3. Read All");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Delete");
                Console.WriteLine("0. Back to Main Menu");
                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        CreatEngineer();
                        break;
                    case 2:
                        ReadEngineerById();
                        break;
                    case 3:
                        ReadAllEngineers();
                        break;
                    case 4:
                        UpdateEngineer();
                        break;
                    case 5:
                        DeleteEngineer();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void DisplayEntityMenuT(string entityName)
        {
            while (true)
            {
                Console.WriteLine($"{entityName} Menu - Select a method to execute:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read by ID");
                Console.WriteLine("3. Read All");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Delete");
                Console.WriteLine("0. Back to Main Menu");
                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        CreatTask();
                        break;
                    case 2:
                        ReadTaskById();
                        break;
                    case 3:
                        ReadAllTasks();
                        break;
                    case 4:
                        UpdateTask();
                        break;
                    case 5:
                        DeleteTask();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static int GetUserChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            return choice;
        }

        static void CreatEngineer()
        {

            Console.WriteLine(@"Enter id: ");
            string? d = Console.ReadLine();
            int idD = int.Parse(d!);
            Console.WriteLine(@"Enter cost for hour: ");
            string? c = Console.ReadLine();
            double costForHour = double.Parse(c!);
            Console.WriteLine(@"Enter Engineers Email: ");
            string? engineerEmail = Console.ReadLine();
            Console.WriteLine(@"Enter Engineer Name: ");
            string? engineerName = Console.ReadLine();
            Console.WriteLine(@"Enter the the level experience of engineer: ");
            string? l = Console.ReadLine();
            BO.EngineerExperience? engineerLevel = (BO.EngineerExperience?)int.Parse(l!);
            BO.Engineer engineer = new BO.Engineer(idD, costForHour, engineerEmail, engineerName, engineerLevel);
            int crt = s_bl!.Engineer.Create(engineer);
            Console.WriteLine(crt);

        }
        static void ReadEngineerById()
        {

            Console.WriteLine(@"Enter id of Engineer: ");
            string? d = Console.ReadLine();
            int idD = int.Parse(d!);
            BO.Engineer? eng = new BO.Engineer();
            eng = s_bl!.Engineer.Read(idD);
            Console.WriteLine(eng);

        }
        static void ReadAllEngineers()
        {

            IEnumerable<BO.Engineer?> engineers;
            engineers = (IEnumerable<BO.Engineer?>)s_bl!.Engineer.ReadAll();
            foreach (BO.Engineer? engineer in engineers) { Console.WriteLine(engineer); }

        }

        static void UpdateEngineer()
        {

            Console.WriteLine(@"Enter id of Engineer: ");
            string? d = Console.ReadLine();
            int idD = int.Parse(d!);
            BO.Engineer? eng = new BO.Engineer();
            eng = s_bl!.Engineer.Read(idD);
            if (eng != null)
            {
                Console.WriteLine(eng);
                Console.WriteLine(@"Enter cost for hour: ");
                string? c = Console.ReadLine();
                double costForHour = double.Parse(c!);
                Console.WriteLine(@"Enter Engineers Email: ");
                string? engineerEmail = Console.ReadLine();
                Console.WriteLine(@"Enter Engineer Name: ");
                string? engineerName = Console.ReadLine();
                Console.WriteLine(@"Enter the the level experience of engineer: ");
                string? l = Console.ReadLine();
                BO.EngineerExperience engineerLevel = (BO.EngineerExperience)int.Parse(l!);
               BO.Engineer engineer = new BO.Engineer(idD, costForHour, engineerEmail, engineerName, engineerLevel);
                s_bl!.Engineer.Update(engineer);
            }

        }
        static void DeleteEngineer()
        {

            Console.WriteLine(@"Enter id of Engineer to delete: ");
            string? d = Console.ReadLine();
            int idD = int.Parse(d!);
            s_bl!.Engineer.Delete(idD);

        }
        static void CreatTask()
        {
            BO.Task tsk = new BO.Task();
            Random s_rand = new Random();

            Console.WriteLine(@"Enter the engineer ID of the task: ");
            tsk.Id = int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the discription of the task if there: ");
            tsk.Description = Console.ReadLine();
            Console.WriteLine(@"Enter the alias of the task");
            tsk.Alias  = Console.ReadLine();
            Console.WriteLine(@"Enter the level of the task: ");
            tsk.Copmlexity = (BO.EngineerExperience)int.Parse(Console.ReadLine()!);

            Console.WriteLine(@"Enter the deliverables of the task: ");
            string? deliTask = Console.ReadLine();
            Console.WriteLine(@"Enter the remark of the task: ");
            string? taskRemark = Console.ReadLine();
            DateTime start = new DateTime(2024, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _createAtD = start.AddDays(s_rand.Next(range));
            Random random = new();
            int randomDays = random.Next(range);
            DateTime _scheduledDate = start.AddDays(randomDays);
            DateTime _deadLine = new DateTime(2025, 1, 1);
            TimeSpan _requiredET = TimeSpan.FromDays(s_rand.Next((_deadLine - start).Days));
            DateTime _endDate = start.AddDays(randomDays);
            DateTime _startDate = start.AddDays(s_rand.Next((_endDate - _scheduledDate).Days));
            
            int? tk = s_bl!.Task.Create(tsk);
            Console.WriteLine(tk);

        }
        static void ReadTaskById()
        {

            Console.WriteLine(@"Enter id of task: ");
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            BO.Task? tsk = new BO.Task();
            tsk = s_bl!.Task.Read(idT);
            Console.WriteLine(tsk);

        }
        static void ReadAllTasks()
        {

            IEnumerable<BO.Task?> tasks;
            tasks = (IEnumerable<BO.Task?>)s_bl!.Task.ReadAll();
            foreach (BO.Task? task in tasks) { Console.WriteLine(task); }

        }

        static void UpdateTask()
        {
            Random s_rand = new Random();
            BO.Task tskUp = new BO.Task();

            Console.WriteLine(@"Enter id of task: ");
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            BO.Task? tsks = new BO.Task();
            tsks = s_bl!.Task.Read(idT);
            if (tsks != null)
            {
                Console.WriteLine(tsks);
                Console.WriteLine(@"Enter the engineer ID of the task: ");
                string? dT = Console.ReadLine();
                int idTsk = int.Parse(dT!);
                Console.WriteLine(@"Enter the alias of the task");
                string? aliasTask = Console.ReadLine();
                Console.WriteLine(@"Enter the discription of the task if there: ");
                string? discTask = Console.ReadLine();
                Console.WriteLine(@"Enter the level of the task: ");
                string? l = Console.ReadLine();
                DO.EngineerExperience taskLevel = (DO.EngineerExperience)int.Parse(l!);
                Console.WriteLine(@"Enter the deliverables of the task: ");
                string? deliTask = Console.ReadLine();
                Console.WriteLine(@"Enter the remark of the task: ");
                string? taskRemark = Console.ReadLine();
                DateTime start = new DateTime(2024, 1, 1);
                int range = (DateTime.Today - start).Days;
                DateTime _createAtD = start.AddDays(s_rand.Next(range));
                Random random = new();
                int randomDays = random.Next(range);
                DateTime _scheduledDate = start.AddDays(randomDays);
                DateTime _deadLine = new DateTime(2025, 1, 1);
                TimeSpan _requiredET = TimeSpan.FromDays(s_rand.Next((_deadLine - start).Days));
                DateTime _endDate = start.AddDays(randomDays);
                DateTime _startDate = start.AddDays(s_rand.Next((_endDate - _scheduledDate).Days));
                s_bl!.Task.Update(tskUp);
            }

        }
        static void DeleteTask()
        {

            Console.WriteLine(@"Enter id of Task to delete: ");
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            s_bl?.Task.Delete(idT);
        }
    }
}
