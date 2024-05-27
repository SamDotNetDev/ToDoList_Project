using ToDoListProject.DAL.Interfaces;
using ToDoListProject.Domain.Entity;

namespace ToDoListProject.DAL.Repositories
{
    public class TaskRepository : IBaseRepository<TaskEntity>
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(TaskEntity entity)
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TaskEntity entity)
        {
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TaskEntity> GetAll()
        {
            return _context.Tasks;
        }

        public async Task<TaskEntity> Update(TaskEntity entity)
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
