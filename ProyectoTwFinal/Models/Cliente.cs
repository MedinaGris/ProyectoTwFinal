

/*
  Autor: Medina Avendaño Griselda
  Fecha creación:03/02/2023
  Fecha actualización 07/02/2023
  Descripción: Models de proveedores
*/
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoTwFinal.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int Edad { get; set; }

    public String Path { get; set; } = null!;

    [NotMapped]

    public IFormFile File { get; set; } = null!;


}
