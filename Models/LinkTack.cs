using System.Collections.Generic;

namespace AppRastreamento.Models
{
    public class LinkTack
    {
        public string codigo { get; set; }
        public string servico { get; set; }
        public string host { get; set; }
        public int quantidade { get; set; }
        public List<Eventos> eventos { get; set; }
        public decimal time { get; set; }
        public string ultimo { get; set; }
    }

    
    public class Eventos
    {
        public string data { get; set; }
        public string hora { get; set; }
        public string local { get; set; }
        public string status { get; set; }
        public List<string> subStatus { get; set; }
    }

    public class Endereco
    {
        public bool achou { get; set; }
        public string local { get; set; }
        public string address { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }

    }
}