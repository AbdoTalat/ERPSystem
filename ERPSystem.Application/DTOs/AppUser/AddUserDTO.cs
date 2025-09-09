using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.AppUser
{
    public class AddUserDTO
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }


        [Required(ErrorMessage = "At least one branch must be assigned.")]
        public List<int> BranchIds { get; set; } = new();

        [Required(ErrorMessage = "Default branch is required.")]
        public int DefaultBranchId { get; set; }
    }
}
