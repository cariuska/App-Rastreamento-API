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
    public class NotificationController : ControllerBase
    {


        Util util = new Util();
        NotificationDML notificationDML = new NotificationDML();

        [HttpGet("")]
        public IActionResult Get()
        {
            int idUser = util.ValidaTokenHash(HttpContext);
            if (idUser <= 0){
                Error error = new Error(true, "Not Authorized.");
                return BadRequest(error); 
            }

            List<Notification> notifications = notificationDML.GetNotification(idUser);

            /*
            if (objects.Count == 0){
                Error error = new Error(true, "Record not found.");
                return NotFound(error);
            }
            */

            return Ok(notifications);
        }

        [HttpGet("{idNotification}")]
        public IActionResult GetId(int idNotification)
        {
            return Ok("Get Id " + idNotification + " Notification");
        }

    }
}