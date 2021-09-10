using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Gateways.Database
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> IsUniqueAsync(Person person)
        {
            var query = (person.Id > 0)
                ? _context.Person.Where(p => p.Id != person.Id)
                : _context.Person;

            return await query.AllAsync(p => p.Name != person.Name);
        }

        public override async Task<Person> GetByIdAsync(int id)
        {
            return await _context.Person
                .Include(p => p.Items)
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
