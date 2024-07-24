using System.Collections.Generic;

namespace ApiTest.Models
{
    public class Person : PersonBase
    {
        public int Id { get; set; }
    }

    public class PersonBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class PersonList
    {
        public List<Person> People { get; set; }
    }
}
