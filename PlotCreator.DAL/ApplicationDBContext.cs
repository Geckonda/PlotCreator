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


            modelBuilder.Entity<Access_Modificator>().HasData(
              new Access_Modificator { Id = 1, Name = "Публично" },
              new Access_Modificator { Id = 2, Name = "Приватно" }
            );


            modelBuilder.Entity<Book_Status>().HasData(
              new Book_Status { Id = 1, Name = "В процессе" },
              new Book_Status { Id = 2, Name = "Завершен" },
              new Book_Status { Id = 3, Name = "Заморожен" },
              new Book_Status { Id = 4, Name = "Заброшен" }
            );


            modelBuilder.Entity<Genre>().HasData(
              new Genre { Id = 1, Name = "Мистика" },
              new Genre { Id = 2, Name = "Драма" },
              new Genre  { Id = 3, Name = "Приключения" },
              new Genre { Id = 4, Name = "Ужасы" },
              new Genre { Id = 5, Name = "Романтика" }
            );

            modelBuilder.Entity<Rating>().HasData(
             new Rating { Id = 1, Name = "0+" },
             new Rating { Id = 2, Name = "6+" },
             new Rating { Id = 3, Name = "12+" },
             new Rating { Id = 4, Name = "16+" },
             new Rating { Id = 5, Name = "18+" }
           );

            modelBuilder.Entity<Role>().HasData(
             new Role { Id = 1, Name = "Admin" },
             new Role { Id = 2, Name = "Moderator" },
             new Role { Id = 3, Name = "User" }
           );
            modelBuilder.Entity<Worldview>().HasData(
             new Worldview { Id = 1, Name = "Законопослушно-доброе", Description = "персонажи с таким мировоззрением считаются с правилами и совершают поступки, которых от них ожидает общество" },
             new Worldview { Id = 2, Name = "Нейтрально-доброе", Description = "персонажи с таким мировоззрением совершают хорошие поступки в соответствии со своими потребностями" },
             new Worldview { Id = 3, Name = "Хаотично-доброе", Description = "персонажи с таким мировоззрением действуют по совести, с небольшой оглядкой на мнение и ожидания других" },
             new Worldview { Id = 4, Name = "Законопослушно-нейтральное", Description = "персонажи с таким мировоззрением действуют в соответствии с законом, традицией, или своим кодексом" },
             new Worldview { Id = 5, Name = "Хаотично-нейтральное", Description = "персонажи с таким мировоззрением предпочитают чистоту от моральных вопросов в своих действиях, и не принимают какой бы то ни было стороны, даже если одна из них более выгодна в данное время" },
             new Worldview { Id = 6, Name = "Нейтральное", Description = "персонажи с таким мировоззрением следуют своим целям или капризам, вознося свою цель/свободу выше всего остального" },
             new Worldview { Id = 7, Name = "Законопослушно-злое", Description = "персонажи с таким мировоззрением постоянно берут то, что хотят, в рамках традиции, лояльности или порядка" },
             new Worldview { Id = 8, Name = "Нейтрально-злое", Description = "персонажи с таким мировоззрением делают что угодно и когда угодно, без оглядки на сострадание и сомнение" },
             new Worldview { Id = 9, Name = "Хаотично-злое", Description = "персонажи с таким мировоззрением совершают неконтролируемое насилие, стимулируемое их жадностью, ненавистью или жаждой крови" }
           );
        }
	}
}
