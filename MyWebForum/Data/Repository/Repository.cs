﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MyWebForum.Data.Interfaces;

namespace MyWebForum.Data.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly MyForumContext repositoryContext;

        // Конструктор
        public Repository(MyForumContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        // Витягує всі об'єкти вказаного типу з бд
        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return repositoryContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($" We can`t get entites!!!\n {ex.Message}");
            }
        }

        // Додає новий об'єкт
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null!!!");
            }

            try
            {
                await repositoryContext.AddAsync(entity);
                await repositoryContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        // Оновлює дані об'єкта
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null!!!");
            }

            try
            {
                repositoryContext.Update(entity);
                await repositoryContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}
