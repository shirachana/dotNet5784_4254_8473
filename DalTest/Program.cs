using Dal;
using System.Collections.Generic;
using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;
using System.ComponentModel;

namespace DalTest
{
    internal class Program
    {
        //private static readonly IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        //private static readonly IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        //private static readonly ITask? s_dalTask = new TaskImplementation(); //stage 1 
        private static readonly Random s_rand=new();
        static readonly IDal s_dal = new Dal.DalList();
        //internal static class PrintExtension
        //{
        //    internal static void PrintList<Entity>(this IEnumerable<Entity> entities) =>
        //        string.Join(Environment.NewLine, entities);
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            try
            {
                Initialization.Do(s_dal);
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
                        DisplayEntityMenuD("Dependency");
                        break;
                    case 2:
                        DisplayEntityMenuE("Engineer");
                        break;
                    case 3:
                        DisplayEntityMenuT("Task");
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            while (true);
            
        }

        static void DisplayEntityMenuD( string entityName)
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
                        CreatDependency();
                        break;
                    case 2:
                        ReadDependencyById();
                        break;
                    case 3:
                        ReadAllDependencies();
                        break;
                    case 4:
                        UpdateDependency();
                        break;
                    case 5:
                        DeleteDependency();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
        }
        static void DisplayEntityMenuE( string entityName)
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
        static void CreatDependency()
        {
            //try
            //{
                Console.WriteLine(@"Enter id dependent Task: ");
                string? d = Console.ReadLine();
                int idDTsk= int.Parse(d!);
                Console.WriteLine(@"Enter id depends on Task: ");
                string? n = Console.ReadLine();
                int idDOnTsk = int.Parse(n!);
                Dependency dependency = new Dependency(0, idDTsk,idDOnTsk);
                int dpncy = s_dal!.Dependency.Create(dependency);
                Console.WriteLine(dpncy);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }

        static void ReadDependencyById()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Dependency: ");
                string? d = Console.ReadLine();
                int idDep = int.Parse(d!);
                Dependency? dpcy = s_dal!.Dependency.Read(idDep);
                Console.WriteLine(dpcy);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void ReadAllDependencies()
        {
            //try
            //{
                IEnumerable<Dependency?> dependencies;
                dependencies = s_dal!.Dependency.ReadAll();
                foreach (Dependency? dependency in dependencies) { Console.WriteLine(dependency); }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }

        static void UpdateDependency()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Dependency: ");
                string? d = Console.ReadLine();
                int idDep = int.Parse(d!);
                Dependency? dpcy = new Dependency();
                dpcy = s_dal!.Dependency.Read(idDep);
                if (dpcy != null)
                {
                    Console.WriteLine(dpcy);
                    Console.WriteLine(@"Enter id dependent Task: ");
                    string? dDep = Console.ReadLine();
                    int idDepenT = int.Parse(dDep!);
                    Console.WriteLine(@"Enter id depends on Task: ");
                    string? depOn = Console.ReadLine();
                    int idDepenedOnTsk = int.Parse(depOn!);
                    Dependency dependencyNew = new Dependency(0, idDepenT, idDepenedOnTsk);
                    s_dal!.Dependency.Update(dependencyNew);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void DeleteDependency()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Dependency to delete: ");
                string? d = Console.ReadLine();
                int idDepn = int.Parse(d!);
                s_dal?.Dependency.Delete(idDepn);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void CreatEngineer()
        {
            //try
            //{
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
                DO.EngineerExperience engineerLevel=(DO.EngineerExperience)int.Parse(l!);
                Engineer engineer = new Engineer(idD,costForHour,engineerEmail,engineerName,engineerLevel);
                int crt = s_dal!.Engineer.Create(engineer);
                 Console.WriteLine(crt);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void ReadEngineerById()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Engineer: ");
                string? d = Console.ReadLine();
                int idD = int.Parse(d!);
                Engineer? eng = new Engineer();
                eng=s_dal!.Engineer.Read(idD);
                Console.WriteLine(eng);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void ReadAllEngineers()
        {
            //try
            //{
                IEnumerable<Engineer?> engineers;
                engineers = s_dal!.Engineer.ReadAll();
                foreach(Engineer? engineer in engineers) { Console.WriteLine(engineer); }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }

        static void UpdateEngineer()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Engineer: ");
                string? d = Console.ReadLine();
                int idD = int.Parse(d!);
                Engineer? eng = new Engineer();
                eng = s_dal!.Engineer.Read(idD);
                if(eng != null ) 
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
                    DO.EngineerExperience engineerLevel = (DO.EngineerExperience)int.Parse(l!);
                    Engineer engineer = new Engineer(idD, costForHour, engineerEmail, engineerName, engineerLevel);
                    s_dal!.Engineer.Update(engineer);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void DeleteEngineer()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Engineer to delete: ");
                string? d = Console.ReadLine();
                int idD = int.Parse(d!);
                s_dal!.Engineer.Delete(idD);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void CreatTask()
        {
            //try
            //{
                Console.WriteLine(@"Enter the engineer ID of the task: ");
                string? d = Console.ReadLine();
                int idT = int.Parse(d!);
                Console.WriteLine(@"Enter the alias of the task");
                string? aliasTask= Console.ReadLine();
                Console.WriteLine(@"Enter the discription of the task if there: ");
                string? discTask = Console.ReadLine();
                Console.WriteLine(@"Enter the level of the task: ");
                string? l = Console.ReadLine();
                DO.EngineerExperience taskLevel = (DO.EngineerExperience)int.Parse(l!);
                Console.WriteLine(@"Enter the deliverables of the task: ");
                string? deliTask = Console.ReadLine();
                Console.WriteLine(@"Enter the remark of the task: ");
                string? taskRemark= Console.ReadLine();
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
                DO.Task tsk = new DO.Task(0, idT, aliasTask, discTask, null, null, false,taskLevel,null,null,null,null,deliTask,taskRemark );
                int? tk = s_dal!.Task.Create(tsk);
                Console.WriteLine(tk);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void ReadTaskById()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of task: ");
                string? d = Console.ReadLine();
                int idT = int.Parse(d!);
                DO.Task? tsk = new DO.Task();
                tsk= s_dal!.Task.Read(idT);
                Console.WriteLine(tsk);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void ReadAllTasks()
        {
            //try
            //{
                IEnumerable<DO.Task?> tasks;
                tasks = s_dal!.Task.ReadAll();
                foreach (DO.Task? task in tasks) { Console.WriteLine(task); }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }

        static void UpdateTask()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of task: ");
                string? d = Console.ReadLine();
                int idT = int.Parse(d!);
                DO.Task? tsks = new DO.Task();
                tsks = s_dal!.Task.Read(idT);
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
                    DO.Task tskUp = new DO.Task(0, idTsk, aliasTask, discTask, null, null, false, taskLevel, null, null, null, null, deliTask, taskRemark);
                    s_dal!.Task.Update(tskUp);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }
        static void DeleteTask()
        {
            //try
            //{
                Console.WriteLine(@"Enter id of Task to delete: ");
                string? d = Console.ReadLine();
                int idT = int.Parse(d!);
                s_dal?.Task.Delete(idT);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}
        }




    }
}
      


