using LogicLayer.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogicLayer
{
    public class TestProductFactory
    {
        [Fact]
        public void TestRegister()
        {
            ProductFactory pf = new ProductFactory();
            IMakeProduct bike = new MakeBike();
            pf.Register("bike", bike);
            Assert.Equal("bike", pf.ProdcutNames[0]);
        }

        [Fact]
        public void testInitFactory()
        {
            ProductFactory pf = new ProductFactory();
            Initializer.InitFactory(pf);
            Assert.Equal("bike", pf.ProdcutNames[0]);
        }
    }
}
