// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// [Table("offers")]
// public partial class Offer
// {
//     [Key]
//     [Column("id")]
//     public Guid Id { get; set; }
//
//     [Column("book_id")]
//     public long? BookId { get; set; }
//
//     [Column("owner_id")]
//     public long? OwnerId { get; set; }
//
//     [Column("description")]
//     public string? Description { get; set; }
//
//     [Column("price")]
//     [Precision(10, 2)]
//     public decimal? Price { get; set; }
//
//     [Column("city")]
//     [StringLength(30)]
//     public string City { get; set; } = null!;
//
//     [Column("picture")]
//     public string? Picture { get; set; }
//
//     [Column("created_at", TypeName = "timestamp without time zone")]
//     public DateTime CreatedAt { get; set; }
//
//     [Column("deleted_at", TypeName = "timestamp without time zone")]
//     public DateTime? DeletedAt { get; set; }
//
//     [ForeignKey("BookId")]
//     [InverseProperty("Offers")]
//     public virtual Book? Book { get; set; }
//
//     [ForeignKey("OwnerId")]
//     [InverseProperty("Offers")]
//     public virtual User? Owner { get; set; }
//
//     [ForeignKey("OfferId")]
//     [InverseProperty("OffersNavigation")]
//     public virtual ICollection<User> Users { get; set; } = new List<User>();
// }
