﻿using LogicLayer;
using LogicLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver
    {
        private LogicLayer.Enterprise enterprise;
        private Timer timerSecond;
        private Timer timerWeek;
        public MainWindow()
        {
            InitializeComponent();


            enterprise = new LogicLayer.Enterprise();
            DataContext = enterprise;
            timerSecond = new Timer(TimerSecondTick);
            timerSecond.Change(0, LogicLayer.Constants.TIME_SLICE); 
            timerWeek = new Timer(TimerWeekTick);
            timerWeek.Change(0, LogicLayer.Constants.WEEK_TIME);
            enterprise.Register(this);
            MoneyChange(enterprise.Money);
            MaterialChange(enterprise.Materials);
            StockChange(enterprise.TotalStock);
            EmployeesChange(enterprise.FreeEmployees, enterprise.Employees);

        }

        private void TimerSecondTick(object? data)
        {
            Dispatcher.Invoke(() =>
            {
                // every second, to update screen
                UpdateScreen();
            });
            
        }

        private void TimerWeekTick(object? data)
        {
            Dispatcher.Invoke(() =>
            {
                // nothing to do every week...
            });
        }


        private void EndOfSimulation()
        {
            MessageBox.Show("END OF SIMULATION");
            Close();
        }

        private void UpdateScreen()
        {
            enterprise.UpdateProductions();
            enterprise.UpdateBuying();
            

            bikesProd.Content = enterprise.GetProduction("bike").ToString();
            scootsProd.Content = enterprise.GetProduction("scooter").ToString();
            carsProd.Content = enterprise.GetProduction("car").ToString();

            bikeStock.Content = enterprise.GetStock("bike").ToString();
            scootStock.Content = enterprise.GetStock("scooter").ToString();
            carStock.Content = enterprise.GetStock("car").ToString();

        }

        private void BuyMaterials(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.BuyMaterials();
                UpdateScreen();
            }
            catch(LogicLayer.NotEnoughMoney)
            {
                MessageBox.Show("Not enough money to buy materials !");
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void Hire(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.Hire();
                UpdateScreen();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void Dismiss(object sender, RoutedEventArgs e)
        {
            try
            {
                enterprise.Dismiss();
                UpdateScreen();
            }
            catch(LogicLayer.NoEmployee)
            {
                MessageBox.Show("There is no employee to dismiss");
            }
            catch(LogicLayer.NotEnoughMoney)
            {
                MessageBox.Show("There is not enough money to puy dismiss bonus");
            }
            catch(LogicLayer.EmployeeWorking)
            {
                MessageBox.Show("You can't dismiss no : employees working");
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void BuildProduct(string s)
        {
            try
            {
                enterprise.MakeProduct(s);
                UpdateScreen();
            }
            catch (LogicLayer.ProductUnknown)
            {
                MessageBox.Show("I don't know how to make " + s);
            }
            catch (LogicLayer.NotEnoughMaterials)
            {
                MessageBox.Show("You do not have suffisent materials to build a "+s);
            }
            catch (LogicLayer.NoEmployee)
            {
                MessageBox.Show("You do not have enough employees to build a "+s);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        private void BuildBike(object sender, RoutedEventArgs e)
        {
            BuildProduct("bike");
        }

        private void BuildScooter(object sender, RoutedEventArgs e)
        {
            BuildProduct("scooter");
        }

        private void BuildCar(object sender, RoutedEventArgs e)
        {
            BuildProduct("car");
        }

        public void MoneyChange(int money)
        {
            Dispatcher.Invoke(() =>
            {
                this.money.Content = money.ToString("C");
            });
            
        }

        public void StockChange(int stock)
        {
            totalStock.Content = stock.ToString() + " %";
        }

        public void MaterialChange(int materials)
        {
            this.materials.Content = materials.ToString();
        }

        public void EmployeesChange(int free, int total)
        {
            this.employees.Content = free.ToString() + "/" + total.ToString();
        }

        public void ClientNeedsChange(string type, int need)
        {
            Dispatcher.Invoke(() =>
            {
                switch (type)
                {
                    case "bike": bikeAsk.Content = need.ToString(); break;
                    case "scooter": scootAsk.Content = need.ToString(); break;
                    case "car": carAsk.Content = need.ToString(); break;
                }
            });
        }
    }
}
