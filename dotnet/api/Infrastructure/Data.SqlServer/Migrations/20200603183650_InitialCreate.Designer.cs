// <auto-generated />
using System;
using AndcultureCode.GB.Infrastructure.Data.SqlServer;
using AndcultureCode.GB.Infrastructure.Data.SqlServer.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.SqlServer.Migrations
{
    [DbContext(typeof(GBApiContext))]
    [Migration(GBFlattenedMigration.InitialMigrationId)]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Acls.Acl", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<int>("Permission");

                    b.Property<string>("Resource")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.Property<string>("Verb")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Acls");
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Roles.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsSuperAdmin");

                    b.Property<string>("LastName");

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Users.UserLogin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<int>("FailedAttemptCount");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasMaxLength(39);

                    b.Property<bool>("IsSuccessful");

                    b.Property<DateTimeOffset?>("KeepAliveOn");

                    b.Property<long?>("RoleId");

                    b.Property<string>("ServerName")
                        .HasMaxLength(150);

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.Property<string>("UserAgent");

                    b.Property<long?>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Users.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<long>("RoleId");

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Jobs.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BackgroundJobId")
                        .HasMaxLength(1000);

                    b.Property<long?>("CreatedById");

                    b.Property<DateTimeOffset?>("CreatedOn");

                    b.Property<string>("DebugJson");

                    b.Property<long?>("DeletedById");

                    b.Property<DateTimeOffset?>("DeletedOn");

                    b.Property<DateTimeOffset?>("EndedOn");

                    b.Property<string>("Error");

                    b.Property<long?>("StartedById");

                    b.Property<DateTimeOffset?>("StartedOn");

                    b.Property<int?>("Status");

                    b.Property<long?>("UpdatedById");

                    b.Property<DateTimeOffset?>("UpdatedOn");

                    b.Property<string>("WorkerArgs")
                        .HasMaxLength(1000);

                    b.Property<string>("WorkerName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Users.UserLogin", b =>
                {
                    b.HasOne("AndcultureCode.GB.Business.Core.Models.Entities.Roles.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AndcultureCode.GB.Business.Core.Models.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AndcultureCode.GB.Business.Core.Models.Entities.Users.UserRole", b =>
                {
                    b.HasOne("AndcultureCode.GB.Business.Core.Models.Entities.Roles.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AndcultureCode.GB.Business.Core.Models.Entities.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
