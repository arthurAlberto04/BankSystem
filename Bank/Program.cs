using Bank;
using System.Globalization;

void Menu()
{
    int opcao;
    do
    {
        Console.WriteLine("Bem Vindo!");
        Console.WriteLine("\n1 - Criar Conta");
        Console.WriteLine("2 - Entrar");
        Console.WriteLine("3 - Sair");
        Console.Write("Opcao desejada: ");
        opcao = int.Parse(Console.ReadLine());
        if (opcao == 1) CriarConta();
        Entrar();
    } while (opcao < 1 || opcao > 2);
}

void Entrar()
{
    int numero;
    string senha;
    Console.Clear();
    Console.Write("Digite Sua Conta: ");
    numero = int.Parse(Console.ReadLine());
    Console.Write("Digite Sua Senha: ");
    senha = Console.ReadLine();

    // Fazer verificacao para ver se existe, se existir Exibir Menu de opcoes dentro da conta. Sacar, Exibir saldo etc....

}

void CriarConta()
{
    string senha;
    Console.Clear();
    var cliente = NovoCliente();
    Console.Write("Digite Sua Senha: ");
    senha = Console.ReadLine();
    Console.WriteLine("\nDeseja Fazer Um Deposito Inicial? s/n");
    char resp = char.Parse(Console.ReadLine());
    if (resp == 's')
    {
        Console.WriteLine("Qual o valor: ");
        float dep = float.Parse(Console.ReadLine());
        var c = new Conta(cliente, senha, dep);
        Console.WriteLine($"Numero da sua conta: {c.Numero}");
        return;
    }
    var conta = new Conta(cliente, senha);
    Console.WriteLine($"Numero da sua conta: {conta.Numero}");
    return;
}

Cliente NovoCliente()
{
    string cpf;
    string nome;
    DateTime dataNascimento;
    Console.Clear();
    Console.Write("Digite seu cpf: ");
    cpf = Console.ReadLine();
    Console.Write("Digite seu nome: ");
    nome = Console.ReadLine();
    Console.Write("Digite sua data de nascimento: ");
    string d = Console.ReadLine();
    dataNascimento = DateTime.Parse(d, CultureInfo.CreateSpecificCulture("pt-BR"));
    Cliente c = new Cliente(cpf, nome, dataNascimento);
    return c;
}

