using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class MedicineWarning
{
    public int WarningId { get; set; }

    public int MedicineId { get; set; }

    public string? WarningContent { get; set; }

    public string? WarningLevel { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;
}
