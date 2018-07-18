namespace EFCore.Models
{
    public class CatViewModel
    {

        public int Id { get; set; }

        public int TailLength { get; set; }

        public int MeowLoudness { get; set; }

        public string Name { get; set; }

        public CatBreedViewModel Breed { get; set; }


    }
}
