using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class Disease
{
    public int DiseaseId { get; set; }

    public string DiseaseName { get; set; } = null!;

    public string? Description { get; set; }

    public string? WarningSigns { get; set; }

    public virtual ICollection<DiseaseMedicine> DiseaseMedicines { get; set; } = new List<DiseaseMedicine>();

    public virtual ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
}
