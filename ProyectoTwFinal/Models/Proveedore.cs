using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoTwFinal.Models;

public partial class Proveedore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Avatar { get; set; } = null!;
   
    public int Edad { get; set; }

    [NotMapped]
    
    public IFormFile File { get; set; } = null!;
    
}
