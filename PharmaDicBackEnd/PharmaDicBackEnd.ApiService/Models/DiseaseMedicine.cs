using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class DiseaseMedicine
{
    public int DiseaseId { get; set; }

    public int MedicineId { get; set; }

    public string? Note { get; set; }

    public virtual Disease Disease { get; set; } = null!;

    public virtual Medicine Medicine { get; set; } = null!;
}
