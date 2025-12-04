
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using CrudStudents.Model;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound(new { message = "Usuario no encontrado" });

        return user;
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validar email único
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return Conflict(new { message = "El email ya existe" });

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
            return BadRequest(new { message = "El ID no coincide" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validar email único (excepto el mismo usuario)
        if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != id))
            return Conflict(new { message = "El email ya existe" });

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == id))
                return NotFound(new { message = "Usuario no encontrado" });
            throw;
        }

        return NoContent();
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return NotFound(new { message = "Usuario no encontrado" });

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}