using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using WebFrontUsuario.Models;
using WebFrontUsuario.ServiceUsuariosR;

namespace WebFrontUsuario.Controllers
{
    public class UsuariosController : Controller
    {
        private static List<UsuarioViewModel> usuarioViewModel = new List<UsuarioViewModel>();

        // GET: Usuarios
        public ActionResult Usuarios()
        {
            ViewBag.SexoList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Masculino", Value = "M" },
                new SelectListItem { Text = "Femenino", Value = "F" }
            }, "Value", "Text");

            return View();
        }

        // POST: Usuarios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Usuarios(UsuarioViewModel model)
        {
            ServiceUsuarioClient client = new ServiceUsuarioClient();
            BUsuarios usuario = new BUsuarios
            {
                Nombre = model.Nombre,
                Fecha = model.Fecha,
                Sexo = string.IsNullOrEmpty(model.Sexo) ? '\0' : model.Sexo[0] // Convertir string a char
            };

            if (ModelState.IsValid)
            {
                bool isInserted = await client.InsertarUsuarioAsync(usuario);

                if (isInserted)
                {
                    model.Id = usuarioViewModel.Count + 1; // Generar un ID simple para el ejemplo
                    usuarioViewModel.Add(model);

                    return RedirectToAction("UsuariosConsulta");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo insertar el usuario en la base de datos.");
                }
            }

            ViewBag.SexoList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Masculino", Value = "M" },
                new SelectListItem { Text = "Femenino", Value = "F" }
            }, "Value", "Text");

            return View(model);
        }

        // GET: Usuarios/UsuariosConsulta
        public ActionResult UsuariosConsulta(int? page)
        {
            ServiceUsuariosR.ServiceUsuarioClient client = new ServiceUsuariosR.ServiceUsuarioClient();
            var usuarios = client.ConsultarUsuarios(); // Obtener todos los usuarios desde el servicio

            // Mapear los usuarios a ViewModel si es necesario
            var viewModelUsuarios = usuarios.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Fecha = u.Fecha,
                Sexo = u.Sexo.ToString()
            }).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(viewModelUsuarios.ToPagedList(pageNumber, pageSize));
        }

        // POST: Usuarios/EliminarUsuario
        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            try
            {
                var usuario = new BUsuarios { Id = id };

                // Llamar al método de servicio para eliminar usuario
                ServiceUsuarioClient client = new ServiceUsuarioClient();
                string resultMessage = client.EliminarUsuario(usuario);

                if (resultMessage.Contains("Error"))
                {
                    return Json(new { success = false, message = resultMessage });
                }

                return Json(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return Json(new { success = false, message = $"Error al intentar eliminar el usuario: {ex.Message}" });
            }
        }

        // GET: Usuarios/EditarUsuario/5
        public ActionResult EditarUsuario(int id)
        {
            try
            {
                ServiceUsuarioClient client = new ServiceUsuarioClient();
                var usuario = client.ObtenerUsuario(id); // Obtener el usuario del servicio

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var model = new UsuarioViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Fecha = usuario.Fecha,
                    Sexo = usuario.Sexo.ToString() // Asumiendo que usuario.Sexo es de tipo char
                };

                ViewBag.SexoList = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "Masculino", Value = "M" },
                    new SelectListItem { Text = "Femenino", Value = "F" }
                }, "Value", "Text");

                return PartialView("_EditarUsuario", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al cargar los datos del usuario: {ex.Message}");
                // Aquí podrías redireccionar o manejar el error según tu lógica de la aplicación
                return HttpNotFound();
            }
        }

        // POST: Usuarios/EditarUsuario/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarUsuario(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BUsuarios usuario = new BUsuarios
                    {
                        Id = model.Id,
                        Nombre = model.Nombre,
                        Fecha = model.Fecha, // Asignar directamente, ya que model.Fecha es un DateTime
                        Sexo = string.IsNullOrEmpty(model.Sexo) ? '\0' : model.Sexo[0] // Convertir string a char
                    };

                    ServiceUsuarioClient client = new ServiceUsuarioClient();
                    string resultado = await client.EditarUsuarioAsync(usuario);

                    return Json(new { success = true, message = resultado });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error al editar usuario: {ex.Message}" });
                }
            }
            else
            {
                // Si el modelo no es válido, devolver los errores de validación
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }
        }

        // GET: Usuarios/ObtenerUsuario/5
        public ActionResult ObtenerUsuario(int id)
        {
            try
            {
                ServiceUsuarioClient client = new ServiceUsuarioClient();
                var usuario = client.ObtenerUsuario(id); // Utilizar el método ObtenerUsuario del cliente del servicio

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var model = new UsuarioViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Fecha = usuario.Fecha,
                    Sexo = usuario.Sexo.ToString()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al cargar los datos del usuario: {ex.Message}");
                return RedirectToAction("UsuariosConsulta");
            }
        }
    }
}