using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class Symptom
{
    public int SymptomId { get; set; }

    public string SymptomName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Disease> Diseases { get; set; } = new List<Disease>();
}
