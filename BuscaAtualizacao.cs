using System;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Collections.Generic;
using AppRastreamento.Models;
using AppRastreamento.Utils;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AppRastreamento
{
    public class BuscaAtualizacao
    {
        public string HandleSQSEvent(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {                
                ObjectRastreio json  = JsonConvert.DeserializeObject<ObjectRastreio>(record.Body);
                
                Console.WriteLine(json.objectCode);

                Util util = new Util();
                util.buscaAtualiacao(json.idObject);

            }

            return $"Processed {sqsEvent.Records.Count} records.";
        }
        

        public void CronJob()
        {
            
            ObjectRastreioDML objectDML = new ObjectRastreioDML();

            List<ObjectRastreio> objectsRastreio = objectDML.GetAllObjectRastreio("A");

            foreach(ObjectRastreio item in objectsRastreio)
            {
                try{

                    string body = "{ \"idObject\": " + item.idObject + " }";

                    Util util = new Util();
                    util.SendSQS(body);

                }catch(Exception e){
                    Console.WriteLine("Erro no Object: " + item.idObject + " / Erro: " +e.Message);
                }
            }
        }

    }
}