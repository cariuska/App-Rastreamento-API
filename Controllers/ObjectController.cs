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
    public class ObjectController : ControllerBase
    {


        Util util = new Util();
        ObjectRastreioDML objectDML = new ObjectRastreioDML();

        [HttpGet]
        public IActionResult Get()
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            List<ObjectRastreio> objects = objectDML.GetObjectRastreio(idUser);

            /*
            if (objects.Count == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }
            */

            return Ok(objects);
        }

        [HttpGet("{idObject}")]
        public IActionResult GetId(int idObject)
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            ObjectRastreio objectRastreio = objectDML.GetObjectRastreioId(idUser, idObject);

            
            if (objectRastreio.idObject == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }

            return Ok(objectRastreio);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ObjectRastreio objectRastreio)
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }
            
            ObjectRastreio objectRastreioRet = objectDML.Insert(idUser, objectRastreio);
            
            if (objectRastreioRet.idObject == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }

            return Ok(objectRastreioRet);
        }

        [HttpPut("{idObject}")]
        public IActionResult Put(int idObject, [FromBody] ObjectRastreio objectRastreio)
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }
            
            ObjectRastreio objectRastreioRet = objectDML.Update(idUser, idObject, objectRastreio);
            
            if (objectRastreioRet.idObject == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }

            return Ok(objectRastreioRet);
        }

        [HttpDelete("{idObject}")]
        public IActionResult Delete(int idObject)
        {
            Error error = null;
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                error = new Error(true, "Not Authorized.");
                return BadRequest(error);
            }
            
            objectDML.Delete(idUser, idObject);
            
            error = new Error(false, "Deleted record.");
            return Ok(error);
        }
    }
}