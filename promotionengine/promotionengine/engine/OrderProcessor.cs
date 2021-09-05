using promotionengine.exceptions;
using promotionengine.interfaces;
using promotionengine.models;
using System.Collections.Generic;
using System.Linq;

namespace promotionengine.engine
{
    public class OrderProcessor
    {
        public List<Product> ProductList { get; set; }
        public List<IPromotion> PromotionList { get; set; }

        public void LoadProducts(List<Product> productsList)
        {
            foreach (var product in productsList)
            {
                product.Validate();
            }

            ProductList = productsList;
        }

        public void LoadPromotions(List<IPromotion> promotionList)
        {
            foreach (var promotion in promotionList)
            {
                promotion.Validate();
            }

            PromotionList = promotionList;
        }

        public OrderOutput ProcessOrder(Order order)
        {
            Dictionary<Product, int> matchedProductsOnOrder = CollateOrderItems(order);
            float totalPrice = CalculateTotalPrice(matchedProductsOnOrder);
            return new OrderOutput() { CustomerName = order.CustomerName, OrderNumber = order.OrderNumber, TotalPrice = totalPrice };
        }

        private float CalculateTotalPrice(Dictionary<Product, int> matchedProductsOnOrder)
        {
            var totalPrice = CalculateTotalGrossPrice(matchedProductsOnOrder);
            totalPrice = ProcessPromotions(matchedProductsOnOrder, totalPrice);
            return totalPrice;
        }

        private float ProcessPromotions(Dictionary<Product, int> matchedProductsOnOrder, float totalPrice)
        {
            foreach (var promotion in PromotionList)
            {
                totalPrice = promotion.CheckAndApplyPromotion(matchedProductsOnOrder, totalPrice);
            }

            return totalPrice;
        }

        private static float CalculateTotalGrossPrice(Dictionary<Product, int> matchedProductsOnOrder)
        {
            float totalPrice = 0.00f;

            foreach (var item in matchedProductsOnOrder)
            {
                totalPrice += item.Key.UnitPrice * item.Value;
            }

            return totalPrice;
        }

        private Dictionary<Product, int> CollateOrderItems(Order order)
        {
            Dictionary<Product, int> matchedProductsOnOrder = new Dictionary<Product, int>();
            
            foreach (var orderItem in order.OrderItems)
            {                
                var matchedProduct = ProductList.FirstOrDefault(product => product.SkuName == orderItem.Sku);
                                
                if (matchedProduct == null)
                {
                    throw new UnknownSkuException(orderItem.Sku);
                }
                else
                {
                    matchedProductsOnOrder.Add(matchedProduct, orderItem.Amount);
                }
            }

            return matchedProductsOnOrder;
        }
    }
}
