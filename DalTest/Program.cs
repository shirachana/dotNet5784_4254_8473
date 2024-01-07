using Dal;
using System.Collections.Generic;
using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;

namespace DalTest
{
    internal class Program
    {
        private static readonly IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        private static readonly IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static readonly ITask? s_dalTask = new TaskImplementation(); //stage 1 
       
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            try
            {
            Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
                    DisplayMainMenu();
            }
           catch (Exception ex)
            {
               Console.WriteLine($"An exception occurred: {ex}");
            }
        }
        static void DisplayMainMenu()
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
                    DisplayEntityMenuD(s_dalDependency! , "Dependency");
                    break;
                case 2:
                    DisplayEntityMenuE(s_dalEngineer! , "Engineer");
                    break;
                case 3:
                    DisplayEntityMenuT(s_dalTask! , "Task");
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        static void DisplayEntityMenuD(IDependency entityAccessD, string entityName)
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
                        CreatDependency(entityAccessD);
                        break;
                    case 2:
                        ReadDependencyById(entityAccessD);
                        break;
                    case 3:
                        ReadAllDependencies(entityAccessD);
                        break;
                    case 4:
                        UpdateDependency(entityAccessD);
                        break;
                    case 5:
                        DeleteDependency(entityAccessD);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
        }
        static void DisplayEntityMenuE(IEngineer entityAccessE, string entityName)
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
                        CreatEngineer(entityAccessE);
                        break;
                    case 2:
                        ReadEngineerById(entityAccessE);
                        break;
                    case 3:
                        ReadAllEngineers(entityAccessE);
                        break;
                    case 4:
                        UpdateEngineer(entityAccessE);
                        break;
                    case 5:
                        DeleteEngineer(entityAccessE);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void DisplayEntityMenuT(ITask entityAccessT, string entityName)
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
                        CreatTask(entityAccessT);
                        break;
                    case 2:
                        ReadTaskById(entityAccessT);
                        break;
                    case 3:
                        ReadAllTasks(entityAccessT);
                        break;
                    case 4:
                        UpdateTask(entityAccessT);
                        break;
                    case 5:
                        DeleteTask(entityAccessT);
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
        static void CreatDependency(IDependency dep)
        {
            try
            {
                Console.WriteLine("enter id,  id dependent Task and  id depends on task");
                string? d = Console.ReadLine();
                int idD= int.Parse(d!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ReadDependencyById(IDependency dep)
        {
            try
            {
                // Implement logic for reading an entity by its ID and displaying it
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void ReadAllDependencies(IDependency dep)
        {
            try
            {
                // Implement logic for reading all entities and displaying them
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void UpdateDependency(IDependency dep)
        {
            try
            {
                // Implement logic for updating an existing entity
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void DeleteDependency(IDependency dep)
        {
            try
            {
                // Implement logic for deleting an existing entity
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void CreatEngineer(IEngineer eng)
        {
            try
            {
                Console.WriteLine("enter id, cost to hour, Email name and the level experience of engineer");
                string? d = Console.ReadLine();
                string? c = Console.ReadLine();
                string? l = Console.ReadLine();
                int idD = int.Parse(d!);
                double costForHour = double.Parse(c!);
                string? engineerEmail = Console.ReadLine();
                string? engineerName = Console.ReadLine();
                DO.EngineerExperience engineerLevel=(DO.EngineerExperience)int.Parse(l!);
                Engineer engineer = new Engineer(idD,costForHour,engineerEmail,engineerName,engineerLevel);
                int crt = s_dalEngineer!.Create(engineer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void ReadEngineerById(IEngineer eng)
        {
            try
            {
                // Implement logic for reading an entity by its ID and displaying it
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void ReadAllEngineers(IEngineer eng)
        {
            try
            {
                // Implement logic for reading all entities and displaying them
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void UpdateEngineer(IEngineer eng)
        {
            try
            {
                // Implement logic for updating an existing entity
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void DeleteEngineer(IEngineer eng)
        {
            try
            {
                // Implement logic for deleting an existing entity
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void CreatTask(ITask tsk)
        {
            try
            {
                Console.WriteLine("enter id,  id dependent Task and  id depends on task");
                string? d = Console.ReadLine();
                int idD = int.Parse(d!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void ReadTaskById(ITask tsk)
        {
            try
            {
                // Implement logic for reading an entity by its ID and displaying it
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void ReadAllTasks(ITask tsk)
        {
            try
            {
                // Implement logic for reading all entities and displaying them
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void UpdateTask(ITask tsk)
        {
            try
            {
                // Implement logic for updating an existing entity
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void DeleteTask(ITask tsk)
        {
            try
            {
                // Implement logic for deleting an existing entity
                // ...
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }




    }
}
      


