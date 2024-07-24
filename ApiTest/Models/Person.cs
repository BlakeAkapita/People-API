using System.Collections.Generic;

namespace ApiTest.Models
{
    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person : PersonBase
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Used for create and update operations to prevent API users modifying Person ID.
    /// </summary>
    public class PersonBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>
    /// Used to read and write to file.
    /// </summary>
    public class PersonList
    {
        public List<Person> People { get; set; }
    }
}
