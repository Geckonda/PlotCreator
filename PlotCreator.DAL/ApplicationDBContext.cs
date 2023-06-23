using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Multiple_Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Access_Modificator> Modificators => Set<Access_Modificator>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Book_Status> Statuses => Set<Book_Status>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Idea> Ideas => Set<Idea>();
        public DbSet<Book_Idea> Books_Ideas => Set<Book_Idea>();
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Episode> Episodes => Set<Episode>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Book_Character> Books_Characters => Set<Book_Character>();
        public DbSet<Episode_Event> Episodes_Events => Set<Episode_Event>();
        public DbSet<Episode_Character> Episodes_Characters => Set<Episode_Character>();
        public DbSet<Event_Character> Events_Characters => Set<Event_Character>();
        public DbSet<Group_Character> Groups_Characters => Set<Group_Character>();
        public DbSet<Group_Event> Groups_Events => Set<Group_Event>();
		public DbSet<Worldview> Worldview => Set<Worldview>();

		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MigrationsTest;Trusted_Connection=True;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			//Автоматическое включение свойств
			//Книга
			modelBuilder.Entity<Book>().Navigation(book => book.User).AutoInclude();
            modelBuilder.Entity<Book>().Navigation(book => book.Modificator).AutoInclude();
            modelBuilder.Entity<Book>().Navigation(book => book.Rating).AutoInclude();
            modelBuilder.Entity<Book>().Navigation(book => book.Genre).AutoInclude();
            modelBuilder.Entity<Book>().Navigation(book => book.Status).AutoInclude();


            //modelBuilder.Entity<Book>().Navigation(book => book.Episodes).AutoInclude();
            //modelBuilder.Entity<Book>().Navigation(book => book.Events).AutoInclude();
            //modelBuilder.Entity<Book>().Navigation(book => book.Groups).AutoInclude();
            //---------------
            //Персонаж
            modelBuilder.Entity<Character>().Navigation(ch => ch.User).AutoInclude();
            modelBuilder.Entity<Character>().Navigation(ch => ch.Worldview).AutoInclude();
            //----------------
            //Идеи
            modelBuilder.Entity<Idea>().Navigation(idea => idea.User).AutoInclude();
			//----------------
			//Эпизоды
			modelBuilder.Entity<Episode>().Navigation(episode => episode.Book).AutoInclude();
			//----------------
			//События
			modelBuilder.Entity<Event>().Navigation(e => e.Book).AutoInclude();
			//----------------
			//Группы
			modelBuilder.Entity<Group>().Navigation(group => group.Book).AutoInclude();
		}
	}
}
