using System;

namespace AppRastreamento.Models
{
    public class Error
    {
        public bool error { get; set; }
        public String message { get; set; }

        public Error(bool error, String message){    
            this.error = error;
            this.message = message;
        }
    }
}