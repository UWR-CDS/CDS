using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDS.Models
{
    public class UserData
    {
        public int UserID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string UserFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string UserLastName { get; set; }

        [Required(ErrorMessage = "Enter Your Email Address")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Display(Name = "Email Address")]
        public string UserEmail { get; set; }
        [Required]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "Phone Number is not valid")]
        [Display(Name = "Contact Number")]
        public string UserPhone { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string RetypePassword { get; set; }
        public int Status { get; set; }
        public int LoginAs { get; set; }
        public int isMaster { get; set; }
        public List<Privilige> priviligesList { get; set; }
        public List<UserData> UserList { get; set; }
        public string DefaultPage { get; set; }
        public string EntityName { get; set; }
        public int EntityID { get; set; }
        public int LastActivityTime { get; set; }
        public List<Models.Les_Subject> lstUserSubjects { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Your Email Address")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Enter Your Email Address")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Enter Your Email Address")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        ////[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum 8 characters at least 1 Alphabet and 1 Number")]
        [StringLength(8, ErrorMessage = "Password must be 8 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginTracking
    {
        public int LoginTrackID { get; set; }
        public int UserID { get; set; }
        public DateTime LogoutTime { get; set; }
    }
}