﻿using HealthUp.Data;
using HealthUp.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HealthUp.Controllers
{

    [AjaxOnly]
    public class Validation_RegisterController : BaseController
    {
        private readonly HealthUpContext _context;


        public Validation_RegisterController(HealthUpContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult IsValidUsername(string Username)
        {
            Models.Pessoa P = _context.Pessoas.FirstOrDefault(p => p.Username == Username);
            Models.PedidoSocio Pedido = _context.PedidosSocios.FirstOrDefault(p => p.Username == Username);
            if (P == null && Pedido == null)
            {
                return Json(true);
            }
            else
            {
                return Json(new string("Este username já se encontra em utilização!"));
            }

        }

        [HttpPost]
        public JsonResult IsValidEmail(string Email)
        {
            Models.Pessoa P = _context.Pessoas.FirstOrDefault(p => p.Email == Email);
            Models.PedidoSocio Pedido = _context.PedidosSocios.FirstOrDefault(p => p.Email == Email);
            if (P == null && Pedido == null)
            {
                return Json(true);
            }
            else
            {
                return Json(new string("Este email já se encontra em utilização!"));
            }

        }

        [HttpPost]
        public JsonResult DoesExerciceExist(string Nome)
        {
            Models.Exercicio Ex = _context.Exercicios.SingleOrDefault(x => x.Nome == Nome);
            if (Ex == null)
            {
                return Json(true);
            }

            return Json(new string("Este exercício já existe!"));

        }

        [HttpPost]
        public JsonResult IsValidDateOfBirth(string DataNascimento)
        {
            DateTime min = DateTime.Now.AddYears(-18);
            DateTime max = DateTime.Now.AddYears(-110);
            string msg = string.Format("Por favor insira uma data entre {0:MM/dd/yyyy} e {1:MM/dd/yyyy}", max, min);
            try
            {
                DateTime date = DateTime.Parse(DataNascimento);
                if (date > min || date < max)
                {
                    return Json(msg);
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception)
            {
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult IsJpg(string Fotografia)
        {
            try
            {
                if (Path.GetExtension(Fotografia) != ".jpg")
                {
                    return Json("A fotografia tem de possuir o formato .jpg!");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception)
            {
                return Json("A fotografia tem de possuir o formato .jpg!");
            }
        }

        [HttpPost]
        public JsonResult IsValidVideo(string Video)
        {
            try
            {
                if (Path.GetExtension(Video) != ".mp4")
                {
                    return Json("O vídeo tem de possuir o formato .mp4!");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception)
            {
                return Json("O vídeo tem de possuir o formato .mp4!");
            }
        }

        public JsonResult IsValidPhoneNumber(string telemovel)
        {
            if (telemovel == null)
            {
                return Json(new string("O número inserido não é valido!"));
            }
            Match match = Regex.Match(telemovel, @"^(\d+)$", RegexOptions.IgnoreCase);// verificar se a string apenas contem numeros

            if (match.Success)
            {
                return Json(true);
            }

            return Json(new string("O número inserido não é valido!"));

        }
        [HttpPost]
        public JsonResult IsValidPassword(string Password)
        {
            if (Password == null)
            {
                return Json(new string("O número inserido não é valido!"));
            }


            Match match = Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", RegexOptions.IgnoreCase);// verificar se contem 1 maiuscula, 1 minuscula e 1 numero

            try
            {
                if (match.Success)
                {
                    return Json("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception)
            {
                return Json("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!");
            }

        }

        [HttpPost]
        public JsonResult IsValidNumCC(string numCC)
        {
            Models.Pessoa P = _context.Pessoas.FirstOrDefault(p => p.NumCC == numCC.ToString());
            Models.PedidoSocio Pedido = _context.PedidosSocios.FirstOrDefault(p => p.NumCC == numCC);
            if (P == null && Pedido == null)
            {
                return Json(true);
            }
            return Json(new string("Este número de cartão de cidadão já se encontra em utilização!"));
        }

        [HttpPost]
        public JsonResult IsValidCoordinates(string LocalizacaoGps)
        {
            try
            {
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                string[] coordenadas = LocalizacaoGps.Split(',');
                double latitude = double.Parse(coordenadas[0], NumberStyles.Any, ci);
                double longitude = double.Parse(coordenadas[1], NumberStyles.Any, ci);

                if (latitude <= 90 && latitude >= -90 && longitude <= 180 && longitude > -180)
                {
                    return Json(true);
                }
                return Json(new string("As coordenadas não tem o formato correto! (latitude,longitude)"));
            }
            catch (Exception)
            {
                return Json(new string("As coordenadas não tem o formato correto! (latitude,longitude)"));
            }


        }
        //[HttpPost]
        //public JsonResult IsValidNomeAula(string Nome)
        //{
        //    Models.Aula aula = _context.Aulas.SingleOrDefault(p => p.Nome == Nome);
        //    if (aula == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json("Já existe uma aula com o nome " + Nome);
        //    }
        //}

        [HttpGet]
        public JsonResult IsValidDataDe(DateTime ValidoDe)
        {
            if (ValidoDe == null)
            {
                return Json(false);
            }
            //var data = DateTime.Parse(ValidoDe);
            if (ValidoDe < DateTime.Now.Date)
            {
                return Json("Esta data não pode ser inferior à data atual!");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        public JsonResult IsValidDataAte(DateTime ValidoDe, DateTime ValidoAte)
        {
            if (ValidoDe == null || ValidoAte == null)
            {
                return Json(false);
            }

            if (ValidoAte < ValidoDe)
            {
                return Json("Esta data não pode ser inferior à data inicial de validade!");
            }
            else
            {
                return Json(true);
            }
        }
    }
}