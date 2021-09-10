using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Enums;
using GripItemTrader.Core.Exceptions;
using GripItemTrader.Core.Interfaces.Repositories;
using GripItemTrader.Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GripItemTrader.UseCases
{
    public class PersonUseCases : IPersonUseCases
    {
        private readonly IPersonRepository _personRepository;

        public PersonUseCases(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task DeletePersonAsync(int personId)
        {
            var person = await EnsureExists(personId);

            person.IsActive = false;
            InactivateItems(person);

            await _personRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            return await _personRepository.GetAllAsync();
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
            var person = await EnsureExists(id);
            return person;
        }

        public async Task InsertPersonAsync(string personName)
        {
            var person = PersonFactory(personName);

            EnsurePersonIsValid(person);

            await EnsureIsUnique(person);

            await _personRepository.InsertAsync(person);

            await _personRepository.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(PersonUpdateRequestDto personUpdateRequest)
        {
            var currentPerson = await EnsureExists(personUpdateRequest.Id);
            currentPerson.Name = personUpdateRequest.Name.Trim();

            EnsurePersonIsValid(currentPerson);

            await EnsureIsUnique(currentPerson);

            await _personRepository.SaveChangesAsync();
        }

        private async Task<Person> EnsureExists(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person != null)
                return person;

            throw new GripItemTraderException(GripItemTraderError.PERSON_NOT_FOUND, "Person not fouond for the given Id");
        }

        private async Task EnsureIsUnique(Person person)
        {
            bool isUnique = await _personRepository.IsUniqueAsync(person);
            if (!isUnique)
                throw new GripItemTraderException(GripItemTraderError.PERSON_NOT_UNIQUE, "Already exists a person with the given Name");
        }

        private static void EnsurePersonIsValid(Person person)
        {
            if (string.IsNullOrEmpty(person.Name))
                throw new GripItemTraderException(GripItemTraderError.PERSON_NAME_REQUIRED);
            if (person.Name.Length > 100)
                throw new GripItemTraderException(GripItemTraderError.PERSON_NAME_TOO_LONG);
        }

        private static Person PersonFactory(string personName)
        {
            return new Person
            {
                Name = personName.Trim(),
                IsActive = true
            };
        }

        private static void InactivateItems(Person person)
        {
            if (person.Items != null && person.Items.Any())
                foreach (var item in person.Items)
                    item.IsActive = false;
        }
    }
}
