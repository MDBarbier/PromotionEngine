using System;
using Xunit;
using promotionengine.engine;
using promotionengine.models;
using System.Collections.Generic;

namespace promotionengine.tests
{
    public class promotiontests
    {
        private List<Product> ProductList = new List<Product>();
        private List<Promotion> PromotionList = new List<Promotion>();

        [Theory]
        [InlineData("ScenarioA", 1, 1, 1, 0, 100.00)]        
        [InlineData("ScenarioB", 5, 5, 1, 0, 370.00)]
        [InlineData("ScenarioC", 3, 5, 1, 1, 280.00)]
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
            Product productB = new Product() { SkuName = 'B', UnitPrice = 50.00f };
            Product productC = new Product() { SkuName = 'C', UnitPrice = 50.00f };
            Product productD = new Product() { SkuName = 'D', UnitPrice = 50.00f };
            ProductList = new List<Product>() { productA, productB, productC, productD };

            Promotion promotion1 = new Promotion() { SingleSku = true, CombinedSku = false, ApplicableSkus = new char[] { 'A' }, NumUnitsRequired = 3, FixedPrice = 130.00f, PercentageDiscount = 0.00f };
            Promotion promotion2 = new Promotion() { SingleSku = true, CombinedSku = false, ApplicableSkus = new char[] { 'B' }, NumUnitsRequired = 2, FixedPrice = 45.00f, PercentageDiscount = 0.00f };
            Promotion promotion3 = new Promotion() { SingleSku = false, CombinedSku = true, ApplicableSkus = new char[] { 'C', 'D' }, NumUnitsRequired = 1, FixedPrice = 30.00f, PercentageDiscount = 0.00f};
            PromotionList = new List<Promotion>() { promotion1, promotion2, promotion3 };
        }
    }
}
