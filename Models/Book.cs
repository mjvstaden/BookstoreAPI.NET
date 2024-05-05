using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreAPI.Models
{
    public class Book 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string title { get; set; }
        public required string author { get; set; }
        public required string genre { get; set; }
        public required int price { get; set; }

        [JsonIgnore] // ignore __v
        public int __v { get; set; }
    }

    public class BookRequest
    {
        [Required]
        [StringLength(100)]
        public string title { get; set; }

        [Required]
        [StringLength(100)]
        public string author { get; set; }

        [Required]
        [StringLength(50)]
        public string genre { get; set; }

        [Required]
        [Range(0, 9999)]
        public int price { get; set; }
    }
}