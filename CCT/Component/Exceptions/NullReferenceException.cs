using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Component.Exceptions
{
    public class NullReferenceException : SystemException
    {
        public NullReferenceException() { }
        public NullReferenceException(string message) { }
    }
}
