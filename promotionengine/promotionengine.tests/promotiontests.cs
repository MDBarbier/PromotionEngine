using System;
using Xunit;
using promotionengine.engine;
using promotionengine.models;

namespace promotionengine.tests
{
    public class promotiontests
    {
        private List<Product> ProductList = new List<Product>();
        private List<Promotion> PromotionList = new List<Promotion>();

        [Theory]
        [InlineData(1, 1, 1, 0, 100.00)]        
        [InlineData(5, 5, 1, 0, 370.00)]
        [InlineData(3, 5, 1, 1, 280.00)]
        public void ScenarioTest(int amountSkuA, int amountSkuB, int amountSkuC, int amountSkuD, float expectedTotalPrice)
        {
            //Setup
            SetupTestData();
            LoadProducts(ProductList);
            LoadPromotions(PromotionList);
            Order order = new Order() { new OrderItem() { Sku = 'A', Amount = amountSkuA }, new OrderItem() { Sku = 'B', Amount = amountSkuB },
                new OrderItem() { Sku = 'C', Amount = amountSkuD }, new OrderItem() { Sku = 'D', Amount = amountSkuD }};            

            //Execution
            OrderOutput orderOutput = OrderProcessor.ProcessOrder(order);

            //Assertion
            Assert.True(orderOutput.TotalPrice == expectedTotalPrice, $"Test failure ({nameof(ScenarioATest)}): TotalPrice was {orderOutput.TotalPrice} but expected {expectedTotalPrice}");
        }

        private void SetupTestData()
        {
            Product productA = new Product() { Sku = 'A', Price = 50.00 };
            Product productB = new Product() { Sku = 'B', Price = 50.00 };
            Product productC = new Product() { Sku = 'C', Price = 50.00 };
            Product productD = new Product() { Sku = 'D', Price = 50.00 };
            ProductList = new List<Product>() { productA, productB, productC, productD };

            Promotion promotion1 = new Promotion() { SingleSku = true, CombinedSku = false, ApplicableSkus = new char[] { 'A' }, NumUnitsRequired = 3, FixedPrice = 130.00, PercentageDiscount = 0.00 };
            Promotion promotion2 = new Promotion() { SingleSku = true, CombinedSku = false, ApplicableSkus = new char[] { 'B' }, NumUnitsRequired = 2, FixedPrice = 45.00, PercentageDiscount = 0.00 };
            Promotion promotion3 = new Promotion() { SingleSku = false, CombinedSku = true, ApplicableSkus = new char[] { 'C', 'D' }, NumUnitsRequired = 1, FixedPrice = 30.00, PercentageDiscount = 0.00 };
            PromotionList = new List<Promotion>() { promotion1, promotion2, promotion3 };
        }
    }
}
