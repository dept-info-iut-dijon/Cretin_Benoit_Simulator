using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class FakeObserver : IObserver
    {
        public void EmployeesChange(int free, int total)
        {
            
        }

        private int materials = 0;
        public int Materials { get => materials; }
        public void MaterialChange(int materials)
        {
            this.materials = materials;
        }

        public void MoneyChange(int money)
        {
           
        }

        public void StockChange(int stock)
        {
            
        }
    }
}
