using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Application.Results
{
    public class ServiceResultOfT<T> : BaseResult
    {
        public T Data { get; set; }
    }
}
