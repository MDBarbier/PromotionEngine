using promotionengine.engine;
using promotionengine.interfaces;
using promotionengine.models;
using System.Collections.Generic;
using Xunit;

namespace promotionengine.tests
{
    public class promotiontests
    {
        private List<Product> ProductList = new List<Product>();
        private List<IPromotion> PromotionList = new List<IPromotion>();

        [Theory]
        [InlineData("ScenarioA", 1, 1, 1, 0, 100.00)] //No promotions matched   
        [InlineData("ScenarioB", 5, 5, 1, 0, 370.00)] //Matches promotion 1 and 2
        [InlineData("ScenarioC", 3, 5, 1, 1, 280.00)] //Match promotions 1, 2 and 3
        [InlineData("ScenarioD", 3, 1, 1, 0, 180.00)] //Matches promotion 1 only
        [InlineData("ScenarioE", 6, 0, 0, 0, 260.00)] //Matches promotion 1 (x2)
        [InlineData("ScenarioF", 6, 0, 1, 1, 290.00)] //Matches promotion 1 (x2) and 3
        [InlineData("ScenarioG", 6, 0, 2, 2, 320.00)] //Matches promotion 1 (x2) and 3 (x2)
        [InlineData("ScenarioH", 0, 8, 0, 0, 180.00)] //Matches promotion 1 (x2) and 3 (x2)
        [InlineData("ScenarioI", 6, 8, 2, 2, 500.00)] //Matches promotion 1 (x2), 2 (x4) and 3 (x2)
        [InlineData("ScenarioI", 0, 8, 2, 2, 240.00)] //Matches promotion 2 (x4) and 3 (x2)
        [InlineData("ScenarioI", 0, 0, 2, 1, 50.00)] //Matches promotion 3 (x1)
        [InlineData("ScenarioI", 2, 0, 2, 1, 150.00)] //Matches promotion 3 (x1)
        public void ScenarioTest(string testScenarioName, int amountSkuA, int amountSkuB, int amountSkuC, int amountSkuD, float expectedTotalPrice)
        {
            //Setup
            SetupTestData();
            OrderProcessor orderProcessor = new OrderProcessor();
            orderProcessor.LoadProducts(ProductList);
            orderProcessor.LoadPromotions(PromotionList);            
            Order order = new Order()
            {
                OrderItems = new List<OrderItem>() { new OrderItem() { Sku = 'A', Amount = amountSkuA }, new OrderItem() { Sku = 'B', Amount = amountSkuB },
                new OrderItem() { Sku = 'C', Amount = amountSkuC }, new OrderItem() { Sku = 'D', Amount = amountSkuD }}
            };            

            //Execution
            OrderOutput orderOutput = orderProcessor.ProcessOrder(order);

            //Assertion
            Assert.True(orderOutput.TotalPrice == expectedTotalPrice, $"Test failure ({testScenarioName}): TotalPrice was {orderOutput.TotalPrice} but expected {expectedTotalPrice}");
        }

        private void SetupTestData()
        {
            Product productA = new Product() { SkuName = 'A', UnitPrice = 50.00f };
            Product productB = new Product() { SkuName = 'B', UnitPrice = 30.00f };
            Product productC = new Product() { SkuName = 'C', UnitPrice = 20.00f };
            Product productD = new Product() { SkuName = 'D', UnitPrice = 15.00f };
            ProductList = new List<Product>() { productA, productB, productC, productD };
            
            PromotionList = new List<IPromotion>() { new Promotion1(), new Promotion2(), new Promotion3()};
        }
    }
}
