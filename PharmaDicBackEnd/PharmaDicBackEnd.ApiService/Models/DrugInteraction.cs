using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class DrugInteraction
{
    public int InteractionId { get; set; }

    public int MedicineId1 { get; set; }

    public int MedicineId2 { get; set; }

    public string? Severity { get; set; }

    public string? Description { get; set; }

    public string? Recommendation { get; set; }

    public virtual Medicine MedicineId1Navigation { get; set; } = null!;

    public virtual Medicine MedicineId2Navigation { get; set; } = null!;
}
