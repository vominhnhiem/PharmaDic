using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class DrugLookupAppContext : DbContext
{
    public DrugLookupAppContext()
    {
    }

    public DrugLookupAppContext(DbContextOptions<DrugLookupAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<DiseaseMedicine> DiseaseMedicines { get; set; }

    public virtual DbSet<DrugInteraction> DrugInteractions { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicineCategory> MedicineCategories { get; set; }

    public virtual DbSet<MedicineIngredient> MedicineIngredients { get; set; }

    public virtual DbSet<MedicineWarning> MedicineWarnings { get; set; }

    public virtual DbSet<SearchHistory> SearchHistories { get; set; }

    public virtual DbSet<Symptom> Symptoms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.DiseaseId).HasName("PK__Diseases__69B533A9DF36CA37");

            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.DiseaseName).HasMaxLength(150);
        });

        modelBuilder.Entity<DiseaseMedicine>(entity =>
        {
            entity.HasKey(e => new { e.DiseaseId, e.MedicineId }).HasName("PK__DiseaseM__7D4721264AE26E97");

            entity.ToTable("DiseaseMedicine");

            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");

            entity.HasOne(d => d.Disease).WithMany(p => p.DiseaseMedicines)
                .HasForeignKey(d => d.DiseaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiseaseMe__Disea__4F7CD00D");

            entity.HasOne(d => d.Medicine).WithMany(p => p.DiseaseMedicines)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DiseaseMe__Medic__5070F446");
        });

        modelBuilder.Entity<DrugInteraction>(entity =>
        {
            entity.HasKey(e => e.InteractionId).HasName("PK__DrugInte__922C03766A653988");

            entity.Property(e => e.InteractionId).HasColumnName("InteractionID");
            entity.Property(e => e.MedicineId1).HasColumnName("MedicineID1");
            entity.Property(e => e.MedicineId2).HasColumnName("MedicineID2");
            entity.Property(e => e.Severity).HasMaxLength(50);

            entity.HasOne(d => d.MedicineId1Navigation).WithMany(p => p.DrugInteractionMedicineId1Navigations)
                .HasForeignKey(d => d.MedicineId1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DrugInter__Medic__534D60F1");

            entity.HasOne(d => d.MedicineId2Navigation).WithMany(p => p.DrugInteractionMedicineId2Navigations)
                .HasForeignKey(d => d.MedicineId2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DrugInter__Medic__5441852A");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__CE74FAF5AF258C9F");

            entity.Property(e => e.FavoriteId).HasColumnName("FavoriteID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Medicine).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorites__Medic__5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorites__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB27A0F39FA2B");

            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.IngredientName).HasMaxLength(150);
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__4F2128F0486E5BDD");

            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DosageForm).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(150);
            entity.Property(e => e.MedicineName).HasMaxLength(150);
            entity.Property(e => e.Strength).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Medicines__Categ__3F466844");
        });

        modelBuilder.Entity<MedicineCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Medicine__19093A2B366D8A04");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<MedicineIngredient>(entity =>
        {
            entity.HasKey(e => new { e.MedicineId, e.IngredientId }).HasName("PK__Medicine__F4CBC3D7D7AA2604");

            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Amount).HasMaxLength(100);

            entity.HasOne(d => d.Ingredient).WithMany(p => p.MedicineIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicineI__Ingre__44FF419A");

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicineIngredients)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicineI__Medic__440B1D61");
        });

        modelBuilder.Entity<MedicineWarning>(entity =>
        {
            entity.HasKey(e => e.WarningId).HasName("PK__Medicine__214571B874D10CCC");

            entity.Property(e => e.WarningId).HasColumnName("WarningID");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.WarningLevel).HasMaxLength(50);

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicineWarnings)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicineW__Medic__5FB337D6");
        });

        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.HasKey(e => e.SearchId).HasName("PK__SearchHi__21C53514CC980D92");

            entity.ToTable("SearchHistory");

            entity.Property(e => e.SearchId).HasColumnName("SearchID");
            entity.Property(e => e.Keyword).HasMaxLength(255);
            entity.Property(e => e.SearchDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SearchType).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SearchHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SearchHis__UserI__5812160E");
        });

        modelBuilder.Entity<Symptom>(entity =>
        {
            entity.HasKey(e => e.SymptomId).HasName("PK__Symptoms__D26ED8B6AE45016A");

            entity.Property(e => e.SymptomId).HasColumnName("SymptomID");
            entity.Property(e => e.SymptomName).HasMaxLength(150);

            entity.HasMany(d => d.Diseases).WithMany(p => p.Symptoms)
                .UsingEntity<Dictionary<string, object>>(
                    "SymptomDisease",
                    r => r.HasOne<Disease>().WithMany()
                        .HasForeignKey("DiseaseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SymptomDi__Disea__4CA06362"),
                    l => l.HasOne<Symptom>().WithMany()
                        .HasForeignKey("SymptomId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SymptomDi__Sympt__4BAC3F29"),
                    j =>
                    {
                        j.HasKey("SymptomId", "DiseaseId").HasName("PK__SymptomD__54F58B8CCE6612A6");
                        j.ToTable("SymptomDisease");
                        j.IndexerProperty<int>("SymptomId").HasColumnName("SymptomID");
                        j.IndexerProperty<int>("DiseaseId").HasColumnName("DiseaseID");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC37887F71");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105347A0EE942").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("Dược sĩ");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
