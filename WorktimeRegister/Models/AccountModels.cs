using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace WorktimeRegister.Models
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage="{0} is required")]
        [StringLength(50, ErrorMessage = "The {0} length should be less than 50 characters long.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "The {0} length should be less than 50 characters long.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} length should be less than 50 characters long.")]
        [Display(Name = "Email adress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, ErrorMessage = "The {0} must be maximum {1} characters long.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "The {0} should contain only numbers.")]
        [Display(Name = "Mobile number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
