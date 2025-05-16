using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Transacao
    {
        private string Id {  get; set; }
        public string Tipo { get; set; }
        public float Valor { get; set; }
        public DateTime Data { get; set; }

        public Transacao(string tipo, float valor, DateTime data)
        {
            Id = Guid.NewGuid().ToString();
            Tipo = tipo;
            Valor = valor;
            Data = data;
        }
    }
}
