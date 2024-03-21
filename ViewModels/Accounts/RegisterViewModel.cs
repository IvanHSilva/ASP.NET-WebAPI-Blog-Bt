using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class RegisterViewModel
{

    [Required(ErrorMessage = "Nome obrigatório")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "E-Mail obrigatório")]
    [EmailAddress(ErrorMessage = "E-Mail inválido!")]
    public string Email { get; set; } = string.Empty;
}
