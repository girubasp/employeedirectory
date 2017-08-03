using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DomainObject
{
    public class PhoneNumber
    {
        public virtual int Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Number { get; set; }
    }
}
