using BarberShop.Domain.Enums;

namespace BarberShop.Domain.Entities;

public class Agendamento : Entity
{
    public Guid ClienteId { get; private set; }
    public Cliente Cliente { get; private set; }

    public Guid ProfissionalId { get; private set; }
    public Profissional Profissional { get; private set; }

    public Guid ServicoId { get; private set; }
    public Servico Servico { get; private set; }

    public DateTime DataHoraInicio { get; private set; }
    public DateTime DataHoraFim { get; private set; }

    public StatusAgendamento Status { get; private set; }

    public string? Observacoes { get; private set; }

    private Agendamento() { }

    public Agendamento(
        Guid clienteId,
        Guid profissionalId,
        Guid servicoId,
        DateTime dataHoraInicio,
        string? observacoes = null)
    {
        ClienteId = clienteId;
        ProfissionalId = profissionalId;
        ServicoId = servicoId;
        DataHoraInicio = dataHoraInicio;
        Observacoes = observacoes;
        Status = StatusAgendamento.Agendado;

        Validar();
    }

    public void DefinirServico(Servico servico)
    {
        if (servico == null)
            throw new ArgumentNullException(nameof(servico));

        Servico = servico;
        DataHoraFim = DataHoraInicio.Add(servico.Duracao);
    }

    public void Confirmar()
    {
        if (Status != StatusAgendamento.Agendado)
            throw new InvalidOperationException("Apenas agendamentos com status 'Agendado' podem ser confirmados");

        Status = StatusAgendamento.Confirmado;
        AtualizarDataModificacao();
    }

    public void Realizar()
    {
        if (Status != StatusAgendamento.Confirmado)
            throw new InvalidOperationException("Apenas agendamentos confirmados podem ser marcados como realizados");

        Status = StatusAgendamento.Realizado;
        AtualizarDataModificacao();
    }

    public void Cancelar()
    {
        if (Status == StatusAgendamento.Realizado)
            throw new InvalidOperationException("Não é possível cancelar um agendamento já realizado");

        if (Status == StatusAgendamento.Cancelado || Status == StatusAgendamento.Faltou)
            throw new InvalidOperationException("Agendamento já foi finalizado");

        Status = StatusAgendamento.Cancelado;
        AtualizarDataModificacao();
    }

    public void MarcarComoFaltou()
    {
        if (Status == StatusAgendamento.Realizado)
            throw new InvalidOperationException("Não é possível marcar como falta um agendamento já realizado");

        if (Status == StatusAgendamento.Cancelado || Status == StatusAgendamento.Faltou)
            throw new InvalidOperationException("Agendamento já foi finalizado");

        Status = StatusAgendamento.Faltou;
        AtualizarDataModificacao();
    }

    public void AdicionarObservacao(string observacao)
    {
        Observacoes = observacao;
        AtualizarDataModificacao();
    }

    public bool PodeSerAlterado()
    {
        return Status == StatusAgendamento.Agendado || Status == StatusAgendamento.Confirmado;
    }

    public bool OcupaHorario(DateTime inicio, DateTime fim)
    {
        return DataHoraInicio < fim && DataHoraFim > inicio;
    }

    private void Validar()
    {
        if (ClienteId == Guid.Empty)
            throw new ArgumentException("Cliente é obrigatório", nameof(ClienteId));

        if (ProfissionalId == Guid.Empty)
            throw new ArgumentException("Profissional é obrigatório", nameof(ProfissionalId));

        if (ServicoId == Guid.Empty)
            throw new ArgumentException("Serviço é obrigatório", nameof(ServicoId));

        if (DataHoraInicio < DateTime.UtcNow)
            throw new ArgumentException("Data e hora do agendamento não pode ser no passado", nameof(DataHoraInicio));

        if (DataHoraInicio.Minute % 30 != 0)
            throw new ArgumentException("Agendamento deve iniciar em horários múltiplos de 30 minutos", nameof(DataHoraInicio));

        if (!string.IsNullOrWhiteSpace(Observacoes) && Observacoes.Length > 500)
            throw new ArgumentException("Observações não podem ter mais de 500 caracteres", nameof(Observacoes));
    }
}