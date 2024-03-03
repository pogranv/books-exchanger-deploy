// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore;
//
// namespace BooksExchanger.Entities;
//
// [Table("genres")]
// [Index("Name", Name = "genres_name_key", IsUnique = true)]
// public partial class Genre
// {
//     [Key]
//     [Column("id")]
//     public int Id { get; set; }
//
//     [Column("name")]
//     [StringLength(50)]
//     public string Name { get; set; } = null!;
//
//     [InverseProperty("Genre")]
//     public virtual ICollection<Book> Books { get; set; } = new List<Book>();
//
//     [InverseProperty("Genre")]
//     public virtual ICollection<OffersCollector> OffersCollectors { get; set; } = new List<OffersCollector>();
// }
