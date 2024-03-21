using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts; 

public class UploadImageViewModel {
    [Required(ErrorMessage = "Imagem inváldia!")]
    public string Base64Image { get; set; } = string.Empty;
}
