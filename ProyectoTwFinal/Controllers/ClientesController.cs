
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTwFinal.Models;

namespace ProyectoTwFinal.Controllers
{
    public class ClientesController : Controller
    {
        private readonly PruebaContext _context;

        public ClientesController(PruebaContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return _context.Clientes != null ?
                         View(await _context.Clientes.ToListAsync()) :
                         Problem("Entity set 'PruebaContext.Clientes'  is null.");
           
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Correo,Edad, File")] Cliente cliente, IFormFile File)
        {
            /*if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);*/
            if (cliente != null && File != null)
            {
                if (File != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\proveedor");
                    //si es la primera imagen, crea la capeta img/provedor/
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = cliente.Nombre + Path.GetFileName(File.FileName);
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        //copia la imagen a su nueva direccion en wwwrot/img
                        File.CopyTo(stream);
                        //copia solo el nombre del archivo
                        cliente.Path = fileName;
                    }
                }
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Path,Nombre,Direccion,Correo,Edad")] Cliente cliente, IFormFile? FileImg)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }
            /*

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);*/
            // SI SE AGREGÓ NUEVA IMAGEN, SE BORRA LA ANTERIOR Y SE GUARDA LA NUEVA
            if (FileImg != null)
            {
                //direccion donde se guardará la imagen
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\cliente");
                //direccion de la imagen a eliminar
                string oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\cliente", cliente.Path);
                //se borra la imagen anterior
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);
                //crea la direccion de la imagen tomando en cuenta el nombre del usuario para no chocar nombre
                string fileName = cliente.Nombre + Path.GetFileName(FileImg.FileName);
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    //copia la imagen del imput hacia la nueva direccion
                    FileImg.CopyTo(stream);
                    cliente.Path = fileName;
                }
            }
            if (cliente != null)//Valida que el modleo no se encunetra vacio, las otras validaciones se hacen antes de entrar al metodo
            {
                try
                {
                    //actualiza la informacion de proveedor
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);


        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'PruebaContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
