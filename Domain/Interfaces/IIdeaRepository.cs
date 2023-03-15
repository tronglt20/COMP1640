﻿namespace Domain.Interfaces
{
    public interface IIdeaRepository : IBaseRepository<Idea>
    {
        Task<Idea> GetAsync(int id);
        IQueryable<Idea> GetById(int id);

        IQueryable<Idea> GetDeactive();

        Task<List<Idea>> GetListAsync(int academicYearId);

    }
}
