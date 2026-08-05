namespace Budget.Application.Dtos.Authentication;

public sealed record LoginDto(string Email, string MotDePasse, bool SeSouvenirDeMoi);
