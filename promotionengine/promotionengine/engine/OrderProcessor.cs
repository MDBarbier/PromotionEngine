using promotionengine.exceptions;
using promotionengine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.engine
{
    public class OrderProcessor
    {
        public List<Product> ProductList { get; set; }
        public List<Promotion> PromotionList { get; set; }

        public void LoadProducts(List<Product> productsList)
        {
            foreach (var product in productsList)
            {
                product.Validate();
            }

            ProductList = productsList;
        }

        public void LoadPromotions(List<Promotion> promotionList)
        {
            foreach (var promotion in promotionList)
            {
                promotion.Validate();
            }

            PromotionList = promotionList;
        }

        public OrderOutput ProcessOrder(Order order)
        {
            Dictionary<Product, int> matchedProductsOnOrder = new Dictionary<Product, int>();            
            float totalPrice = 0.00f;

            //loop through order items
            foreach (var orderItem in order.OrderItems)
            {
                //match SKU against a product
                var matchedProduct = ProductList.FirstOrDefault(product => product.SkuName == orderItem.Sku);

                //add to list of order items
                if (matchedProduct == null)
                {
                    throw new UnknownSkuException(orderItem.Sku);
                }
                else
                {
                    matchedProductsOnOrder.Add(matchedProduct, orderItem.Amount);
                }
            }            

            //calculate total gross price
            foreach (var item in matchedProductsOnOrder)
            {
                totalPrice += item.Key.UnitPrice * item.Value;
            }

            foreach (var promotion in PromotionList)
            {
                //Is promotion valid based on combinedOrderItems?
                totalPrice = promotion.CheckAndApplyPromotion(matchedProductsOnOrder, totalPrice);
            }

            //return output
            return new OrderOutput() { CustomerName = order.CustomerName, OrderNumber = order.OrderNumber, TotalPrice = totalPrice };
        }
    }
}
