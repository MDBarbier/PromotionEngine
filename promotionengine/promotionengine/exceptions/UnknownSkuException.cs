using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promotionengine.exceptions
{
    [Serializable]
    public class UnknownSkuException : Exception
    {
        public UnknownSkuException() { }

        public UnknownSkuException(char skuId)
            : base(String.Format("Unknown SKU identifier: {0}", skuId))
        {

        }
    }
}
