﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapstoneProject.Models {
    [Table("User")]
    [Index(nameof(Username), IsUnique = true)]
    public class User {
        public int Id { get; set; }
        [StringLength(30)]
        public string Username { get; set; }
        [StringLength(30)]
        public string Password { get; set; }
        [StringLength(30)]
        public string Firstname { get; set; }
        [StringLength(30)]
        public string Lastname { get; set; }
        [StringLength(12)]
        public string? Phone { get; set; }
        [StringLength(255)]
        public string? Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

    }
}
