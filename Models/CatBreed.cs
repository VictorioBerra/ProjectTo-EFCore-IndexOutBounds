using System.Collections.Generic;

namespace EFCore.Models
{
    public class CatBreed
    {
        public CatBreed()
        {
            Cats = new HashSet<Cat>();
        }

        public int Id { get; set; }

        public string BreedName { get; set; }

        public ICollection<Cat> Cats { get; }
    }
}
