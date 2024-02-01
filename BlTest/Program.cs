using BlApi;
using BO;
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
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();

            do
            {
                Console.WriteLine("Main Menu - Select an entity you want to check:");
                Console.WriteLine("1. Engineer");
                Console.WriteLine("2. Task");
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

        static void CreatEngineer()
        {
            BO.Engineer? eng = new BO.Engineer();
            Console.WriteLine(@"Enter the engineer ID: ");
            eng.Id = int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the engineer cost for hour: ");
            eng.Cost = double.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the engineer Email: ");
            eng.Email = Console.ReadLine();
            Console.WriteLine(@"Enter the engineer Name: ");
            eng.Name = Console.ReadLine();
            Console.WriteLine(@"Enter the Engineer ID for the task: ");
            int engineerId = int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the Engineer Name for the task: ");
            string engineerName = (Console.ReadLine()!);
            BO.EngineerInTask engineer = new BO.EngineerInTask()
            {
                Id = engineerId,
                Name = engineerName,

            };
            eng.Task = engineer;
            Console.WriteLine(@"Enter the level of the task: ");
            eng.Level = (BO.EngineerExperience)int.Parse(Console.ReadLine()!);
            s_bl.Engineer.Create(eng);
            

            //BO.Engineer engineer = new BO.Engineer(idD, costForHour, engineerEmail, engineerName, engineerLevel);
            //s_bl!.Engineer.Create(eng);
            //Console.WriteLine(crt);

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
            Random s_rand = new Random();

            BO.Engineer eng = new BO.Engineer();
            Console.WriteLine(@"Enter id of Engineer: ");
            string? d = Console.ReadLine();
            int idD = int.Parse(d!);
            BO.Engineer? engi = new BO.Engineer();
            engi = s_bl!.Engineer.Read(idD)!;
            if (engi != null)
            {
                Console.WriteLine(engi);
                Console.WriteLine(@"Enter the engineer ID: ");
                eng.Id = int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the engineer cost for hour: ");
                eng.Cost = double.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the engineer Email: ");
                eng.Email = Console.ReadLine();
                Console.WriteLine(@"Enter the engineer Name: ");
                eng.Name = Console.ReadLine();
                Console.WriteLine(@"Enter the Engineer ID for the task: ");
                int engineerId = int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the Engineer Name for the task: ");
                string engineerName = (Console.ReadLine()!);
                BO.EngineerInTask engineer = new BO.EngineerInTask()
                {
                    Id = engineerId,
                    Name = engineerName,

                };
                eng.Task = engineer;
                Console.WriteLine(@"Enter the level of the task: ");
                eng.Level = (BO.EngineerExperience)int.Parse(Console.ReadLine()!);

            }
            s_bl.Engineer.Update(eng);

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
            Console.WriteLine(@"Enter the id of the task if there: ");
            tsk.Id = int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the discription of the task if there: ");
            tsk.Description = Console.ReadLine();
            Console.WriteLine(@"Enter the alias of the task");
            tsk.Alias  = Console.ReadLine();
            Console.WriteLine(@"Enter the Creat at Date of the task: ");
            string dateString = Console.ReadLine()!;
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                tsk.CreatedAtDate = date;
            }
            else
            {
                throw new Exception("Invalid date format. Please enter again");
            }
            Console.WriteLine(@"Enter the Status the task: ");
            tsk.Status = (BO.Status)int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter number of dependencies: ");
            int numberOfDependencies = int.Parse(Console.ReadLine()!);
            
            tsk.Dependencies = new List<BO.TaskInList>();
            for (int i = 0;i<numberOfDependencies; i++)
            {
                Console.WriteLine(@"Enter the Dependency id: ");
                int idDep = int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the Dependency description: ");
                string descriptionDep = Console.ReadLine()!;
                Console.WriteLine(@"Enter the Dependency alias: ");
                string aliasDep = (Console.ReadLine()!);
                Console.WriteLine(@"Enter the Dependency status: ");
                BO.Status statusDep = (BO.Status)int.Parse(Console.ReadLine()!);
                BO.TaskInList dependency = new BO.TaskInList
                {
                    Id = idDep,
                    Description=descriptionDep,
                    Alias=aliasDep,
                    Status=statusDep,
                };

                tsk.Dependencies?.Add(dependency);
            }
            Console.WriteLine(@"Enter the Required Effort Time: ");
            int requiredEffortDays = int.Parse(Console.ReadLine()!);
            tsk.RequiredEffortTime = TimeSpan.FromDays(requiredEffortDays);

            //Console.WriteLine(@"Enter the Start Date of the task: ");
            //if (DateTime.TryParse(Console.ReadLine()!, out DateTime startDate))
            //{
            //    tsk.StartDate = startDate;
            //}
            //else
            //{
            //    throw new Exception("Invalid date format. Please enter again");
            //}

            Console.WriteLine(@"Enter the Scheduled Date of the task: ");
            if (DateTime.TryParse(Console.ReadLine()!, out DateTime scheduledDate))
            {
                tsk.ScheduledDate = scheduledDate;
            }
            else
            {
                throw new Exception("Invalid date format. Please enter again");
            }

            Console.WriteLine(@"Enter the Forecast Date of the task: ");
            if (DateTime.TryParse(Console.ReadLine()!, out DateTime forecastDate))
            {
                tsk.ForecastDate = forecastDate;
            }
            else
            {
                throw new Exception("Invalid date format. Please enter again");
            }

            Console.WriteLine(@"Enter the Deadline Date of the task: ");
            if (DateTime.TryParse(Console.ReadLine()!, out DateTime deadlineDate))
            {
                tsk.DeadlineDate = deadlineDate;
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
            Console.WriteLine(@"Enter the deliverables of the task: ");
            tsk.Deliverables = Console.ReadLine();
            Console.WriteLine(@"Enter the Engineer ID for the task: ");
            int engineerId = int.Parse(Console.ReadLine()!);
            Console.WriteLine(@"Enter the Engineer Name for the task: ");
            string engineerName = (Console.ReadLine()!);
            BO.EngineerInTask engineer= new BO.EngineerInTask()
            {
                Id= engineerId,
                Name= engineerName,
                
            };
            tsk.Engineer=engineer;

            Console.WriteLine(@"Enter the Remarks of the task: ");
            tsk.Remarks = Console.ReadLine();
            Console.WriteLine(@"Enter the level of the task: ");
            tsk.Copmlexity = (BO.EngineerExperience)int.Parse(Console.ReadLine()!);

            s_bl!.Task.Create(tsk);
        }
        //private DateTime GetEarliestStartDate(List<BO.Task>previousTasks)
        //{
        //    DateTime earliestStartDate = DateTime.MaxValue;

        //    foreach (var task in previousTasks)
        //    {
        //        if (!task.ScheduledDate.HasValue)
        //        {
        //            throw new Exception($"Task {task.Id} does not have a scheduled start date.");
        //        }

        //        earliestStartDate = DateTime.Compare(task.ScheduledDate.Value, earliestStartDate) < 0
        //            ? task.ScheduledDate.Value
        //            : earliestStartDate;
        //    }

        //    return earliestStartDate;
        //}


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
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            BO.Task? tsks = new BO.Task();
            tsks = s_bl!.Task.Read(idT);
            if (tsks != null)
            {

                Console.WriteLine(@"Enter the engineer ID of the task: ");
                tsk.Id = int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the discription of the task if there: ");
                tsk.Description = Console.ReadLine();
                Console.WriteLine(@"Enter the alias of the task");
                tsk.Alias = Console.ReadLine();
                Console.WriteLine(@"Enter the Creat at Date of the task: ");
                string dateString = Console.ReadLine()!;
                if (DateTime.TryParse(dateString, out DateTime date))
                {
                    tsk.CreatedAtDate = date;
                }
                else
                {
                    throw new Exception("Invalid date format. Please enter again");
                }
                Console.WriteLine(@"Enter the Status the task: ");
                tsk.Status = (BO.Status)int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter number of dependencies: ");
                int numberOfDependencies = int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the Dependency description: ");
                string descriptionDep = Console.ReadLine()!;
                Console.WriteLine(@"Enter the Dependency alias: ");
                string aliasDep = (Console.ReadLine()!);
                Console.WriteLine(@"Enter the Dependency status: ");
                BO.Status statusDep = (BO.Status)int.Parse(Console.ReadLine()!);
                for (int i = 0; i < numberOfDependencies; i++)
                {
                    Console.WriteLine(@"Enter the Dependency id: ");
                    int idDep = int.Parse(Console.ReadLine()!);
                    BO.TaskInList dependency = new BO.TaskInList
                    {
                        Id = idDep,
                        Description = descriptionDep,
                        Alias = aliasDep,
                        Status = statusDep,
                    };

                    tsk.Dependencies?.Add(dependency);
                }
                Console.WriteLine(@"Enter the Required Effort Time: ");
                int requiredEffortDays = int.Parse(Console.ReadLine()!);
                tsk.RequiredEffortTime = TimeSpan.FromDays(requiredEffortDays);

                Console.WriteLine(@"Enter the Start Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime startDate))
                {
                    tsk.StartDate = startDate;
                }
                else
                {
                    throw new Exception("Invalid date format. Please enter again");
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

                Console.WriteLine(@"Enter the Forecast Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime forecastDate))
                {
                    tsk.ForecastDate = forecastDate;
                }
                else
                {
                    throw new Exception("Invalid date format. Please enter again");
                }

                Console.WriteLine(@"Enter the Deadline Date of the task: ");
                if (DateTime.TryParse(Console.ReadLine()!, out DateTime deadlineDate))
                {
                    tsk.DeadlineDate = deadlineDate;
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
                Console.WriteLine(@"Enter the deliverables of the task: ");
                tsk.Deliverables = Console.ReadLine();
                Console.WriteLine(@"Enter the Engineer ID for the task: ");
                int engineerId = int.Parse(Console.ReadLine()!);
                Console.WriteLine(@"Enter the Engineer Name for the task: ");
                string engineerName = (Console.ReadLine()!);
                BO.EngineerInTask engineer = new BO.EngineerInTask()
                {
                    Id = engineerId,
                    Name = engineerName,

                };
                tsk.Engineer = engineer;

                Console.WriteLine(@"Enter the Remarks of the task: ");
                tsk.Remarks = Console.ReadLine();
                Console.WriteLine(@"Enter the level of the task: ");
                tsk.Copmlexity = (BO.EngineerExperience)int.Parse(Console.ReadLine()!);
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
        private static void UpdateTheStartDate()
        {
            Console.WriteLine(@"Enter id of Task to update the tasks start date: ");
            string? d = Console.ReadLine();
            int idT = int.Parse(d!);
            Console.WriteLine(@"Enter the Start Date of the task: ");
            if (DateTime.TryParse(Console.ReadLine()!, out DateTime startDate))
            {
                DateTime sd = startDate;
                s_bl?.Task.UpdateStartDate(idT, sd);
            }
            else
            {
                throw new Exception("Invalid date format. Please enter again");
            }
        }
    }
}
