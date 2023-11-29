using LogicLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Favrique de produit
    /// </summary>
    public class ProductFactory
    {
        private Dictionary<string, IMakeProduct> products = new Dictionary<string, IMakeProduct>();


        /// <summary>
        /// Enregistre un produit associé à son nom dans le dictionnaire
        /// </summary>
        /// <param name="productName">nom du produit</param>
        /// <param name="product">Produit</param>
        public void Register(String productName, IMakeProduct product)
        {
            this.products[productName] = product;
        }

        /// <summary>
        /// Renvoie un tableau avec tous les noms de produits contenus dans le dictionnaire
        /// </summary>
        public String[] ProdcutNames
        {
            get { return products.Keys.ToArray(); }
        }


        public Product Create(string productName)
        {
            if (!products.ContainsKey(productName))
                throw new Exception("Product name " + productName + " unknown");
            return products[productName].CreateProduct();
        }
    }
}
