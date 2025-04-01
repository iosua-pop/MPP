using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;

namespace AgentieTurism.Models
{
    public class Employee : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Employee() { }

        public Employee(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public override string ToString()
        {
            return $"Employee {{ Id={Id}, Name={Name}, Email={Email} }}";
        }
    }
}