using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Cliente
    {
        [Key]
        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public int Idade { get; set; }
        protected Cliente() { }
        public Cliente(string cpf, string nome, DateTime nascimento)
        {
            if (!CpfEhValido(cpf)) throw new Exception("CPF Invalido");
            CPF = cpf;
            Nome = nome;
            Nascimento = nascimento.Date;
            Idade = DateTime.Today.Date.Subtract(Nascimento).Days / 365;
        }
        private bool CpfEhValido(string cpf)
        {
            int[] multiplicadores = {10, 9, 8, 7, 6, 5, 4, 3, 2};
            int[] multiplicadores2d = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            cpf = cpf.Replace(".", "");
            cpf = cpf.Replace("-", "");
            if(cpf.Length != 11 )
                return false;

            string parcial = cpf.Substring(0, 9); //Metodo Substring seleciona os chars em um range passado como parametro.
            int verificador1 = CalculoVerificadores(parcial, multiplicadores);
            int verficador2 = CalculoVerificadores(parcial + verificador1, multiplicadores2d);

            return cpf[9] - '0' == verificador1 && cpf[10] - '0' == verficador2;
        }
        private int CalculoVerificadores(string parcial, int[] multiplicadores)
        {
            int result = 0;

            for(int i = 0; i < multiplicadores.Length; i++)
            {
                result += (parcial[i] - '0') * multiplicadores[i];  //Em ASCII '0' = 48, '1' = 49...para obter o inteiro dentro do Array de string x - '0' = int 
            }

            result = result % 11;
            return result < 2 ? 0 : 11 - result;
        }
    }
}
