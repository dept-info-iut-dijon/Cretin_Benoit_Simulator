using LogicLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Initializer
    {
        public static void InitFactory(ProductFactory productFactory)
        {
            productFactory.Register("bike", new MakeBike());
            productFactory.Register("scooter", new MakeScooter());
            productFactory.Register("car", new MakeCar());
        }
    }
}
