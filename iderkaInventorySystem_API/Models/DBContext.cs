using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetailOrder> DetailOrders { get; set; }

    public virtual DbSet<DetailTransfer> DetailTransfers { get; set; }

    public virtual DbSet<LogisticChief> LogisticChiefs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SparePart> SpareParts { get; set; }

    public virtual DbSet<SparePartStock> SparePartStocks { get; set; }

    public virtual DbSet<StorageLocation> StorageLocations { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source= FAB-PC\\SQLEXPRESS01;Initial Catalog= iderkaInven;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetailOrder>(entity =>
        {
            entity.HasKey(e => e.IdDetOrd).HasName("PK_DetOrder");

            entity.ToTable("DetailOrder");

            entity.Property(e => e.IdDetOrd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idDetOrd");
            entity.Property(e => e.IdOrd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idOrd");
            entity.Property(e => e.IdSpare)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idSpare");

            entity.HasOne(d => d.IdOrdNavigation).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.IdOrd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order");

            entity.HasOne(d => d.IdSpareNavigation).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.IdSpare)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product");
        });

        modelBuilder.Entity<DetailTransfer>(entity =>
        {
            entity.HasKey(e => e.IdDetTransf).HasName("PK_DetTransfers");

            entity.Property(e => e.IdDetTransf)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idDetTransf");
            entity.Property(e => e.IdSpare)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idSpare");
            entity.Property(e => e.IdTransf)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idTransf");

            entity.HasOne(d => d.IdSpareNavigation).WithMany(p => p.DetailTransfers)
                .HasForeignKey(d => d.IdSpare)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductDetTransf");

            entity.HasOne(d => d.IdTransfNavigation).WithMany(p => p.DetailTransfers)
                .HasForeignKey(d => d.IdTransf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers");
        });

        modelBuilder.Entity<LogisticChief>(entity =>
        {
            entity.HasKey(e => e.IdAdm);

            entity.ToTable("LogisticChief");

            entity.Property(e => e.IdAdm)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idAdm");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Pwd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pwd");
            entity.Property(e => e.Usr)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usr");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrd).HasName("PK_Order");

            entity.Property(e => e.IdOrd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idOrd");
            entity.Property(e => e.DateOrd)
                .HasColumnType("datetime")
                .HasColumnName("dateOrd");
            entity.Property(e => e.DescOrd)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descOrd");
            entity.Property(e => e.IdLoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idLoc");
            entity.Property(e => e.StatusOrd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("statusOrd");
            entity.Property(e => e.WorkOrd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("workOrd");

            entity.HasOne(d => d.IdLocNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdLoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Location_Order");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.IdReg);

            entity.ToTable("Region");

            entity.Property(e => e.IdReg)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idReg");
            entity.Property(e => e.DescReg)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("descReg");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK_Rol");

            entity.Property(e => e.IdRol)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idRol");
            entity.Property(e => e.RolName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rolName");
        });

        modelBuilder.Entity<SparePart>(entity =>
        {
            entity.HasKey(e => e.IdSpare).HasName("PK_Product");

            entity.ToTable("SparePart");

            entity.Property(e => e.IdSpare)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idSpare");
            entity.Property(e => e.DescPart)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descPart");
            entity.Property(e => e.NumberPart)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numberPart");
            entity.Property(e => e.Rework).HasColumnName("rework");
        });

        modelBuilder.Entity<SparePartStock>(entity =>
        {
            entity.HasKey(e => new { e.IdSpare, e.IdLoc }).HasName("PK_ProdStk");

            entity.ToTable("SparePartStock");

            entity.Property(e => e.IdSpare)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idSpare");
            entity.Property(e => e.IdLoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idLoc");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdLocNavigation).WithMany(p => p.SparePartStocks)
                .HasForeignKey(d => d.IdLoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Location_PS");

            entity.HasOne(d => d.IdSpareNavigation).WithMany(p => p.SparePartStocks)
                .HasForeignKey(d => d.IdSpare)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_PS");
        });

        modelBuilder.Entity<StorageLocation>(entity =>
        {
            entity.HasKey(e => e.IdLoc).HasName("PK_Location");

            entity.ToTable("StorageLocation");

            entity.Property(e => e.IdLoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idLoc");
            entity.Property(e => e.DescStLoc)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descStLoc");
            entity.Property(e => e.IdReg)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idReg");
            entity.Property(e => e.NameSt)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nameSt");

            entity.HasOne(d => d.IdRegNavigation).WithMany(p => p.StorageLocations)
                .HasForeignKey(d => d.IdReg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Region");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.IdTransf);

            entity.Property(e => e.IdTransf)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idTransf");
            entity.Property(e => e.ArrivalDate)
                .HasColumnType("datetime")
                .HasColumnName("arrivalDate");
            entity.Property(e => e.DateTransf)
                .HasColumnType("datetime")
                .HasColumnName("dateTransf");
            entity.Property(e => e.DestinyId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("destinyId");
            entity.Property(e => e.OriginId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("originId");
            entity.Property(e => e.StatusTransf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("statusTransf");

            entity.HasOne(d => d.Destiny).WithMany(p => p.TransferDestinies)
                .HasForeignKey(d => d.DestinyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Destiny");

            entity.HasOne(d => d.Origin).WithMany(p => p.TransferOrigins)
                .HasForeignKey(d => d.OriginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Origin");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsr).HasName("PK_User");

            entity.Property(e => e.IdUsr)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idUsr");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdLoc)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("idLoc");
            entity.Property(e => e.Pwd)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("pwd");
            entity.Property(e => e.Usr)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usr");

            entity.HasOne(d => d.IdLocNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdLoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Location");

            entity.HasMany(d => d.IdRols).WithMany(p => p.IdUsrs)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_IDrol"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("IdUsr")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_IDusr"),
                    j =>
                    {
                        j.HasKey("IdUsr", "IdRol").HasName("PK_UsrRol");
                        j.ToTable("UserRoles");
                        j.IndexerProperty<string>("IdUsr")
                            .HasMaxLength(5)
                            .IsUnicode(false)
                            .HasColumnName("idUsr");
                        j.IndexerProperty<string>("IdRol")
                            .HasMaxLength(5)
                            .IsUnicode(false)
                            .HasColumnName("idRol");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
