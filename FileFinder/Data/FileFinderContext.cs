using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileFinder.Models;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.DependencyInjection;

namespace FileFinder.Data
{
    public class FileFinderContext : DbContext
    {
        public FileFinderContext(DbContextOptions<FileFinderContext> options) : base(options)
        {
            
        }

        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<CaseManager> CaseManagers { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Models.Program> Programs { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<FileMember> FileMembers { get; set; }



//SEARCH METHODS

        public List<Consumer> SearchConsumers(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Consumers.OrderBy(c => c.FullName()).ToList();
            }

            var results = from c in Consumers where c.FullName().Contains(value) || c.FullName().ToLower().Contains(value.ToLower()) select c;

            if (results.ToList().Count() == 0)
            {
                return null;
            }

            return results.OrderBy(c => c.FullName()).ToList();
        }

        public List<CaseManager> SearchCaseManagers(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return CaseManagers.OrderBy(c => c.FullName()).ToList();
            }

            var results = from c in CaseManagers where c.FullName().Contains(value) || c.FullName().ToLower().Contains(value.ToLower()) select c;

            if (results.ToList().Count() == 0)
            {
                return null;
            }
            return results.OrderBy(c => c.FullName()).ToList();
        }

        public List<Models.Program> SearchPrograms(string value)
        {
            if(String.IsNullOrEmpty(value))
            {
                return Programs.OrderBy(p => p.Name).ToList();
            }

            var results = from p in Programs where p.Name.Contains(value) || p.Name.ToLower().Contains(value.ToLower()) select p;

            if (results.ToList().Count() == 0)
            {
                return null;
            }
            return results.OrderBy(p => p.Name).ToList();
        }


        public Consumer FindByID(int id)
        {
            var results = from c in Consumers
                          where c.ID == id
                          select c;

            return results.Single();
        }

        // Before creating a new file, this will check if the same Consumer/CaseManager/Room combo already exists.
        public bool FileExists(Models.File file)
        {
            var results = from f in Files
                          where f.Equals(file)
                          select f;

            if (results.Count() == 0)
            {
                return false;
            }

            return true;
        }

    }
}


