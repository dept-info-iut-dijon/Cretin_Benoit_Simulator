using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Subject
    {
        private List<IObserver> observers = new List<IObserver>();

        public void Register(IObserver obs)
        {
            observers.Add(obs);
        }

        public void UnRegister(IObserver obs)
        {
            observers.Remove(obs);
        }

        protected void NotifyMoneyChange(int money)
        {
            foreach(var obs in observers)
            {
                obs.MoneyChange(money);
            }
        }
    }
}
