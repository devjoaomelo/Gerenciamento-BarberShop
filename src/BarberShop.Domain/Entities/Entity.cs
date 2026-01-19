namespace BarberShop.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set;  }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.Now;
    }

    public void AtualizarDataModificacao()
    {
        DataAtualizacao = DateTime.Now;
    }
}