using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
    }
}
