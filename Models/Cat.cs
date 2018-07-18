using System.Collections.Generic;

namespace EFCore.Models
{
    public class Cat : Animal
    {

        public int MeowLoudness { get; set; }

        public int CatBreedId { get; set; }

        public CatBreed Breed { get; set; }

    }
}
