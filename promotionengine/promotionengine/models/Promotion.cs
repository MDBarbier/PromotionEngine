using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.models
{
    public class Promotion
    {
        public bool SingleSku { get; set; }
        public bool CombinedSku { get; set; }
        public char[] ApplicableSkus { get; set; }
        public int NumUnitsRequired { get; set; }
        public float FixedPrice { get; set; }
        public float PercentageDiscount { get; set; }
    }
}
