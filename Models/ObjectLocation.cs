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

    [Table("objectLocation")]
    public class ObjectLocation
    {
        public int idObjectLocation { get; set; }
        public int idObject { get; set; }
        public DateTime data { get; set; }
        public String local { get; set; }
        public String status { get; set; }
        public String source { get; set; }
        public String sourceAddress { get; set; }
        public String sourceLat { get; set; }
        public String sourceLng { get; set; }
        public String destiny { get; set; }
        public String destinyAddress { get; set; }
        public String destinyLat { get; set; }
        public String destinyLng { get; set; }
        
        [JsonIgnore]
        public virtual ObjectRastreio objectRastreio  { get; set; }
    }


    public class ObjectLocationDML {

        public ObjectLocationDML(){
            
        }
        
        public List<ObjectLocation> GetObjectLocation(int idObject)
        {

            List<ObjectLocation> objectRet = new List<ObjectLocation>();

            using (var context = new MyDbContext()){

                var objects = context.ObjectLocation;
                objectRet = objects.Where(p => p.idObject == idObject).OrderBy(o => o.data).ToList();
            }

            return objectRet;

        }

        
        public ObjectLocation GetObjectLocationUltimo(int idObject)
        {

            ObjectLocation objectRet = new ObjectLocation();

            using (var context = new MyDbContext()){

                var objects = context.ObjectLocation;
                var objectsA = objects.Where(p => p.idObject == idObject).OrderByDescending(o => o.data).ToList();

                if (objectsA.Count > 0){
                    objectRet = objectsA[0];
                }   
            }

            return objectRet;

        }

        public ObjectLocation GetObjectLocationId(int idObject, int idObjectLocation)
        {

            ObjectLocation objectRet = new ObjectLocation();

            using (var context = new MyDbContext()){

                var objects = context.ObjectLocation;
                var objectsA = objects.Where(p => p.idObjectLocation == idObjectLocation && p.idObject == idObject).ToList();

                if (objectsA.Count > 0){
                    objectRet = objectsA[0];
                }   
            }

            return objectRet;

        }
        
        
        public void Insert(ObjectLocation objectLocation){

            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();

                context.ObjectLocation.Add(objectLocation);
                
                context.SaveChanges();
                
            }

        }

    }

}