﻿using FurEver_Together.Models;
using FurEver_Together.Interfaces;
using FurEver_Together.DataModels;

namespace FurEver_Together.Repository
{
    public class DogRepository : GenericRepository<Dog>, IDogRepository
    {
        public DogRepository(FurEverTogetherDbContext dbContext) : base(dbContext)
        {
        }
    }
}