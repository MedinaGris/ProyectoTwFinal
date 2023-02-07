using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTwFinal.Models;

/*
  Autores:Vicente Leonel Vásquez Hernández
  fecha creación:04/02/2023
  fecha actualización: 06/02/2023
  Descripción: Controller de productos
*/

namespace ProyectoTwFinal.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly PruebaContext _context;

        public ProductoesController(PruebaContext context)
        {
            _context = context;
        }

        // Método qué genera el index
        public async Task<IActionResult> Index()
        {
            return _context.Productos != null ?
                          View(await _context.Productos.ToListAsync()) :
                          Problem("Entity set 'PruebaContext.Productos'  is null.");
        
        }

        // Método qué genera el apartado detalles
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // Método qué genera el create
        public IActionResult Create()
        {
            return View();
        }

        // Método qué se encarga del create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Codigo,Descripcion,Cantidad,File")] Producto producto, IFormFile File)
        {

            if (producto != null && File != null)
            {
                if (File != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\producto");
                    //si es la primera imagen, crea la capeta img/producto/
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = producto.Nombre + Path.GetFileName(File.FileName);
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        //copia la imagen a su nueva direccion en wwwrot/img
                        File.CopyTo(stream);
                        //copia solo el nombre del archivo
                        producto.Avatar = fileName;
                    }
                }
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);

           
        }

        // Método qué genera el apartado Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // Método qué se encarga del Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Codigo,Descripcion,Cantidad,Avatar")] Producto producto, IFormFile? FileImg)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }
            // SI SE AGREGÓ NUEVA IMAGEN, SE BORRA LA ANTERIOR Y SE GUARDA LA NUEVA
            if (FileImg != null)
            {
                //direccion donde se guardará la imagen
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\producto");
                //direccion de la imagen a eliminar
                string oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\producto", producto.Avatar);
                //se borra la imagen anterior
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);
                //crea la direccion de la imagen tomando en cuenta el nombre del usuario para no chocar nombre
                string fileName = producto.Nombre + Path.GetFileName(FileImg.FileName);
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    //copia la imagen del imput hacia la nueva direccion
                    FileImg.CopyTo(stream);
                    producto.Avatar = fileName;
                }
            }
            if (producto != null)//Valida que el modleo no se encunetra vacio, las otras validaciones se hacen antes de entrar al metodo
            {
                try
                {
                    //actualiza la informacion de proveedor
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //si todo sale bien regresa a la lista de provedores
                return RedirectToAction(nameof(Index));
            }

           
            return View(producto);
        }

        // Método qué genera el apartado Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // Método qué se encarga del delate
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'PruebaContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Método qué verifica la existencia del proyecto
        private bool ProductoExists(int id)
        {
          return _context.Productos.Any(e => e.Id == id);
        }
    }
}
