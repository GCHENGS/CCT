using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Component.Exceptions
{
    public class ResourceReferenceKeyNotFoundException : InvalidOperationException
    {
        public ResourceReferenceKeyNotFoundException() { }
        public ResourceReferenceKeyNotFoundException(string message, object resourceKey) { }
    }
}
