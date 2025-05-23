using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Bank.Db;
using static System.Net.Mime.MediaTypeNames;

namespace Bank
{
    public class Conta
    {
        [Key]
        public int Numero { get; set; }
        public virtual Cliente Titular { get; private set; }
        public float Saldo { get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
        protected Conta() { } 
        public Conta(Cliente titular, string senha)
        {
            Titular = titular;
            Salt = CriarPalavra();
            Senha = Hashing(senha + Salt);
        }
        public Conta(Cliente titular, string senha, float deposito)
        {
            Titular = titular;
            Salt = CriarPalavra();
            Senha = Hashing(senha + Salt);
            Saldo = deposito;
        }
        private string CriarPalavra()
        {
            int stringlen = new Random().Next(5, 10);
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
        public void Sacar(float v, BankDAL<Conta> account, Conta conta)
        {
            if (Saldo < v) throw new Exception("Saldo insuficiete");
            Saldo -= v;
            Transacoes.Add(new Transacao("Saque", v, DateTime.Now));
            account.Update(conta);
            
        }
        public void Depositar(float v, BankDAL<Conta> account, Conta conta)
        {
            if (v < 0) throw new Exception("O valor deve ser positivo");
            
            Saldo += v;

            Transacoes.Add(new Transacao("Deposito", v, DateTime.Now));
            account.Update(conta);
            
        }
        public void ExibirTransacoes()
        {
            if (!Transacoes.Any()) throw new Exception("A conta deve ter transacoes");

            Console.WriteLine("Historico de transacoes");
            foreach (var transacao in Transacoes)
            {
                Console.WriteLine($"\nId: {transacao.Id}, Tipo: {transacao.Tipo} R${transacao.Valor} - Data {transacao.Data.Day}/{transacao.Data.Month}/{transacao.Data.Year} - Hora {transacao.Data.Hour}:{transacao.Data.Minute}min ");
            }
        }
        public static string Hashing(string source)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = GetHash(sha256Hash, source);
                return hash;
            }
        }
        private static string GetHash(HashAlgorithm sha256Hash, string senha)
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha)); // transformando string em bytes

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2")); // transformando bytes em hexadecimais
            }
            return sBuilder.ToString();
        }
        public static bool VerifyHash(HashAlgorithm hashAlgorithm, string senha, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, senha);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}

