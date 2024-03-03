// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// [Table("feedbacks")]
// public partial class Feedback
// {
//     [Key]
//     [Column("id")]
//     public long Id { get; set; }
//
//     [Column("book_id")]
//     public long? BookId { get; set; }
//
//     [Column("feedback")]
//     public string Feedback1 { get; set; } = null!;
//
//     [Column("given_by_user_id")]
//     public long? GivenByUserId { get; set; }
//
//     [Column("created_at", TypeName = "timestamp without time zone")]
//     public DateTime CreatedAt { get; set; }
//
//     [Column("deleted_at", TypeName = "timestamp without time zone")]
//     public DateTime? DeletedAt { get; set; }
//
//     [ForeignKey("BookId")]
//     [InverseProperty("Feedbacks")]
//     public virtual Book? Book { get; set; }
//
//     [ForeignKey("GivenByUserId")]
//     [InverseProperty("Feedbacks")]
//     public virtual User? GivenByUser { get; set; }
// }
