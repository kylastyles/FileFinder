using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileFinder.Models;

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
        public DbSet<File> Files { get; set; }
        public DbSet<Room> Rooms { get; set; }

        // TODO: Improve search. "min" searched in CaseManagers returned 0, but SHOULD return McGonagall.

        public List<Consumer> FindByValue(string value)
        {
            var results = from c in Consumers
                          where c.FullName().Contains(value)
                          || c.FullName().ToLower().Contains(value.ToLower())
                          select c;
            return results.OrderBy(c => c.FullName()).ToList();
        }

        public Consumer FindByID(int id)
        {
            var results = from c in Consumers
                          where c.ID == id
                          select c;

            return results.Single();
        }

        // Before creating a new file, this will check if the same Consumer/CaseManager/Room combo already exists.
        public bool FileExists(File file)
        {
            var results = from f in Files
                          where f.Equals(file)
                          select f;

            if (results == null)
            {
                return false;
            }

            return true;
        }
    }
}
