using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.models
{
    public class Order : System.Collections.IEnumerable
    {
        public List<OrderItem> OrderItems { get; set; }
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }

        public IEnumerator GetEnumerator()
        {
            foreach (var OrderItem in OrderItems)
            {
                yield return OrderItem;
            }
        }
    }
}
