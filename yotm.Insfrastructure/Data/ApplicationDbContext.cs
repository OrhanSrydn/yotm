using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yotm.Core.Entities;

namespace yotm.Insfrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseApplication> CoursesApplication { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student Configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(150);

                entity.Property(e => e.StudentNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Department)
                    .HasMaxLength(100);
            });

            // Course Configuration
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Department)
                    .HasMaxLength(100);

                entity.Property(e => e.Faculty)
                    .HasMaxLength(100);

                entity.Property(e => e.Instructor)
                    .HasMaxLength(150);
            });

            // CourseApplication Configuration
            modelBuilder.Entity<CourseApplication>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Foreign Keys
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Applications)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Applications)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Bir öğrenci aynı derse sadece 1 kez başvurabilir
                entity.HasIndex(e => new { e.StudentId, e.CourseId })
                    .IsUnique();

                entity.Property(e => e.ProcessedBy)
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .HasMaxLength(500);
            });

            // OtpCode Configuration
            modelBuilder.Entity<OtpCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.HasIndex(e => new { e.PhoneNumber, e.Code, e.IsUsed });
            });

            // AdminUser Configuration
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(e => e.UserName)
                    .IsUnique();

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            // Seed Data - Admin User (Username: admin, Password: admin123)
            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    Id = 1,
                    UserName = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    FullName = "Sistem Yöneticisi",
                    CreatedAt = DateTime.Now
                }
            );

            modelBuilder.Entity<Course>().HasData(
                // Bilgisayar Mühendisliği - 10 Ders
                new Course
                {
                    Id = 1,
                    Code = "CEN1003",
                    Name = "Computer Programming I",
                    Quota = 30,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Ahmet Yılmaz",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 2,
                    Code = "CEN2001",
                    Name = "Data Structures",
                    Quota = 35,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Doç. Dr. Ayşe Demir",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 3,
                    Code = "CEN2005",
                    Name = "Object Oriented Programming",
                    Quota = 30,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Dr. Öğr. Üyesi Mehmet Kaya",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 4,
                    Code = "CEN2002",
                    Name = "Analysis of Algorithms",
                    Quota = 25,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Zeynep Akar",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 5,
                    Code = "CEN3001",
                    Name = "Advanced Computer Programming",
                    Quota = 28,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Doç. Dr. Can Öztürk",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 6,
                    Code = "CEN3007",
                    Name = "Database Systems I",
                    Quota = 32,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Dr. Öğr. Üyesi Elif Şahin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 7,
                    Code = "CEN3002",
                    Name = "Software Engineering",
                    Quota = 30,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Ali Yıldırım",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 8,
                    Code = "CEN3004",
                    Name = "Operating Systems",
                    Quota = 30,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Doç. Dr. Fatma Arslan",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 9,
                    Code = "CEN3006",
                    Name = "Computer Networks",
                    Quota = 28,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Dr. Öğr. Üyesi Burak Tekin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 10,
                    Code = "MTH3004",
                    Name = "Numerical Analysis",
                    Quota = 25,
                    Department = "Bilgisayar Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Selin Yıldız",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },

                // Yazılım Mühendisliği - 10 Ders
                new Course
                {
                    Id = 11,
                    Code = "SEN1001",
                    Name = "Introduction to Software Engineering",
                    Quota = 35,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Emre Çelik",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 12,
                    Code = "SEN2001",
                    Name = "Object Oriented Programming I",
                    Quota = 30,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Doç. Dr. Deniz Koç",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 13,
                    Code = "SEN2002",
                    Name = "Database Design and Management",
                    Quota = 32,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Dr. Öğr. Üyesi Ceren Aydın",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 14,
                    Code = "SEN3001",
                    Name = "Visual Programming",
                    Quota = 28,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Doç. Dr. Kerem Polat",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 15,
                    Code = "SEN3003",
                    Name = "Software Design and Analysis",
                    Quota = 25,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Seda Güneş",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 16,
                    Code = "SEN3002",
                    Name = "Internet & Web Programming",
                    Quota = 35,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Dr. Öğr. Üyesi Cem Karaca",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 17,
                    Code = "SEN3004",
                    Name = "Software Architecture",
                    Quota = 25,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Leyla Toprak",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 18,
                    Code = "SEN4001",
                    Name = "Software Testing",
                    Quota = 30,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Doç. Dr. Mert Erdoğan",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 19,
                    Code = "SEN4010",
                    Name = "Artificial Intelligence",
                    Quota = 28,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Prof. Dr. Tuğba Akman",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    Id = 20,
                    Code = "SEN4002",
                    Name = "Software Verification and Validation",
                    Quota = 22,
                    Department = "Yazılım Mühendisliği",
                    Faculty = "Mühendislik Fakültesi",
                    Instructor = "Dr. Öğr. Üyesi Berk Yılmaz",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}
