using System;
using System.Data;
using AppRastreamento.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using RestClient.Net;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AppRastreamento.Utils
{
    public class Util
    {
        
        public int ValidaToken(String token){
            
            int idUser = 0;

            using (var context = new MyDbContext()){

                var users = context.User;
                var usersA = users.Where(p => p.token == token).ToList();

                if (usersA.Count > 0){
                    idUser = usersA[0].idUser;
                }   
            }

            return idUser;
        }

        public bool ValidaHash(String hash){

            string hashValido = Environment.GetEnvironmentVariable("HASH");

            if (hash == hashValido){
                return true;
            }else{
                return false;
            }
        }

        public int ValidaTokenHash(HttpContext context){
            int idUser = 0;

            String hash = context.Request.Headers["hash"].ToString();
            String token = context.Request.Headers["token"].ToString();
            
            if (!this.ValidaHash(hash)){
                return 0;
            }            
            idUser = this.ValidaToken(token);
            return idUser;
        }
        


        public dynamic getQuery<T>(HttpContext context, String name, int minValue = 0){

            String value = context.Request.Query[name].ToString();

            if (typeof(T) == typeof(int)){
                int valueI = value == "" ? 0 : Convert.ToInt32(value);
                if (valueI <= 0){
                    valueI = minValue;
                }
                return valueI;
            }

            return value;
        }

        public Endereco buscaEndereco(string busca){

            Endereco retorno = new Endereco();
            retorno.achou = false;
            retorno.local = busca;

            string key = Environment.GetEnvironmentVariable("KEY_GEOCODE");
            string urlGeoCodeGoogle = Environment.GetEnvironmentVariable("URL_GEOCODE");


            string url = $"{urlGeoCodeGoogle}?address={busca}&key={key}";

            var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(url));
            var response = client.GetAsync<GeoCode>();

            GeoCode retultado = response.Result.Body;

            if (retultado.status == "OK"){
                retorno.achou = true;
                retorno.lat = retultado.results[0].geometry.location.lat;
                retorno.lng = retultado.results[0].geometry.location.lng;
                retorno.address = retultado.results[0].formatted_address;
            }

            return retorno;
        }

        public void buscaAtualiacao(int idObject){


            ObjectRastreioDML objectRastreioDML = new ObjectRastreioDML();

            ObjectRastreio objectRastreio = objectRastreioDML.GetObjectRastreioId2(idObject);


            string user = Environment.GetEnvironmentVariable("USER_LINK_TRACE");
            string token = Environment.GetEnvironmentVariable("TOKEN_LINK_TRACE");
            string urlLinkeTrace = Environment.GetEnvironmentVariable("URL_LINK_TRACE");

            string url = $"{urlLinkeTrace}?user={user}&token={token}&codigo={objectRastreio.objectCode}";

            var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(url));
            var response = client.GetAsync<LinkTack>();

            LinkTack retultado = response.Result.Body;


            ObjectLocationDML objectLocationDML = new ObjectLocationDML();
            NotificationDML notificationDML = new NotificationDML();

            List<ObjectLocation> objectLocations = objectLocationDML.GetObjectLocation(objectRastreio.idObject);

            
            if (retultado.quantidade > objectLocations.Count){

                int dif = retultado.quantidade - objectLocations.Count;

                foreach(var eventos in retultado.eventos){

                    if (dif > 0){

                        string source = "";                        
                        string sourceAddress = "";
                        string sourceLat = "";
                        string sourceLng = "";
                        string destiny = "";
                        string destinyAddress = "";
                        string destinyLat = "";
                        string destinyLng = "";

                        foreach(var sub in eventos.subStatus){
                            
                            if (sub.ToUpper().Contains("LOCAL") || sub.ToUpper().Contains("ORIGEM") || sub.Contains("Registrado por"))
                            {
                                var sub2 = sub.Replace("Local: ", "").Replace("Origem: ", "").Replace("Registrado por ", "");

                                Endereco end = buscaEndereco(sub2);
                                if (end.achou)
                                {
                                    source = sub2;
                                    sourceAddress = end.address;
                                    sourceLat = end.lat;
                                    sourceLng = end.lng;
                                }else{                                
                                    source = sub2;
                                }
                            }else if (sub.ToUpper().Contains("DESTINO"))
                            {         
                                var sub2 = sub.Replace("Destino: ", "");
                                Endereco end = buscaEndereco(sub2);
                                if (end.achou)
                                {                            
                                    destiny = sub2;
                                    destinyAddress = end.address;
                                    destinyLat = end.lat;
                                    destinyLng = end.lng;
                                }else{                                
                                    destiny = sub2;
                                }
                            }else{                   
                                source = sub;
                            }
                            

                        }


                        string[] formats= {"dd/MM/yyyy H:mm"};

                        DateTime data = DateTime.ParseExact(eventos.data + " " + eventos.hora, formats, new CultureInfo("en-US"), DateTimeStyles.None);

                        
                        ObjectLocation novo = new ObjectLocation{
                            idObject = objectRastreio.idObject,
                            data = data,
                            local = eventos.local,
                            status = eventos.status,
                            source = source,
                            sourceAddress = sourceAddress,
                            sourceLat = sourceLat,
                            sourceLng = sourceLng,
                            destiny = destiny,
                            destinyAddress = destinyAddress,
                            destinyLat = destinyLat,
                            destinyLng = destinyLng
                        };

                        objectLocationDML.Insert(novo);

                        if (eventos.status == "Objeto entregue ao destinatário"){
                            objectRastreioDML.UpdateStatus(objectRastreio.idObject, "F");
                        }

                        Notification notification = new Notification{
                            idUser = objectRastreio.idUser,
                            idObject = objectRastreio.idObject,
                            idObjectLocation = novo.idObjectLocation,
                            title = "Nova Atualização",
                            message = "Mensagem",
                            flagSend = false,
                            flagRead = false,
                            date = DateTime.Now
                        };

                        notificationDML.Insert(notification);

                        dif--;
                    }

                }

            }
        }

        public void SendSQS(string body)
        {      
            
            try{

                string urlSQS = Environment.GetEnvironmentVariable("URL_SQS");

                var client = new AmazonSQSClient(Amazon.RegionEndpoint.USEast1);

                var request = new SendMessageRequest
                {
                    MessageBody = body,
                    QueueUrl = urlSQS
                };

                var response = client.SendMessageAsync(request);

            }
            catch (Exception e){
                Console.WriteLine("Erro no SQS - Body: " + body + " \n Erro: " + e);
            }

        }


    }
}