using System.Collections.Generic;

namespace EFCore.Models
{
    public class CatBreedViewModel
    {

        public int Id { get; set; }

        public string BreedName { get; set; }

        public List<CatViewModel> Cats { get; set; }
    }
}
