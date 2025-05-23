using System;
using System.ComponentModel.DataAnnotations;

namespace projet_1.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Le login est requis")]
        public required string Login { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "La date de naissance est requise")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Le genre est requis")]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "Le téléphone est requis")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Le téléphone doit contenir exactement 8 chiffres")]
        public required string Phone { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "La ville est requise")]
        public required string City { get; set; }

        [Required(ErrorMessage = "Le code postal est requis")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Le code postal doit contenir exactement 4 chiffres")]
        public required string PostalCode { get; set; }

        [EnumDataType(typeof(Role), ErrorMessage = "Rôle invalide")]
        public Role Role { get; set; }

        public string? Specialite { get; set; }

        public int AjoutePar { get; set; }

    }
}