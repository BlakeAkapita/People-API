using ApiTest.Helpers;
using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTest.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PeopleController
    {
        /// <summary>
        /// Gets every person in the JSON file.
        /// </summary>
        /// <returns>A list of people.</returns>
        [HttpGet]
        public ActionResult<List<Person>> GetPeople()
        {
            var people = JsonHelper.ReadFromJsonFile();

            return people;
        }

        /// <summary>
        /// Gets a single person from the JSON file if they exist.
        /// </summary>
        /// <param name="id">Person ID.</param>
        /// <returns>A single person.</returns>
        [HttpGet("{id}")]
        public ActionResult<Person> GetPersonById(int id)
        {
            var people = JsonHelper.ReadFromJsonFile();
            var person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
                return new NotFoundResult();

            return person;
        }

        /// <summary>
        /// Searches for one or more people who match the specified name search string.
        /// </summary>
        /// <param name="nameToSearch">The string used to search people's names.</param>
        /// <returns>One or more people who match the search string, or none of no person matches the search string.</returns>
        [HttpGet("search")]
        public ActionResult<List<Person>> Search([FromQuery] string nameToSearch)
        {
            var people = JsonHelper.ReadFromJsonFile();

            var results = people.Where(p => p.Name.Contains(nameToSearch, StringComparison.OrdinalIgnoreCase)).ToList();

            return results;
        }

        /// <summary>
        /// Creates a new person and writes to the JSON data file.
        /// </summary>
        /// <param name="newPerson">An Person object in the request body specifying the new person's details.</param>
        /// <returns>A 201 success code on creation.</returns>
        [HttpPost]
        public ActionResult<Person> CreatePerson([FromBody] PersonBase newPerson)
        {
            var people = JsonHelper.ReadFromJsonFile();

            // Get the last person's ID when creating a person to ensure IDs are sequential
            var lastPerson = people.LastOrDefault();
            var newPersonId = lastPerson != null ? lastPerson.Id + 1 : 0;

            // Create new person
            var personToAdd = new Person
            {
                Id = newPersonId,
                Name = newPerson.Name,
                Age = newPerson.Age,
            };

            // Add person to the list of people and write to file
            people.Add(personToAdd);
            JsonHelper.WriteToJsonFile(people);

            return new CreatedResult("/", personToAdd);
        }

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="id">The ID of the person to update.</param>
        /// <param name="updatedPerson">A Person object in the request body that updates the person's details.</param>
        /// <returns>A 200 success code if the person was updated, or 404 if the person does not exist.</returns>
        [HttpPut("{id}")]
        public ActionResult UpdatePerson(int id, [FromBody] PersonBase updatedPerson)
        {
            // Get the person to update if they exist
            var people = JsonHelper.ReadFromJsonFile();
            var person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
                return new NotFoundResult();

            // Update person's details
            person.Name = updatedPerson.Name;
            person.Age = updatedPerson.Age;
            JsonHelper.WriteToJsonFile(people);

            return new OkObjectResult(person);
        }

        /// <summary>
        /// Deletes a person from the JSON file.
        /// </summary>
        /// <param name="id">The ID of the person to delete.</param>
        /// <returns>A 204 success code if the person was successfully deleted.</returns>
        [HttpDelete("{id}")]
        public ActionResult DeletePerson(int id)
        {
            // Get the person to delete if they exist
            var people = JsonHelper.ReadFromJsonFile();
            var person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
                return new NotFoundResult();

            // Remove the person from the list and write to file
            people.Remove(person);
            JsonHelper.WriteToJsonFile(people);

            return new NoContentResult();
        }
    }
}
