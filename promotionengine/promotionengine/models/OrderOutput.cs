using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.models
{
    public class OrderOutput
    {
        public float TotalPrice { get; set; }
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
    }
}
