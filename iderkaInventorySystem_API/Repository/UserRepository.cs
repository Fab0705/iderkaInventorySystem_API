using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.EntityFrameworkCore;

namespace iderkaInventorySystem_API.Repository
{
    public class UserRepository : iUser
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(User usr, List<string> roleIds)
        {
            try
            {
                var roles = await dbContext.Roles
                    .Where(r => roleIds.Contains(r.IdRol))
                    .ToListAsync();
                usr.IdUsr = GetUserId();
                usr.IdRols = roles; // Asigna los roles al usuario
                dbContext.Users.Add(usr);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Delete(string id)
        {
            var user = await dbContext.Users
                .Include(u => u.IdRols)
                .FirstOrDefaultAsync(u => u.IdUsr == id);

            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<object>> GetAllUsers()
        {
            return await dbContext.Users
                .Select(u => new
                {
                    u.IdUsr,
                    u.Usr,
                    u.Pwd,
                    u.Email,
                    u.IdRols,
                    u.IdLocNavigation.NameSt
                })
                .ToListAsync();
        }

        public async Task<object?> GetUserById(string id)
        {
            return await dbContext.Users
                .Where(u => u.IdUsr == id)
                .Select(u => new
                {
                    u.IdUsr,
                    u.Usr,
                    u.Pwd,
                    u.Email,
                    u.IdRols,
                    u.IdLocNavigation.NameSt
                })
                .FirstOrDefaultAsync();
        }

        public string GetUserId()
        {
            int nextId = 1;

            // hay registros?
            if (dbContext.Users.Any())
            {
                string lastId = dbContext.Users.Max(u => u.IdUsr);

                int numericPart = int.Parse(lastId.Substring(1));

                nextId = numericPart + 1;
            }

            return $"U{nextId:D4}";
        }

        public async Task<object> Login(string username, string password)
        {
            // Buscar en tabla de usuarios normales (con roles)
            var user = await dbContext.Users
                .Where(u => u.Usr == username && u.Pwd == password)
                .Include(u => u.IdRols)
                .FirstOrDefaultAsync();

            // Buscar en tabla de jefes de logística
            var logisticUser = await dbContext.LogisticChiefs
                .Where(u => u.Usr == username && u.Pwd == password)
                .FirstOrDefaultAsync();

            // Si se encontró en la tabla de usuarios
            if (user != null)
            {
                var firstRole = user.IdRols.FirstOrDefault()?.RolName ?? "Sin rol";
                return $"Bienvenido {firstRole} {user.Usr}";
            }

            // Si se encontró en la tabla de jefes de logística
            if (logisticUser != null)
            {
                return $"Bienvenido Jefe de Logística {logisticUser.Usr}";
            }

            // No se encontró en ninguna tabla
            return "Usuario no encontrado";
        }

        public async Task Update(User usr)
        {
            var existingUser = await dbContext.Users
                .Include(u => u.IdRols)
                .FirstOrDefaultAsync(u => u.IdUsr == usr.IdUsr);

            if (existingUser != null)
            {
                existingUser.Usr = usr.Usr;
                existingUser.Pwd = usr.Pwd;
                existingUser.Email = usr.Email;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
