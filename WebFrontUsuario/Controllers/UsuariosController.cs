﻿using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebFrontUsuario.Models;
using System.Collections.Generic;
using ServicioUsuarios;
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
        public ActionResult UsuariosConsulta()
        {
            return View(usuarioViewModel);
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

        //// GET: Usuario/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var usuario = usuarios.FirstOrDefault(u => u.Id == id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(usuario);
        //}

        //// POST: Usuario/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var usuario = usuarios.FirstOrDefault(u => u.Id == id);
        //    if (usuario != null)
        //    {
        //        usuarios.Remove(usuario);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
