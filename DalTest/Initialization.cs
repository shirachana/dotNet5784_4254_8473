namespace DalTest;
using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class Initialization
{
    //private static IDependency? s_dalDependency; //stage 1
    //private static IEngineer? s_dalEngineer; //stage 1
    //private static ITask? s_dalTask; //stage 1
    private static IDal? s_dal;
    private static readonly Random s_rand = new();
    private static void CreateDependencies()
    {
        for (int i = 0; i < 40; i++)
        {
            Dependency dep = new Dependency();
            switch (i)
            {
                case int c when i < 10:
                    dep = dep with
                    {
                        DependentTask = s_rand.Next(6, 11),
                        DependsOnTask = s_rand.Next(1, 6)
                    };
                    break;
                case int c when i < 20:
                    dep = dep with
                    {
                        DependentTask = s_rand.Next(11, 16),
                        DependsOnTask = s_rand.Next(6, 11),
                    };
                    break;
                case int c when i < 40:
                    dep = dep with
                    {
                        DependentTask = s_rand.Next(16, 21),
                        DependsOnTask = s_rand.Next(11, 16),
                    };
                    break;
            }
            s_dal!.Dependency.Create(dep);
        }
    }
    private static void CreatTask()
    {
        string[] _alias = { "recipe-design",
            "ingredient-sourcing",
            "cost-analysis",
            "quality-control",
            "production-scheduling",
            "batch-tracking",
            "equipment-maintenance",
            "regulatory-compliance",
            "new-ingredient-evaluation",
            "allergen-management",
            "packaging-design",
            "supplier-performance",
            "shelf-life-testing",
            "sustainability-assessment",
            "recipe-scaling",
            "traceability-system",
            "food-safety-training",
            "inventory-management",
            "product-launch-planning",
            "market-trend-analysis" };
        string[] _dedcription = { "Design a new recipe for a food product.",
        "Source and procure high-quality ingredients for recipes",
        "Analyze the cost of ingredients and production for a recipe.",
        "Implement and monitor quality control measures in food production.",
        "Schedule and manage production processes for multiple recipes.",
        "Track batches of food products throughout the production process.",
        "Schedule and perform maintenance on production equipment.",
        "Ensure compliance with food safety and labeling regulations.",
        "Evaluate and test new ingredients for potential use in recipes.",
        "Manage and control allergen information for food products.",
        "Design and choose packaging for food products.",
        "Evaluate and monitor the performance of ingredient suppliers.",
        "Conduct tests to determine the shelf life of food products.",
        "Assess and improve the sustainability of food production processes.",
        "Scale up or down recipes to meet production requirements.",
        "Implement a system for tracing ingredients from source to final product.",
        "Provide training on food safety practices for production staff.",
        "Manage and optimize ingredient and product inventory levels",
        "Plan and coordinate the launch of new food products.",
        "Analyze market trends to inform new product development."};
        string[] _remarks = { "Include details about ingredients, quantities, and preparation steps.",
        "Keep track of suppliers and ingredient quality.",
        "Consider factors like labor, packaging, and overhead costs",
        "Set standards and ensure compliance with quality regulations.",
        "Optimize production efficiency and minimize downtime.",
        "Monitor quality, traceability, and compliance",
        "Ensure equipment reliability and prevent breakdowns",
        "Stay informed about regulatory changes and updates.",
        "Consider factors like taste, texture, and shelf life.",
        "Ensure accurate allergen labeling and prevent cross-contamination.",
        "Consider branding, shelf life, and environmental impact.",
        "Consider factors like reliability, quality, and pricing.",
        "Consider storage conditions, packaging, and quality over time.",
        "Consider environmental impact, waste reduction, and energy efficiency",
        "Adjust ingredient quantities while maintaining quality.",
        "Enhance transparency and respond to recalls effectively.",
        "Keep staff informed about hygiene, sanitation, and safety protocols",
        "Minimize waste, prevent shortages, and track expiration dates.",
        "Consider marketing, distribution, and customer feedback.",
        "Stay ahead of consumer preferences and industry developments." };
        string[] _deliverables = { "Recipe Design Software",
        "Ingredient Sourcing Tool",
        "Cost Analysis Software",
        "Quality Control System",
        "Production Scheduling Tool",
        "Batch Tracking System",
        "Equipment Maintenance Software",
        "Regulatory Compliance Tool",
        "Ingredient Evaluation System",
        "Allergen Management Software",
        "Packaging Design Tool",
        "Supplier Performance Evaluation System",
        "Shelf Life Testing Software",
        "Sustainability Assessment Tool",
        "Recipe Scaling Calculator",
        "Traceability System",
        "Food Safety Training Module",
        "Inventory Management System",
        "Product Launch Planning Tool",
        "Market Trend Analysis Software"};
        EngineerExperience[] lev = { (EngineerExperience)0, (EngineerExperience)2, (EngineerExperience)1, (EngineerExperience)2, (EngineerExperience)3, (EngineerExperience)1, (EngineerExperience)2, (EngineerExperience)2, (EngineerExperience)0, (EngineerExperience)3, (EngineerExperience)2, (EngineerExperience)1, (EngineerExperience)2, (EngineerExperience)1, (EngineerExperience)2, (EngineerExperience)3, (EngineerExperience)2, (EngineerExperience)1, (EngineerExperience)3, (EngineerExperience)2 };
        int _engId;
        List<int> engineersID = s_dal!.Engineer.ReadAll().Select(x => x.Id).ToList();
        for (int i = 0; i < lev.Length; i++)
        {
            _engId = engineersID[s_rand.Next(0, 5)];
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

            Task tsk = new(0, 0, _alias[i], _dedcription[i], _createAtD, _requiredET, false, lev[i], null, null, null, null, _deliverables[i], _remarks[i]);

            s_dal!.Task.Create(tsk);
        }
    }
    private static void CreatEngineer()
    {
        string[] namesOfEng = { "Shir", "Efrat", "Adina", "Moti", "Israel" };

        foreach (var _name in namesOfEng)
        {
            int _id;
            double _cost;
            string? _mail;
            EngineerExperience? _experience;
            _id = s_rand.Next(1000000, 99999999);
            _cost = s_rand.Next(20, 100);
            _mail = _name + "@gmail.com";
            _experience = (EngineerExperience?)s_rand.Next(0, 5);

            Engineer eng = new(_id, _cost, _mail, _name, _experience);

            s_dal?.Engineer.Create(eng);

            string id = _id.ToString();
            string password = "";
            string userName = "";
            User _newUser = new(_name, id, userName, password, false);
            if(_newUser.UserName != null&& _newUser.Password != null) 
                 s_dal?.User.Create(_newUser);
        }
        //User newAdmin1 = new("Adi", "5784", "Adi123", "a2004", true);
        //User newAdmin2 = new("Shira", "4254","sh222", "Shira222", true);
        //s_dal?.User.Create(newAdmin1);
        //s_dal?.User.Create(newAdmin2);

    }


    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
        {
            //s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
            //s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
            //s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
            //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
            s_dal = DalApi.Factory.Get; //stage 4
            CreatEngineer();
            CreatTask();
            CreateDependencies();

        }
    
}