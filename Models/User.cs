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

    [Table("user")]
    public class User
    {
        public int idUser { get; set; }
        public String token { get; set; }
        public String device { get; set; }
        public String tokenDevice { get; set; } 
    }


    public class UserDML {

        public UserDML(){
            
        }

        public User GetUserId(int idUser)
        {

            User user = new User();

            using (var context = new MyDbContext()){

                var users = context.User;
                var usersA = users.Where(p => p.idUser == idUser).ToList();

                if (usersA.Count > 0){
                    user = usersA[0];
                }   
            }

            return user;

        }
        
        
        public User Insert(User user){

            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();

                string tokenHash = gerarHash();

                user.token = tokenHash;               

                context.User.Add(user);
                
                context.SaveChanges();
                
            }


            return GetUserId(user.idUser);
        }

        private string gerarHash1()
        {
            int tamanho = 15;
            string validar = "abcdefghijklmnozABCDEFGHIJKLMNOZ1234567890@#$%&*!";
            StringBuilder strbld = new StringBuilder(100);
            Random random = new Random();
            while (0 < tamanho--)
            {
                strbld.Append(validar[random.Next(validar.Length)]);
            }

            MD5 md5Hash = MD5.Create();            
            string hash = GetMd5Hash(md5Hash, strbld.ToString());

            return hash;
        }


        private string gerarHash()
        {         
            string hash = "";
            int i = 1;
            while (i <= 5)
            {
                hash += gerarHash1();
                i++;
            }

            return hash;

        }


        public User Update(int idUser, User user)
        {

            using (var context = new MyDbContext())
            {
                var userList = context.User.Where(p => p.idUser == idUser).ToList()[0];
                userList.tokenDevice = user.tokenDevice;

                context.SaveChanges();
            }


            return GetUserId(idUser);
        }




        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }


    }

}