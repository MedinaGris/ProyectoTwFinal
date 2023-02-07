using System.ComponentModel.DataAnnotations.Schema;

/*
  Autores:Vicente Leonel Vásquez Hernández
  fecha creación:04/02/2023
  fecha actualización: 06/02/2023
  Descripción: Model de productos
*/

namespace ProyectoTwFinal.Models;

/* Declaración de los get y set de los datos*/
public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int Cantidad { get; set; }

    public string Avatar { get; set; } = null!;

    [NotMapped]
    public IFormFile File { get; set; } = null!;

}
