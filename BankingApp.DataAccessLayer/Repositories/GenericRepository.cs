﻿using BankingApp.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories.EFRepositories
{
    public class GenericRepository<E> : IGenericRepository<E> where E : class
    {
        protected DbContext _context;
        protected DbSet<E> _dbSet;
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<E>();
        }

        public async Task AddAsync(E item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(E item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<E>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task UpdateAsync(E item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<E> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddRangeAsync(IEnumerable<E> item)
        {
            await _dbSet.AddRangeAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<E> items)
        {
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}