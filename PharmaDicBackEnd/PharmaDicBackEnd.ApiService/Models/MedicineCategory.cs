using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class MedicineCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
