using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Interfaces.UseCases
{
    public interface IPersonUseCases
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> GetPersonByIdAsync(int id);
        Task InsertPersonAsync(string personName);
        Task UpdatePersonAsync(PersonUpdateRequestDto personUpdateRequest);
        Task DeletePersonAsync(int personId);
    }
}
