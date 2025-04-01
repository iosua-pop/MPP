using System;

namespace MinimalApiApp.Models
{
    public class Person
    {
        public int Age { get; set; }

        public string Name { get; private set; }

        public DateTime Birthday { get; init; }

        public string Title { get; private set; } = "Junior";

        public Person()
        {
            Name = "Ceva";
        }

        public Person(int age, string name, DateTime birthday)
        {
            Age = age;
            Name = name;
            Birthday = birthday;
        }

        public bool Promote(string newTitle)
        {
            Title = newTitle;

            return true;
        }
    }
}

namespace MinimalApiApp.UserModels
{
    public class UserPerson
    {
        public int Age { get; set; }

        public string Name { get; private set; }

        public DateTime Birthday { get; init; }

        public string Title { get; private set; } = "Junior";

        public Person()
        {
            Name = "Ceva";
        }

        public Person(int age, string name, DateTime birthday)
        {
            Age = age;
            Name = name;
            Birthday = birthday;
        }

        public bool Promote(string newTitle)
        {
            Title = newTitle;

            return true;
        }
    }
}
