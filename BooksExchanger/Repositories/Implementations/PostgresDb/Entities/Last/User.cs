// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// [Table("users")]
// [Index("Email", Name = "users_email_key", IsUnique = true)]
// public partial class User
// {
//     [Key]
//     [Column("id")]
//     public long Id { get; set; }
//
//     [Column("name")]
//     [StringLength(50)]
//     public string Name { get; set; } = null!;
//
//     [Column("email")]
//     [StringLength(50)]
//     public string Email { get; set; } = null!;
//
//     [Column("password")]
//     [StringLength(50)]
//     public string Password { get; set; } = null!;
//
//     [Column("role")]
//     public UserRole Role { get; set; }
//
//     [InverseProperty("GivenByUser")]
//     public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
//
//     [InverseProperty("Owner")]
//     public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
//
//     [InverseProperty("Owner")]
//     public virtual ICollection<OffersCollector> OffersCollectors { get; set; } = new List<OffersCollector>();
//
//     [ForeignKey("UserId")]
//     [InverseProperty("Users")]
//     public virtual ICollection<Offer> OffersNavigation { get; set; } = new List<Offer>();
// }
