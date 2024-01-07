using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
           // try
            //{
                // Initialize the database by calling the Do method of the Initialization class
                Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
                while (true)
                {
                    DisplayMainMenu();
                }

           // }
           // catch (Exception ex)
           // {
                // Print the caught exception to the screen
           //     Console.WriteLine($"An exception occurred: {ex}");
           // }
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
                    DisplayEntityMenuD(s_dalDependency?? throw new Exception("this Dependency does not exist"), "Dependency");
                    break;
                case 2:
                    DisplayEntityMenuE(s_dalEngineer?? throw new Exception("this Engineer does not exist"), "Engineer");
                    break;
                case 3:
                    DisplayEntityMenuT(s_dalTask?? throw new Exception("this Task does not exist"), "Task");
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
                        try
                        {
                            int idDep;
                            Dependency dep = new Dependency();
                            idDep = entityAccessD.Create(dep);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 2:
                        try
                        {
                            Dependency dep = new Dependency();
                            entityAccessD.Read(dep.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 3:
                        try
                        {
                            entityAccessD.ReadAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 4:
                        try
                        {
                            Dependency dep = new Dependency();
                            entityAccessD.Update(dep);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 5:
                        try
                        {
                            Dependency dep = new Dependency();
                            entityAccessD.Delete(dep.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
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
                        try
                        {
                            int idEng;
                            Engineer eng = new Engineer();
                            idEng = entityAccessE.Create(eng);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 2:
                        try
                        {
                            Engineer eng = new Engineer();
                            entityAccessE.Read(eng.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 3:
                        try
                        {
                            entityAccessE.ReadAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 4:
                        try
                        {
                            Engineer eng = new Engineer();
                            entityAccessE.Update(eng);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 5:
                        try
                        {
                            Engineer eng = new Engineer();
                            entityAccessE.Delete(eng.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
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
                        try
                        {
                            int idT;
                            DO.Task tsk = new DO.Task();
                            idT = entityAccessT.Create(tsk);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 2:
                        try
                        {
                            DO.Task tsk = new DO.Task();
                            entityAccessT.Read(tsk.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 3:
                        try
                        {
                            entityAccessT.ReadAll();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 4:
                        try
                        {
                            DO.Task tsk = new DO.Task();
                            entityAccessT.Update(tsk);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 5:
                        try
                        {
                            DO.Task tsk = new DO.Task();
                            entityAccessT.Delete(tsk.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
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
    }
}
      


