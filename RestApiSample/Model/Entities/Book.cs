using System;

namespace RestApiSample.Model.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Author Author { get; set; }
        public virtual Guid AuthorId { get; set; }
    }
}
