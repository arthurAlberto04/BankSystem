using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Conta
    {
        public int Numero { get; set; }
        public Cliente Titular { get; private set; }
        private float Saldo { get; set; }
        public string Senha { get; set; }
        private List<Transacao> Transacoes = new List<Transacao>();
        public Conta(Cliente titular, string senha)
        {
            Numero = new Random().Next(1000000, 10000000);
            Titular = titular;
            string salt = CriarPalavra();
            Senha = CriptografarSenha(senha + salt);
        }
        public Conta(Cliente titular, string senha, float deposito)
        {
            Numero = new Random().Next(1000000, 10000000);
            Titular = titular;
            string salt = CriarPalavra();
            Senha = CriptografarSenha(senha + salt);
            Saldo = deposito;
        }
        private string CriarPalavra()
        {
            int stringlen = new Random().Next(0, 10);
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {
                randValue = new Random().Next(97, 123);

                letter = Convert.ToChar(randValue);

                str = str + letter;
            }
            return str;
        }
        public float GetSaldo()
        {
            return Saldo;
        }
        public void Sacar(int v)
        {
            if (Saldo < v) throw new Exception("Saldo insuficiete");

            Saldo -= v;

            Transacoes.Add(new Transacao("Sacar", v, DateTime.Now));
        }
        public void Depositar(int v)
        {
            if (v < 0) throw new Exception("O valor deve ser positivo");
            
            Saldo += v;

            Transacoes.Add(new Transacao("Depositar", v, DateTime.Now));
        }
        public void ExibirTransacoes()
        {
            if (!Transacoes.Any()) throw new Exception("A conta deve ter transacoes");

            Console.WriteLine("Historico de transacoes");
            foreach (var transacao in Transacoes)
            {
                Console.WriteLine($"Tipo: {transacao.Tipo} R${transacao.Valor} - Data {transacao.Data.Day}/{transacao.Data.Month}/{transacao.Data.Year} - Hora {transacao.Data.Hour}:{transacao.Data.Minute}min ");
            }
        }


        private string CriptografarSenha(string senha)
        {
            
            senha = Reverter(senha);
            senha = CifraDeCesar(senha);
            senha = Criptografar(senha);

            static string Reverter(string senha)
            {
                string resultado = "";

                for (int index = senha.Length - 1; index >= 0; index--)
                {
                    resultado += senha[index];
                }

                return resultado;
            }

            static string CifraDeCesar(string senha)
            {
                string resultado = "";

                for (int i = 0; i < senha.Length; i++)
                {
                    int aux = (int)senha[i] + 3; // convertando para valor ASCII inteiro e adicionando + 3;
                    resultado += (char)aux; // retornando para char e adicionando à string.
                }

                return resultado;
            }

            static string Criptografar(string senha)
            {
                string criptografia = "";
                int aux;
                int valor = 0;

                for (int i = 0; i < senha.Length; i++)
                {
                    aux = (int)(senha[i] + (Math.Pow(2, valor) + 1));
                    criptografia += (char)aux;
                    valor++;

                    if (valor == 3)
                    {
                        valor = 0;
                    }
                }

                return criptografia;
            }

            return senha;
        }

        public bool ValidarSenha(string senha)
        {
            string senhaCriptografada = CriptografarSenha(senha);
            return senhaCriptografada == Senha;
        }
    }
}

