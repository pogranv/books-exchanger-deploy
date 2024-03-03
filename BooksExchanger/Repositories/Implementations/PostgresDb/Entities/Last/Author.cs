// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// [Table("authors")]
// public partial class Author
// {
//     [Key]
//     [Column("id")]
//     public long Id { get; set; }
//
//     [Column("name")]
//     [StringLength(100)]
//     public string Name { get; set; } = null!;
//
//     [ForeignKey("AuthorId")]
//     [InverseProperty("Authors")]
//     public virtual ICollection<Book> Books { get; set; } = new List<Book>();
// }
