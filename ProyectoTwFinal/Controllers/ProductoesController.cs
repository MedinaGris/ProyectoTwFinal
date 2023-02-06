using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoTwFinal.Models;

namespace ProyectoTwFinal.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly PruebaContext _context;

        public ProductoesController(PruebaContext context)
        {
            _context = context;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            return _context.Productos != null ?
                          View(await _context.Productos.ToListAsync()) :
                          Problem("Entity set 'PruebaContext.Productos'  is null.");
         //   return View(await _context.Productos.ToListAsync());
        }

        // GET: Productoes/Details/5
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

        // GET: Productoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Codigo,Descripcion,Cantidad,File")] Producto producto, IFormFile File)
        {

            if (producto != null && File != null)
            {
                if (File != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\producto");
                    //si es la primera imagen, crea la capeta img/provedor/
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

        // GET: Productoes/Edit/5
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

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Productoes/Delete/5
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

        // POST: Productoes/Delete/5
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

        private bool ProductoExists(int id)
        {
          return _context.Productos.Any(e => e.Id == id);
        }
    }
}
