using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Transacao
    {
        [Key]
        public int Id {  get; set; }
        public string Tipo { get; set; }
        public float Valor { get; set; }
        public DateTime Data { get; set; }
        protected Transacao() { }

        public Transacao(string tipo, float valor, DateTime data)
        {
            Tipo = tipo;
            Valor = valor;
            Data = data;
        }
    }
}
