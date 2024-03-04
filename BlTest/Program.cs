using BlApi;
using BO;
using DalApi;
using DalTest;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
            bool validInput = false;

            do
            {
                Console.Write("Would you like to create Initial data? (Y/N): ");
                string? ans = Console.ReadLine();

                if (ans == "Y")
                {
                    s_bl!.Task.Clear();
                    s_bl!.Engineer.Clear();
                    s_bl.SetProjectEndDate(null);
                    s_bl.SetProjectStartDate(null);
                    Initialization.Do();
                    validInput = true;
                }
                else if (ans == "N")
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Wrong input. Please enter 'Y' or 'N'.");
                }

            } while (!validInput);


            do
            {
                Console.WriteLine("Main Menu - Select an entity you want to check:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Engineer");
                Console.WriteLine("2. Task");
                Console.WriteLine("3. Create The Start Date Of The Project");
                Console.WriteLine("4. Create The end Date Of The Project");
                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        DisplayEntityMenuE("Engineer");
                        break;
                    case 2:
                        DisplayEntityMenuT("Task");
                        break;
                    case 3:
                        CreatStartDate();
                        break;
                    case 4:
                        CreatEndDate();
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
                Console.WriteLine("6.Update Engineer to task");
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
                    case 6:
                        UpdateTheEngineerTask();
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
                Console.WriteLine("6.Update the Start Date");
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
                    case 6:
                        UpdateTheStartDate();
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
        static void CreatStartDate()
        {
            Console.WriteLine("enter StartDate\n");
            string Date = Console.ReadLine()!;
            DateTime.TryParse(Date, out DateTime date);
            try {
                s_bl.SetProjectStartDate(date);
                if (date! < DateTime.Today)
                    throw new Exception("the date pass already");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        private static void CreatEndDate()
        {
            Console.WriteLine("enter EndDate\n");
            string Date = Console.ReadLine()!;
            DateTime.TryParse(Date, out DateTime date);
            try
            {
                s_bl.SetProjectEndDate(date);
                if (date! <s_bl.GetProjectStartDate() )
                    throw new Exception("the date its earlier than the start date");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        static void CreatEngineer()
        {
            BO.Engineer? eng = new BO.Engineer();
            Console.WriteLine(@"Enter the engineer ID: ");
            if (int.TryParse(Console.ReadLine(), out int engineerId) && engineerId > 0)
            {
                eng.Id = engineerId;
            }
            else
            {
                throw new Exception("Invalid engineer ID. Please enter a positive integer.");
            }
            Console.WriteLine(@"Enter the engineer cost for hour: ");
            if (double.TryParse(Console.ReadLine(), out double engineerCost) && engineerCost >= 0)
            {
                eng.Cost = engineerCost;
            }
            else
            {
                throw new Exception("Invalid engineer cost. Please enter a non-negative number.");
            }
            Console.WriteLine(@"Enter the engineer Email: ");
            eng.Email = Console.ReadLine();
            Console.WriteLine(@"Enter the engineer Name: ");
            eng.Name = Console.ReadLine();
            Console.WriteLine(@"Enter the level of the engineer: ");
            if (int.TryParse(Console.ReadLine(), out int engineerLevel) && Enum.IsDefined(typeof(BO.EngineerExperience), engineerLevel))
            {
                eng.Level = (BO.EngineerExperience)engineerLevel;
            }
            else
            {
                throw new Exception("Invalid engineer level. Please enter a valid integer corresponding to the EngineerExperience enum.");
            }
            s_bl.Engineer.Create(eng);
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
        static void UpdateTheEngineerTask()
        {
            Random s_rand = new Random();
            Console.WriteLine(@"Enter the Engineer ID for the task: ");
            int engineerId = int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the Task Id: ");
            int TaskId = int.Parse(Console.ReadLine()!);
            s_bl!.Engineer!.UpdateEngineerToTask(engineerId, TaskId);
        }

        static void UpdateEngineer()
        {
            Random s_rand = new Random();
            BO.Engineer eng = new BO.Engineer();
            Console.WriteLine(@"Enter id of Engineer: ");
            string? d = Console.ReadLine();
            int idD = int.Parse(d!);
            eng = s_bl!.Engineer.Read(idD)!;
                if (eng != null)
                {

                    Console.WriteLine(@"Enter the engineer cost for hour: ");
                    eng.Cost = double.Parse(Console.ReadLine()!) >= 0 ? double.Parse(Console.ReadLine()!) : 
                        throw new Exception("Invalid input. Please enter a non-negative number.");

                    Console.WriteLine(@"Enter the engineer Email: ");
                    eng.Email = !string.IsNullOrWhiteSpace(Console.ReadLine()) ? Console.ReadLine() : 
                        throw new Exception("Invalid input. Please enter a non-empty string.");

                    Console.WriteLine(@"Enter the engineer Name: ");
                    eng.Name = !string.IsNullOrWhiteSpace(Console.ReadLine()) ? Console.ReadLine() : 
                        throw new Exception("Invalid input. Please enter a non-empty string.");

                    Console.WriteLine(@"Enter the level of the engineer: ");
                    eng.Level = Enum.TryParse(Console.ReadLine(), out BO.EngineerExperience level) && Enum.IsDefined(typeof(BO.EngineerExperience), level) ? level : 
                        throw new Exception("Invalid input. Please enter a valid EngineerExperience value.");
                    s_bl.Engineer.Update(eng);
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

            
                Console.WriteLine(@"Enter the description of the task if there: ");
                tsk.Description = Console.ReadLine() ?? throw new Exception("Task description cannot be null.");

                Console.WriteLine(@"Enter the alias of the task");
                tsk.Alias = Console.ReadLine() ?? throw new Exception("Task alias cannot be null.");

                Console.WriteLine(@"Enter the Remarks of the task: ");
                tsk.Remarks = Console.ReadLine() ?? throw new Exception("Task remarks cannot be null.");

                Console.WriteLine(@"Enter the level of the task: ");
                if (int.TryParse(Console.ReadLine(), out int complexityValue) && Enum.IsDefined(typeof(BO.EngineerExperience), complexityValue))
                {
                    tsk.Copmlexity = (BO.EngineerExperience)complexityValue;
                }
                else
                {
                    throw new Exception("Invalid complexity level.");
                }
                Console.WriteLine(@"Enter the Create at Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime date))
                {
                    tsk.CreatedAtDate = date;
                }
                else
                {
                    throw new Exception("Invalid date format.");
                }
                Console.WriteLine(@"Enter the Required Effort Time: ");
                int requiredEffortDays;
                if (int.TryParse(Console.ReadLine(), out requiredEffortDays))
                {
                    tsk.RequiredEffortTime = TimeSpan.FromDays(requiredEffortDays);
                }
                else
                {
                    throw new Exception("Invalid input for required effort time.");
                }

                Console.WriteLine(@"Enter the deliverables of the task: ");
                tsk.Deliverables = Console.ReadLine();

                Console.WriteLine(@"Enter the number of dependencies: ");
                int numberOfDependencies;
                if (int.TryParse(Console.ReadLine(), out numberOfDependencies))
                {
                    tsk.Dependencies = new List<BO.TaskInList>();
                    for (int i = 0; i < numberOfDependencies; i++)
                    {
                        Console.WriteLine(@"Enter the Dependency id: ");
                        if (int.TryParse(Console.ReadLine(), out int idDep))
                        {
                            BO.TaskInList dependency = new BO.TaskInList
                            {
                                Id = idDep,
                            };
                            tsk.Dependencies.Add(dependency);
                        }
                        else
                        {
                            throw new Exception("Invalid input for dependency id.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid input for number of dependencies.");
                }
            s_bl!.Task.Create(tsk);
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

            IEnumerable<BO.TaskInList?> tasks;
            tasks = (IEnumerable<BO.TaskInList?>)s_bl!.Task.ReadAll();
            foreach (BO.TaskInList? task in tasks) { Console.WriteLine(task); }

        }

        static void UpdateTask()
        {
            Random s_rand = new Random();
            BO.Task tsk = new BO.Task();

            Console.WriteLine(@"Enter id of task: ");
            tsk.Id = int.Parse(Console.ReadLine()!);
            BO.Task? tsks = new BO.Task();
            tsks = s_bl!.Task.Read(tsk.Id);
            if (tsks != null)
            {
                Console.WriteLine(@"Enter the discription of the task if there: ");
                tsk.Description = Console.ReadLine();
                Console.WriteLine(@"Enter the alias of the task");
                tsk.Alias = Console.ReadLine();
                Console.WriteLine(@"Enter the Remarks of the task: ");
                tsk.Remarks = Console.ReadLine();
                Console.WriteLine(@"Enter the level of the task: ");
                if (int.TryParse(Console.ReadLine(), out int complexityValue) && Enum.IsDefined(typeof(BO.EngineerExperience), complexityValue))
                {
                    tsk.Copmlexity = (BO.EngineerExperience)complexityValue;
                }
                else
                {
                    throw new Exception("Invalid complexity level.");
                }
                Console.WriteLine(@"Enter the deliverables of the task: ");
                tsk.Deliverables = Console.ReadLine();
                Console.WriteLine(@"Enter the Creat at Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime date))
                {
                    tsk.CreatedAtDate = date;
                }
                else
                {
                    throw new Exception("Invalid date format. Please enter again");
                }
                Console.WriteLine(@"Enter the Required Effort Time: ");
                int requiredEffortDays;
                if (int.TryParse(Console.ReadLine(), out requiredEffortDays))
                {
                    tsk.RequiredEffortTime = TimeSpan.FromDays(requiredEffortDays);
                }
                else
                {
                    throw new Exception("Invalid input for required effort time.");
                }
                Console.WriteLine(@"Enter the Scheduled Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime scheduledDate))
                {
                    tsk.ScheduledDate = scheduledDate;
                }
                else
                {
                    throw new Exception("Invalid date format. Please enter again");
                }
                Console.WriteLine(@"Enter the Complete Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime completeDate))
                {
                    tsk.CompleteDate = completeDate;
                }
                else
                {
                    throw new Exception("Invalid date format. Please enter again");
                }
                Console.WriteLine(@"Enter number of dependencies: ");
                int numberOfDependencies = int.Parse(Console.ReadLine()!);
                tsk.Dependencies = new List<BO.TaskInList>();
                for (int i = 0; i < numberOfDependencies; i++)
                {
                    Console.WriteLine(@"Enter the Dependency id: ");
                    int idDep = int.Parse(Console.ReadLine()!);
                    BO.TaskInList dependency = new BO.TaskInList
                    {
                        Id = idDep,
                    };

                    tsk.Dependencies?.Add(dependency);
                }
                Console.WriteLine(@"Enter the Engineer ID for the task: ");
                int engineerId = int.Parse(Console.ReadLine()!);
                BO.EngineerInTask engineer = new BO.EngineerInTask()
                {
                    Id = engineerId,
                };
                tsk.Engineer = engineer;
                s_bl!.Task.Update(tsk);
            }

        }
        static void DeleteTask()
        {

            Console.WriteLine(@"Enter id of Task to delete: ");
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            s_bl?.Task.Delete(idT);
        }
        static void UpdateTheStartDate()
        {
            Console.WriteLine(@"Enter id of Task to update the tasks start date: ");
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            Console.WriteLine(@"Enter the Start Date of the task: ");
            if (DateTime.TryParse(Console.ReadLine()!, out DateTime startDate))
            {
                s_bl?.Task.UpdateStartDate(idT, startDate);
            }
            else
            {
                Console.WriteLine(("Invalid date format. Please enter again"));
            }
        }
    }
}

