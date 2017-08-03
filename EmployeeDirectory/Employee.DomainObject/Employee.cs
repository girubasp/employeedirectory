using System.Collections.Generic;

namespace Employee.DomainObject
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string JobTitle { get; set; }
        public virtual string Location { get; set; }
        public virtual string Email { get; set; }
        public virtual IList<PhoneNumber> PhoneNumbers { get; set; }
    }
}
