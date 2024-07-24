using ApiTest.Helpers;
using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTest.Controllers
{
    [Route("/")]
    [ApiController]
    public class PeopleController
    {
        [HttpGet]
        public ActionResult<List<Person>> GetPeople()
        {
            var people = JsonHelper.ReadFromJsonFile();

            return people;
        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetPersonById(int id)
        {
            var people = JsonHelper.ReadFromJsonFile();
            var person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
                return new NotFoundResult();

            return person;
        }

        [HttpGet("search")]
        public ActionResult<List<Person>> Search([FromQuery] string nameToSearch)
        {
            var people = JsonHelper.ReadFromJsonFile();

            var results = people.Where(p => p.Name.Contains(nameToSearch, StringComparison.OrdinalIgnoreCase)).ToList();

            return results;
        }

        [HttpPost]
        public ActionResult<Person> CreatePerson([FromBody] PersonBase newPerson)
        {
            var people = JsonHelper.ReadFromJsonFile();

            var lastPerson = people.LastOrDefault();
            var newPersonId = lastPerson != null ? lastPerson.Id + 1 : 0;

            var personToAdd = new Person
            {
                Id = newPersonId,
                Name = newPerson.Name,
                Age = newPerson.Age,
            };

            people.Add(personToAdd);

            JsonHelper.WriteToJsonFile(people);

            return new CreatedResult("/", personToAdd);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePerson(int id, [FromBody] Person updatedPerson)
        {
            var people = JsonHelper.ReadFromJsonFile();
            var person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
                return new NotFoundResult();

            person.Name = updatedPerson.Name;
            person.Age = updatedPerson.Age;
            JsonHelper.WriteToJsonFile(people);

            return new OkObjectResult(person);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePerson(int id)
        {
            var people = JsonHelper.ReadFromJsonFile();
            var person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
                return new NotFoundResult();

            people.Remove(person);
            JsonHelper.WriteToJsonFile(people);

            return new NoContentResult();
        }
    }
}
