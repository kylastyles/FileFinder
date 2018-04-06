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
        public DbSet<CProgram> CPrograms { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Room> Rooms { get; set; }

    }
}
