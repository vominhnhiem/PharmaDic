using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class Medicine
{
    public int MedicineId { get; set; }

    public string MedicineName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? DosageForm { get; set; }

    public string? Strength { get; set; }

    public string? Manufacturer { get; set; }

    public string? Country { get; set; }

    public string? Uses { get; set; }

    public string? Dosage { get; set; }

    public string? Contraindications { get; set; }

    public string? SideEffects { get; set; }

    public string? Storage { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual MedicineCategory? Category { get; set; }

    public virtual ICollection<DiseaseMedicine> DiseaseMedicines { get; set; } = new List<DiseaseMedicine>();

    public virtual ICollection<DrugInteraction> DrugInteractionMedicineId1Navigations { get; set; } = new List<DrugInteraction>();

    public virtual ICollection<DrugInteraction> DrugInteractionMedicineId2Navigations { get; set; } = new List<DrugInteraction>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<MedicineIngredient> MedicineIngredients { get; set; } = new List<MedicineIngredient>();

    public virtual ICollection<MedicineWarning> MedicineWarnings { get; set; } = new List<MedicineWarning>();


}
