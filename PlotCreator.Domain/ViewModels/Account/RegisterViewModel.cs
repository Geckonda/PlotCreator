using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(20, ErrorMessage = "Имя должно иметь длину не больше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину не меньше 3 символов")]
        [Display(Name = "Никнейм")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [MaxLength(20, ErrorMessage = "Логин должен иметь длину не больше 20 символов")]
        [MinLength(3, ErrorMessage = "Логин должен иметь длину не меньше 3 символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [EmailAddress(ErrorMessage = "Введено неверно")]

        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину не меньше 6 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
    }
}
