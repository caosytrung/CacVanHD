using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyRestaurant.Models
{
    public partial class MyRestaurantContext : DbContext
    {
        public virtual DbSet<BookTable> BookTable { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ConfigDefault> ConfigDefault { get; set; }
        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<MaterialBill> MaterialBill { get; set; }
        public virtual DbSet<MaterialBillDetail> MaterialBillDetail { get; set; }
        public virtual DbSet<MaterialInStock> MaterialInStock { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Rtable> Rtable { get; set; }
        public virtual DbSet<Ruser> Ruser { get; set; }
        public virtual DbSet<SaleBill> SaleBill { get; set; }
        public virtual DbSet<SaleBillDetail> SaleBillDetail { get; set; }
        public virtual DbSet<TimeKeeping> TimeKeeping { get; set; }
        public virtual DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyRestaurant;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookTable>(entity =>
            {
                entity.Property(e => e.BookAt).HasColumnType("datetime");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.BookTable)
                    .HasForeignKey(d => d.TableId)
                    .HasConstraintName("FK_BookTable_RTable");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatAt)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ConfigDefault>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Value).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Dish)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Dish_Category");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Raddress)
                    .IsRequired()
                    .HasColumnName("RAddress")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Rname)
                    .IsRequired()
                    .HasColumnName("RName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salary).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.StartWorkedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employ_Position");
            });

            modelBuilder.Entity<MaterialBill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Rtype)
                    .HasColumnName("RType")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.EmployeeRespondNavigation)
                    .WithMany(p => p.MaterialBill)
                    .HasForeignKey(d => d.EmployeeRespond)
                    .HasConstraintName("FK_Material_Employ");
            });

            modelBuilder.Entity<MaterialBillDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.MaterialBill)
                    .WithMany(p => p.MaterialBillDetail)
                    .HasForeignKey(d => d.MaterialBillId)
                    .HasConstraintName("FK_MarBillDetail_MarInBill");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.MaterialBillDetail)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK_MarBillDetail_MarInStock");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.MaterialBillDetail)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarBillDetail_UOM");
            });

            modelBuilder.Entity<MaterialInStock>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Quatity).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Rdescription)
                    .HasColumnName("RDescription")
                    .HasColumnType("text");

                entity.Property(e => e.Rname)
                    .IsRequired()
                    .HasColumnName("RName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.MaterialInStock)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .HasConstraintName("FK_MarInStock_UOM");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateAt)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Rname)
                    .IsRequired()
                    .HasColumnName("RName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkDesc).HasColumnType("text");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rtable>(entity =>
            {
                entity.ToTable("RTable");

                entity.Property(e => e.LocationTable)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Thumbnail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TypeTable)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ruser>(entity =>
            {
                entity.ToTable("RUser");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateAt)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Emai)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rpassword)
                    .IsRequired()
                    .HasColumnName("RPassword")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Rusername)
                    .IsRequired()
                    .HasColumnName("RUsername")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Ruser)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<SaleBill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatAt)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.SaleBill)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_SaleBill_Employee");
            });

            modelBuilder.Entity<SaleBillDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.SaleBillDetail)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK_SaleDetail_Dish");

                entity.HasOne(d => d.SaleBill)
                    .WithMany(p => p.SaleBillDetail)
                    .HasForeignKey(d => d.SaleBillId)
                    .HasConstraintName("FK_SaleDetail_SaleBill");
            });

            modelBuilder.Entity<TimeKeeping>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TimeKeeping)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_TimeK_Employ");
            });

            modelBuilder.Entity<UnitOfMeasure>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Rname)
                    .IsRequired()
                    .HasColumnName("RName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
