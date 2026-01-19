using BarberShop.Domain.Entities;

public class Servico : Entity
{
    public string Nome { get; private set; }
    public TimeSpan Duracao { get; private set; }
    public decimal Valor { get; private set; }
    public string Descricao { get; private set; }
    public decimal PercentualComissao { get; private set; }

    private Servico() { }

    public Servico(string nome, TimeSpan duracao, decimal valor, string descricao, decimal percentualComissao)
    {
        Nome = nome;
        Duracao = duracao;
        Valor = valor;
        Descricao = descricao;
        PercentualComissao = percentualComissao;

        Validar();
    }

    public void Atualizar(string nome, TimeSpan duracao, decimal valor, string descricao, decimal percentualComissao)
    {
        Nome = nome;
        Duracao = duracao;
        Valor = valor;
        Descricao = descricao;
        PercentualComissao = percentualComissao;

        Validar();
        AtualizarDataModificacao();
    }

    public decimal CalcularComissao()
    {
        return Valor * (PercentualComissao / 100);
    }

    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome do serviço é obrigatório", nameof(Nome));

        if (Nome.Length < 3 || Nome.Length > 100)
            throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres", nameof(Nome));

        if (Duracao <= TimeSpan.Zero)
            throw new ArgumentException("Duração deve ser maior que zero", nameof(Duracao));

        if (Duracao.TotalMinutes % 30 != 0)
            throw new ArgumentException("Duração deve ser múltipla de 30 minutos", nameof(Duracao));

        if (Valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero", nameof(Valor));

        if (PercentualComissao < 0 || PercentualComissao > 100)
            throw new ArgumentException("Percentual de comissão deve estar entre 0 e 100", nameof(PercentualComissao));

        if (!string.IsNullOrWhiteSpace(Descricao) && Descricao.Length > 500)
            throw new ArgumentException("Descrição não pode ter mais de 500 caracteres", nameof(Descricao));
    }
}