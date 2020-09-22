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
    public class UserController : ControllerBase
    {
        Util util = new Util();
        UserDML userDML = new UserDML();

        [HttpGet]
        public IActionResult Get()
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            User user = userDML.GetUserId(idUser);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            Util util = new Util();
            var hash = HttpContext.Request.Headers["hash"].ToString();

            if (!util.ValidaHash(hash)){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }     

            if (user.device == "" || user.device == null){
                Error error = new Error(true, "Invalid data.");
                return BadRequest(error); 
            }    
            
            if (user.tokenDevice == null){
                Error error = new Error(true, "Invalid data.");
                return BadRequest(error); 
            }

            User userRet = userDML.Insert(user);

            return Ok(userRet);
        }

        [HttpPut]
        public IActionResult Put([FromBody]User user)
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            if (user.tokenDevice == null){
                Error error = new Error(true, "Invalid data.");
                return BadRequest(error); 
            }
            
            User userRet = userDML.Update(idUser, user);

            return Ok(userRet);

        }
    }
}