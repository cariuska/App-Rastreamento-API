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
    public class ObjectLocationController : ControllerBase
    {


        Util util = new Util();
        ObjectLocationDML objectLocationDML = new ObjectLocationDML();

        [HttpGet("{idObject}")]
        public IActionResult Get(int idObject)
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            List<ObjectLocation> objects = objectLocationDML.GetObjectLocation(idObject);

            /*
            if (objects.Count == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }
            */

            return Ok(objects);
        }

        [HttpGet("{idObject}/{idObjectLocation}")]
        public IActionResult GetId(int idObject, int idObjectLocation)
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            ObjectLocation objectLocation = objectLocationDML.GetObjectLocationId(idObject, idObjectLocation);

            /*
            if (objects.Count == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }
            */

            return Ok(objectLocation);
        }

    }
}