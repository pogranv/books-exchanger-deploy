// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// [Table("books")]
// public partial class Book
// {
//     [Key]
//     [Column("id")]
//     public long Id { get; set; }
//
//     [Column("title")]
//     [StringLength(100)]
//     public string Title { get; set; } = null!;
//
//     [Column("genre_id")]
//     public int? GenreId { get; set; }
//
//     [Column("sum_rating")]
//     public long? SumRating { get; set; }
//
//     [Column("count_rating")]
//     public long? CountRating { get; set; }
//
//     [Column("created_at", TypeName = "timestamp without time zone")]
//     public DateTime CreatedAt { get; set; }
//
//     [Column("deleted_at", TypeName = "timestamp without time zone")]
//     public DateTime? DeletedAt { get; set; }
//
//     [InverseProperty("Book")]
//     public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
//
//     [ForeignKey("GenreId")]
//     [InverseProperty("Books")]
//     public virtual Genre? Genre { get; set; }
//
//     [InverseProperty("Book")]
//     public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
//
//     [ForeignKey("BookId")]
//     [InverseProperty("Books")]
//     public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
// }
