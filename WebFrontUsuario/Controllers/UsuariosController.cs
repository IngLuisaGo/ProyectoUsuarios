using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebFrontUsuario.Models;
using System.Collections.Generic;
using ServicioUsuarios;
using WebFrontUsuario.ServiceUsuariosR;
using PagedList;
using System;

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
        public ActionResult Usuarios(UsuarioViewModel model)
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
                bool isInserted = client.InsertarUsuario(usuario);

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
                Sexo = u.Sexo.ToString() // Asegúrate de convertir el sexo según corresponda
            }).ToList();

            // Configurar paginación
            int pageSize = 5; // Número de registros por página
            int pageNumber = (page ?? 1); // Número de página, si no se especifica, es la página 1

            return View(viewModelUsuarios.ToPagedList(pageNumber, pageSize));
        }


        // POST: Usuario/EliminarUsuario
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

        //// GET: Usuario/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    var usuario = usuarios.FirstOrDefault(u => u.Id == id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(usuario);
        //}

        //// POST: Usuario/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Usuario usuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingUsuario = usuarios.FirstOrDefault(u => u.Id == usuario.Id);
        //        if (existingUsuario == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        existingUsuario.Nombre = usuario.Nombre;
        //        existingUsuario.Fecha = usuario.Fecha;
        //        existingUsuario.Sexo = usuario.Sexo;

        //        return RedirectToAction("Index");
        //    }
        //    return View(usuario);
        //}
    }

}
