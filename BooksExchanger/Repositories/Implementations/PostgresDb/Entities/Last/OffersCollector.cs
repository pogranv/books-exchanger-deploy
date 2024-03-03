// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// // public enum ModerationStatus
// // {
// //     Submitted,
// //     Consideration,
// //     Approved,
// //     Rejected
// // }
//
// [Table("offers_collector")]
// public partial class OffersCollector
// {
//     [Key]
//     [Column("id")]
//     public Guid Id { get; set; }
//
//     [Column("owner_id")]
//     public long? OwnerId { get; set; }
//
//     [Column("title")]
//     [StringLength(100)]
//     public string Title { get; set; } = null!;
//
//     [Column("genre_id")]
//     public int? GenreId { get; set; }
//
//     [Column("authors")]
//     public string Authors { get; set; } = null!;
//
//     [Column("description")]
//     public string? Description { get; set; }
//
//     [Column("price")]
//     [Precision(10, 2)]
//     public decimal? Price { get; set; }
//
//     [Column("status", TypeName = "moderation_status")]
//     public ModerationStatus ModerationStatus { get; set; }
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
//     [ForeignKey("GenreId")]
//     [InverseProperty("OffersCollectors")]
//     public virtual Genre? Genre { get; set; }
//
//     [ForeignKey("OwnerId")]
//     [InverseProperty("OffersCollectors")]
//     public virtual User? Owner { get; set; }
// }
