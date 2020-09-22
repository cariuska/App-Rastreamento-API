using System;
using System.Collections.Generic;
using System.Data;
using AppRastreamento.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace AppRastreamento.Models
{

    [Table("object")]
    public class ObjectRastreio
    {
        public int idObject { get; set; }
        public int idUser { get; set; }
        public String objectCode { get; set; }
        public String description { get; set; }
        public DateTime dateRegister { get; set; }
        public String status { get; set; }
        public virtual ICollection<ObjectLocation> objectLocation  { get; set; }
    }


    public class ObjectRastreioDML {

        public ObjectRastreioDML(){
            
        }
        
        public List<ObjectRastreio> GetObjectRastreio(int idUser)
        {

            List<ObjectRastreio> objectRet = new List<ObjectRastreio>();

            using (var context = new MyDbContext()){

                var objects = context.ObjectRastreio;
                objectRet = objects.Where(p => p.idUser == idUser).ToList();

                ObjectLocationDML objectLocationDML = new ObjectLocationDML();

                foreach (var obj in  objectRet){
                    
                    obj.objectLocation = new List<ObjectLocation>();
                    var objectLocation = objectLocationDML.GetObjectLocationUltimo(obj.idObject);
                    if (objectLocation.idObjectLocation > 0){
                        obj.objectLocation.Add(objectLocation);
                    }
                }

            }

            return objectRet;

        }

        
        
        public List<ObjectRastreio> GetAllObjectRastreio(string status)
        {
            List<ObjectRastreio> objectRet = new List<ObjectRastreio>();
            using (var context = new MyDbContext()){

                var objects = context.ObjectRastreio;
                objectRet = objects.Where(p => p.status == status).ToList();
            }
            return objectRet;
        }

        public ObjectRastreio GetObjectRastreioId(int idUser, int idObject)
        {

            ObjectRastreio objectRet = new ObjectRastreio();

            using (var context = new MyDbContext()){

                var objects = context.ObjectRastreio;
                var objectsA = objects.Where(p => p.idUser == idUser && p.idObject == idObject).ToList();

                if (objectsA.Count > 0){
                    objectRet = objectsA[0];
                    
                    ObjectLocationDML objectLocationDML = new ObjectLocationDML();
                    objectRet.objectLocation = new List<ObjectLocation>();
                                        
                    var objectLocation = objectLocationDML.GetObjectLocationUltimo(idObject);
                    if (objectLocation.idObjectLocation > 0){
                        objectRet.objectLocation.Add(objectLocation);
                    }
                    
                }   
            }

            return objectRet;

        }
        
        public ObjectRastreio GetObjectRastreioId2(int idObject)
        {

            ObjectRastreio objectRet = new ObjectRastreio();

            using (var context = new MyDbContext()){

                var objects = context.ObjectRastreio;
                var objectsA = objects.Where(p => p.idObject == idObject).ToList();

                if (objectsA.Count > 0){
                    objectRet = objectsA[0];
                }   
            }

            return objectRet;

        }
        
        
        public ObjectRastreio GetObjectRastreioCode(string objectCode)
        {

            ObjectRastreio objectRet = new ObjectRastreio();

            using (var context = new MyDbContext()){

                var objects = context.ObjectRastreio;
                var objectsA = objects.Where(p => p.objectCode == objectCode).ToList();

                if (objectsA.Count > 0){
                    objectRet = objectsA[0];
                }   
            }

            return objectRet;

        }
        
        
        public ObjectRastreio Insert(int idUser, ObjectRastreio objectRastreio){

            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                objectRastreio.idUser = idUser;         
                objectRastreio.status = "A";
                objectRastreio.dateRegister = DateTime.Now;     

                context.ObjectRastreio.Add(objectRastreio);
                
                context.SaveChanges();

                string body = "{ \"idObject\": " + objectRastreio.idObject + " }";
                Util util = new Util();
                util.SendSQS(body);
            }


            return GetObjectRastreioId(idUser, objectRastreio.idObject);
        }

        
        public ObjectRastreio Update(int idUser, int idObject, ObjectRastreio objectRastreio)
        {

            using (var context = new MyDbContext())
            {
                var userList = context.ObjectRastreio.Where(p => p.idUser == idUser && p.idObject == idObject).ToList()[0];
                userList.description = objectRastreio.description;

                context.SaveChanges(); 
            }


            return GetObjectRastreioId(idUser, idObject);
        }

        public void UpdateStatus(int idObject, string status)
        {

            using (var context = new MyDbContext())
            {
                var userList = context.ObjectRastreio.Where(p => p.idObject == idObject).ToList()[0];
                userList.status = status;

                context.SaveChanges(); 
            }
        }
        
        
        public void Delete(int idUser, int idObject)
        {

            using (var context = new MyDbContext())
            {
                var objectRastreio = context.ObjectRastreio.Where(p => p.idUser == idUser && p.idObject == idObject).ToList()[0];

                foreach(var objectLocation in context.ObjectLocation.Where(p => p.idObject == idObject).ToList()){
                    context.ObjectLocation.Remove(objectLocation);
                }
                
                foreach(var notification in context.Notification.Where(p => p.idObject == idObject).ToList()){
                    context.Notification.Remove(notification);
                }                
                
                context.ObjectRastreio.Remove(objectRastreio);

                context.SaveChanges(); 
            }
        }
        

    }

}