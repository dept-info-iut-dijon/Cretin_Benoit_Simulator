using LogicLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Initialise différentes classes du projet
    /// </summary>
    public class Initializer
    {
        /// <summary>
        /// Initialise la product factory passée en paramètre
        /// </summary>
        /// <param name="productFactory">product factory à initialiser</param>
        public static void InitFactory(ProductFactory productFactory)
        {
            productFactory.Register("bike", new MakeBike());
            productFactory.Register("scooter", new MakeScooter());
            productFactory.Register("car", new MakeCar());
        }

        public static void InitClients(ClientService clientService)
        {
            clientService.InitNeeds("bike", 0);
            clientService.InitNeeds("scooter", 0);
            clientService.InitNeeds("car", 0);

            clientService.InitProbs("bike", 20);
            clientService.InitProbs("scooter", 14);
            clientService.InitProbs("car", 10);
        }
    }
}
