﻿using FurEver_Together.DataModels;

namespace FurEver_Together.ViewModels
{
    public class DogViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public bool Trained { get; set; }
    }
}