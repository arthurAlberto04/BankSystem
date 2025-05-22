using Bank;
using Bank.Db;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args)
    {
        BankContext ctx = new BankContext();
        BankDAL<Conta> account = new BankDAL<Conta>(ctx);
        BankDAL<Cliente> cliente = new BankDAL<Cliente>(ctx);
        void Menu()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("Bem Vindo!");
                Console.WriteLine("\n1 - Criar Conta");
                Console.WriteLine("2 - Entrar");
                Console.WriteLine("3 - Sair");
                Console.Write("Opcao desejada: ");
                opcao = int.Parse(Console.ReadLine());
                if (opcao == 1) CriarConta();
                Entrar();
            } while (opcao < 1 || opcao <=2);
        }
        void Entrar()
        {
            bool success = false;
            do
            {
                int numero;
                string senha;
                Console.Clear();
                Console.Write("Digite Sua Conta: ");
                numero = int.Parse(Console.ReadLine());
                Console.Write("Digite Sua Senha: ");
                senha = Console.ReadLine();

                BankDAL<Conta> dal = new BankDAL<Conta>(ctx);
                var existeConta = dal.Recoverby(c => c.Numero == numero);
                if (existeConta != null && existeConta.Senha.Equals(Conta.Hashing(senha + existeConta.Salt), StringComparison.Ordinal))
                {
                    success = true;
                    MenuConta();
                }
                else
                {
                    Console.WriteLine("Conta ou senha Invalida(s)! Tente novamente.");
                    Console.ReadKey();
                }
            } while (success == false);
        }
        void CriarConta()
        {
            Conta conta;
            string senha;
            char resp;
            Cliente clienteConta;

            Console.Clear();
            Console.Write("Ja possui cadastro? (s/n): ");
            resp = char.Parse(Console.ReadLine());

            if(resp == 'n')
            {
                clienteConta = NovoCliente();
            }
            else
            {
                Console.Write("\nDigite seu cpf: ");
                string cpf = Console.ReadLine();
                clienteConta = cliente.Recoverby(c => c.CPF.Equals(cpf));
            }

            Console.Clear();
            Console.Write("Digite Sua Senha: ");
            senha = Console.ReadLine();

            Console.Write("\nDeseja Fazer Um Deposito Inicial? (s/n): ");
            resp = char.Parse(Console.ReadLine());

            if (resp == 's')
            {
                Console.WriteLine("Qual o valor: ");
                float dep = float.Parse(Console.ReadLine());
                conta = new Conta(clienteConta, senha, dep);
                account.Add(conta);
                Console.WriteLine($"Numero da sua conta: {conta.Numero}");
            }
            else
            {
                conta = new Conta(clienteConta, senha);
                account.Add(conta);
                Console.WriteLine($"Numero da sua conta: {conta.Numero}");
            }

            Console.Write("\nPressione Enter para voltar ao menu ");
            Console.ReadKey();
            Console.Clear();
            Menu();
        }
        Cliente NovoCliente()
        {
            bool sucesso = false;
            Cliente c = null;
            while (!sucesso)
            {
                Console.Clear();

                Console.Write("Digite seu cpf: ");
                string cpf = Console.ReadLine();

                Console.Write("Digite seu nome: ");
                string nome = Console.ReadLine();

                Console.Write("Digite sua data de nascimento: ");
                string d = Console.ReadLine();

                try
                {
                    DateTime dataNascimento = DateTime.Parse(d, CultureInfo.CreateSpecificCulture("pt-BR"));
                    c = new Cliente(cpf, nome, dataNascimento);
                    cliente.Add(c);
                    sucesso = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nData inválida.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n{ex.Message}. Tente novamente.\n");
                    Console.ReadKey();
                }
            }
            return c;
        }
        Menu();
    }

    private static void MenuConta()
    {
        Console.WriteLine("Sucesso");
        Console.ReadKey();
        //Menu a ser criado com features de deposito, saque e extrato
    } 
}