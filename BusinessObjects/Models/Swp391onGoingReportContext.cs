using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class Swp391onGoingReportContext : DbContext
{
    public Swp391onGoingReportContext()
    {
    }

    public Swp391onGoingReportContext(DbContextOptions<Swp391onGoingReportContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local); Database=SWP391OnGoingReport; Uid=sa; Pwd=12345678;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("courseID");
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .HasColumnName("courseName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.TimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("timeCreated");
            entity.Property(e => e.UserId).HasColumnName("userID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("fullName");
            entity.Property(e => e.IsBan).HasColumnName("isBan");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.ToTable("UserCourse");

            entity.Property(e => e.UserCourseId)
                .ValueGeneratedNever()
                .HasColumnName("userCourseID");
            entity.Property(e => e.CourseId).HasColumnName("courseID");
            entity.Property(e => e.UserId).HasColumnName("userID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
