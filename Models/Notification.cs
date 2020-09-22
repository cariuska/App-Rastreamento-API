using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using AppRastreamento.Utils;
using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace AppRastreamento.Models
{

    [Table("notification")]
    public class Notification
    {
        public int idNotification { get; set; }
        public int idUser { get; set; }
        public int? idObject { get; set; }
        public int? idObjectLocation { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public bool flagSend { get; set; }
        public bool flagRead { get; set; }
        public DateTime date { get; set; }
    }


    public class NotificationDML {

        public NotificationDML(){
            
        }
        
        public List<Notification> GetNotification(int idUser)
        {

            List<Notification> notification = new List<Notification>();

            using (var context = new MyDbContext()){

                var objects = context.Notification;
                notification = objects.Where(p => p.idUser == idUser).ToList();
            }

            return notification;

        }

        public Notification GetObjectLocationId(int idUser, int idNotification)
        {

            Notification notification = new Notification();

            using (var context = new MyDbContext()){

                var objects = context.Notification;
                var objectsA = objects.Where(p => p.idUser == idUser && p.idNotification == idNotification).ToList();

                if (objectsA.Count > 0){
                    notification = objectsA[0];
                }   
            }

            return notification;

        }
        
        
        public void Insert(Notification notification){

            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();

                context.Notification.Add(notification);
                
                context.SaveChanges();
                
            }

        }


    }

}