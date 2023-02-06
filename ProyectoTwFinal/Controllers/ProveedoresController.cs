
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTwFinal.Models;

namespace ProyectoTwFinal.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly PruebaContext _context;

        public ProveedoresController(PruebaContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            return _context.Proveedores != null ?
                         View(await _context.Proveedores.ToListAsync()) :
                         Problem("Entity set 'PruebaContext.Proveedores'  is null.");
          //  return View(await _context.Proveedores.ToListAsync());
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Correo,Edad,File")] Proveedore proveedore,IFormFile File)
        {
            if (proveedore!=null && File!=null)
            {
                if (File != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\proveedor");
                    //si es la primera imagen, crea la capeta img/provedor/
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = proveedore.Nombre+Path.GetFileName(File.FileName);
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        //copia la imagen a su nueva direccion en wwwrot/img
                        File.CopyTo(stream);
                        //copia solo el nombre del archivo
                        proveedore.Avatar = fileName;
                    }
                }
                _context.Add(proveedore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedore);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }
            return View(proveedore);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Avatar,Nombre,Direccion,Correo,Edad")] Proveedore proveedore,IFormFile? FileImg)
        {
            if (id != proveedore.Id)
            {
                return NotFound();
            }
            // SI SE AGREGÓ NUEVA IMAGEN, SE BORRA LA ANTERIOR Y SE GUARDA LA NUEVA
            if (FileImg != null)
            {
                //direccion donde se guardará la imagen
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\proveedor");
                //direccion de la imagen a eliminar
                string oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\proveedor", proveedore.Avatar);
                //se borra la imagen anterior
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);
                //crea la direccion de la imagen tomando en cuenta el nombre del usuario para no chocar nombre
                string fileName = proveedore.Nombre + Path.GetFileName(FileImg.FileName);
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    //copia la imagen del imput hacia la nueva direccion
                    FileImg.CopyTo(stream);
                    proveedore.Avatar = fileName;
                }
            }
            if (proveedore !=null)//Valida que el modleo no se encunetra vacio, las otras validaciones se hacen antes de entrar al metodo
            {
                try
                {
                    //actualiza la informacion de proveedor
                    _context.Update(proveedore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedoreExists(proveedore.Id))
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
            
            return View(proveedore);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedores == null)
            {
                return Problem("Entity set 'PruebaContext.Proveedores'  is null.");
            }
            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore != null)
            {
                _context.Proveedores.Remove(proveedore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedoreExists(int id)
        {
          return _context.Proveedores.Any(e => e.Id == id);
        }
    }
}
