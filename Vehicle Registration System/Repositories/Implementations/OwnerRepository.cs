using Microsoft.EntityFrameworkCore;
using Vehicle_Registration_System.Data;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;

namespace Vehicle_Registration_System.Repositories.Implementations
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOwner(InputOwner input)
        {
            Owner requestBody = new Owner();

            requestBody.FirstName = input.FirstName;
            requestBody.LastName = input.LastName;
            requestBody.DateOfBirth = input.DateOfBirth;
            requestBody.PlaceOfBirth = input.PlaceOfBirth;
            requestBody.Email = input.Email;
            requestBody.Phone = input.Phone;
            requestBody.Gender = input.Gender;
            requestBody.Address = input.Address;
            requestBody.LicenseIssueDate = input.LicenseIssueDate;

            _context.Owners.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Owner>> FindByCity(string placeOfBirth)
        {
            return await _context.Owners
                                 .Where(o => o.PlaceOfBirth == placeOfBirth)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Owner>> FindOwnerByVehicle(string manufacturer, string model)
        {
            return await _context.Owners
                                 .Where(o => o.Vehicles.Any(v => v.Manufacturer == manufacturer && v.Model == model))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Owner>> GetAllOwners()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<Owner> GetOwnerById(int id)
        {
            var owner = await _context.Owners.FindAsync(id);

            return owner ?? new Owner();
        }

        public async Task<long> GetLicensesByCity(string placeOfBirth)
        {
            return await _context.Owners.CountAsync(o => o.PlaceOfBirth == placeOfBirth);
        }

        public async Task UpdateOwner(EditOwner edit, int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                owner.Email = edit.Email;
                owner.Phone = edit.Phone;
                owner.Address = edit.Address;

                _context.Entry(owner).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Owner>> GetOwnersByName(string name)
        {
            return await _context.Owners
                                 .Where(o => o.FirstName.Contains(name) || o.LastName.Contains(name))
                                 .ToListAsync();
        }
    }
}
