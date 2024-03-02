using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels; 

public class LoginViewModel {

    [Required(ErrorMessage = "E-Mail obrigatório")]
    [EmailAddress(ErrorMessage = "E-Mail inválido!")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha obrigatória")]
    public string Password { get; set; } = string.Empty;
}
