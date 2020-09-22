using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppRastreamento.Utils;
using AppRastreamento.Models;

namespace AppRastreamento.Controllers
{
    [Route("v1/[controller]")]
    public class UpdateObjectController : ControllerBase
    {
        Util util = new Util();
        ObjectRastreioDML objectRastreioDML = new ObjectRastreioDML();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Ok Atualizado Todos");
        }

        [HttpGet("{idObject}")]
        public IActionResult GetId(int idObject)
        {
            ObjectRastreio objectRastreio = objectRastreioDML.GetObjectRastreioId2(idObject);
            util.buscaAtualiacao(idObject);
            return Ok("Ok Atualizado " + idObject);
        }

    }
}