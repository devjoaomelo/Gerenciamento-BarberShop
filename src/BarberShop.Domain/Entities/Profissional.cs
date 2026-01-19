namespace BarberShop.Domain.Entities;

public class Profissional : Entity
{
    public string Nome { get; private set; }
    public string Telefone { get; private set; }
    public string Email { get; private set; }

    private Profissional() { }

    public Profissional(string nome, string telefone, string email)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;

        Validar();
    }

    public void Atualizar(string nome, string telefone,string email)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;

        Validar();
        AtualizarDataModificacao();
    }
    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome é obrigatório", nameof(Nome));

        if (Nome.Length < 3 || Nome.Length > 100)
            throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres", nameof(Nome));

        if (string.IsNullOrWhiteSpace(Telefone))
            throw new ArgumentException("Telefone é obrigatório", nameof(Telefone));

        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email é obrigatório", nameof(Email));

        if (!Email.Contains("@"))
            throw new ArgumentException("Email inválido", nameof(Email));
    }
}