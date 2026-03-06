using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Application.Results
{
    public abstract class BaseResult
    {
        protected BaseResult()
        {
            Succeeded = true;
        }
        private List<string> _errors = new List<string>();
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors => _errors;
        public void Failed(string errorMessage)
        {
            Succeeded = false;
            _errors.Add(errorMessage);
        }
    }
}
