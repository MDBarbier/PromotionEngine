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
            return new OrderOutput();
        }
    }
}
