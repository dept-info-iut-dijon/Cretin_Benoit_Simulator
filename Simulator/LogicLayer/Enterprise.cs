using LogicLayer.Products;

namespace LogicLayer
{
    /// <summary>
    /// Enterprise simulation
    /// </summary>
    public class Enterprise : Subject
    {
        #region associations
        private Workshop workshop;
        private Stock stock;
        private ClientService clients;
        #endregion

        #region Properties 
        /// <summary>
        /// Gets the amount of money that enterprise disposes
        /// </summary>
        public int Money 
        { get => money; 
          set 
            { 
                this.money = value;
                base.NotifyMoneyChange(Money);
            }
        }
        private int money;

        private int materials;
        /// <summary>
        /// Gets the amount of materials that enterprise disposes
        /// </summary>
        public int Materials
        {
            get => materials;
            set
            {
                this.materials = value;
                base.NotifyMaterialChange(materials);
            }
        }

        private int employees;
        /// <summary>
        /// Gets the number of employees
        /// </summary>
        public int Employees { get => employees; }

        /// <summary>
        /// Gets the number of free employees (they can work)
        /// </summary>
        public int FreeEmployees
        {
            get => employees - EmployeesWorkshop;
        }
       
        /// <summary>
        /// Gets the number of employees working in the workshop
        /// </summary>
        public int EmployeesWorkshop { get => workshop.NbEmployees; } 

        /// <summary>
        /// Gets the total amount of stock
        /// </summary>
        public int TotalStock 
        { 
            get => stock.TotalStock;         
        }

        private ProductFactory productFactory;


        private System.Threading.Timer timer;


        #endregion

        #region Constructors

        /// <summary>
        /// Initialize the enterprise
        /// </summary>
        public Enterprise()
        {
            money = 300000;
            employees = 4;
            materials = 100;  
            workshop = new Workshop();
            stock = new Stock();
            clients = new ClientService();
            Initializer.InitClients(clients);
            this.productFactory = new ProductFactory();
            Initializer.InitFactory(this.productFactory);
            this.timer = new Timer(EndOfMonth);
            timer.Change(0, LogicLayer.Constants.MONTH_TIME);
        }


        #endregion

        #region destructor

        ~Enterprise()
        {
            timer.Dispose();
        }

        #endregion

        #region methods
        /// <summary>
        /// Buy some materials
        /// </summary>
        /// <exception cref="NotEnoughMoney">If insufisant funds</exception>
        public void BuyMaterials()
        {
            int cost = Constants.MATERIALS * Constants.COST_MATERIALS;
            if (money < cost)
                throw new NotEnoughMoney();
            Money -= cost;
            Materials += Constants.MATERIALS;
        }

        /// <summary>
        /// Hire a new emloyee
        /// </summary>        
        public void Hire()
        {
            ++employees;
            base.NotifyEmployeesChange(FreeEmployees, employees);
        }

        /// <summary>
        /// DIsmiss an employee
        /// </summary>
        /// <exception cref="NoEmployee">If no employee to dismiss</exception>
        /// <exception cref="NotEnoughMoney">If not enough money to pay the bonus</exception>
        /// <exception cref="EmployeeWorking">If all employees worked, no dismiss is possible</exception>
        public void Dismiss()
        {
            if (employees < 1) throw new NoEmployee();
            int cost = Constants.BONUS;
            if (money < cost)
                throw new NotEnoughMoney();
            if (FreeEmployees < 1)
                throw new EmployeeWorking();
            Money -= cost;
            employees--;
            base.NotifyEmployeesChange(FreeEmployees, employees);
        }

        /// <summary>
        /// Start a product production
        /// </summary>
        /// <param name="type">a string identifying kind of product</param>
        /// <exception cref="ProductUnknown">the type is unknown</exception>
        /// <exception cref="NotEnoughMaterials">Not enough materials to build</exception>
        /// <exception cref="NoEmployee">Not enough employee to build</exception>
        public void MakeProduct(string type)
        {
            Product p;
            switch(type)
            {
                case "bike":
                    p = productFactory.Create("bike");
                    break;
                case "scooter":
                    p = productFactory.Create("scooter");
                    break;
                case "car":
                    p = productFactory.Create("car");
                    break;
                default:
                    throw new ProductUnknown();
            }
            // test if the product can be build
            if (materials < p.MaterialsNeeded)
                throw new NotEnoughMaterials();
            if (employees - EmployeesWorkshop < p.EmployeesNeeded)
                throw new NoEmployee();

            Materials -= p.MaterialsNeeded; // consume materials
            // start the building...
            workshop.StartProduction(p);
            NotifyEmployeesChange(FreeEmployees, Employees);
        }

        /// <summary>
        /// Update the productions & the stock
        /// </summary>
        /// <exception cref="UnableToStock">If stock is full</exception>
        public void UpdateProductions()
        {
            // update informations about productions
            var list = workshop.ProductsDone(); 
            // add finish products in stock
            foreach(var product in list)
            {
                stock.Add(product);
                base.NotifyStockChange(stock.TotalStock);
                workshop.Remove(product);
                NotifyEmployeesChange(FreeEmployees, Employees);
            }

        }

        /// <summary>
        /// Get the numbers of products of a type workshop build
        /// </summary>
        /// <param name="v">kind of product</param>
        /// <returns>number of products building</returns>        
        public int GetProduction(string v)
        {
            return workshop.InProduction(v);
        }

        /// <summary>
        /// Gets the number of products stocked
        /// </summary>
        /// <param name="v">type of product</param>
        /// <returns>number stocked</returns>
        public int GetStock(string v)
        {
            return stock.GetNbOfType(v);
        }

        /// <summary>
        /// Pay all the employees
        /// </summary>
        /// <exception cref="NotEnoughMoney">if money is not enough !</exception>
        public void PayEmployees()
        {
            int cost = employees * Constants.SALARY;
            if (cost > money)
                throw new NotEnoughMoney();
            Money -= cost;
        }

        /// <summary>
        /// Update the buying status
        /// </summary>
        public void UpdateBuying()
        {            
            if(clients.WantToBuy("bike"))
            {
                TrySell("bike");
            }
            if(clients.WantToBuy("scooter"))
            {
                TrySell("scooter");
            }
            if(clients.WantToBuy("car"))
            {
                TrySell("car");
            }
        }

        private void TrySell(string type)
        {
            Product? p = stock.Find(type);
            if(p!=null)
            {
                stock.Remove(p);
                base.NotifyStockChange(stock.TotalStock);
                Money += p.Price;
                clients.Buy(type);
            }
        }

        /// <summary>
        /// update client needs
        /// </summary>
        public void UpdateClients()
        {            
            clients.UpdateClients();
        }

        /// <summary>
        /// Get clients needs
        /// </summary>
        /// <param name="type">type of product clients wanted</param>
        /// <returns>number of potential clients</returns>
        /// <exception cref="ProductUnknown">If type unknown</exception>
        public int GetAskClients(string type)
        {
            return clients.GetAskFor(type);
        }

        private void EndOfMonth(object? state)
        {
            PayEmployees();
            UpdateClients();
        }

        #endregion



    }
}