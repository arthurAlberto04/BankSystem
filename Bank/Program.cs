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

                var existeConta = account.Recoverby(c => c.Numero == numero);
                if (existeConta != null && existeConta.Senha.Equals(Conta.Hashing(senha + existeConta.Salt), StringComparison.Ordinal))
                {
                    success = true;
                    MenuConta(existeConta);
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
        void VerSaldo(Conta conta)
        {
            Console.Clear();
            Console.WriteLine($"Saldo R${conta.Saldo}");
            Console.Write("Pressione Enter para voltar");
            Console.ReadKey();
            MenuConta(conta);
        }
        void MenuSacar(Conta conta)
        {            
            Console.Clear();
            try
            {
                Console.WriteLine("Saque");
                Console.Write("\nInforme o valor que deseja sacar: ");
                float valor = float.Parse(Console.ReadLine());

                conta.Sacar(valor, account, conta);
                Console.WriteLine("Saque Efetuado com Sucesso");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex}. Tente Novamente, Saldo Disponivel R${conta.Saldo} para saque");
            }
            Console.Write("\nPressione Enter para voltar");
            Console.ReadKey();
            MenuConta(conta);
        }
        void Depositar(Conta conta)
        {
            Console.Clear();
            Console.WriteLine("Deposito");
            Console.Write("\nInforme o valor que deseja depositar: ");
            float deposito = float.Parse(Console.ReadLine());
            conta.Depositar(deposito, account, conta);

            Console.WriteLine("Deposito Efetuado com sucesso!");
            Console.WriteLine("\nPressione enter para voltar");
            Console.ReadKey();
            MenuConta(conta);
        }
        void ExibirExtrato(Conta conta)
        {
            Console.Clear();
            conta.ExibirTransacoes();
            Console.Write("\nPressione Enter Para voltar");
            Console.ReadKey();
            MenuConta(conta);
        }
        void MenuConta(Conta conta)
        {
            Console.Clear();
            Console.WriteLine("Menu Conta");
            Console.WriteLine("\n1 - Saldo");
            Console.WriteLine("2 - Sacar");
            Console.WriteLine("3 - Depositar");
            Console.WriteLine("4 - Extrato");
            Console.WriteLine("5 - Sair");
            Console.Write("\nDigite a opcao desejada: ");
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    VerSaldo(conta);
                    break;
                case 2:
                    MenuSacar(conta);
                    break;
                case 3:
                    Depositar(conta);
                    break;
                case 4:
                    ExibirExtrato(conta);
                    break;
            }
        }
        Menu();
    } 
}