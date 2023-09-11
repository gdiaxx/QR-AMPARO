using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QrAmparoApi.Models;
using QrAmparoApi.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace QrAmparoApi.Controllers
{
    [Authorize]
    [ApiController]   
    [Route("[controller]")] 
    public class IdosoController : ControllerBase
    {     
       private readonly DataContext _context;
       private readonly IHttpContextAccessor _httpContextAccessor;

        public IdosoController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObterUsuarioId()
        {            
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }       

        [HttpGet]                
        public async Task<IActionResult> GetAsync()
        {            
            List<Idoso> idosos = await _context.Idosos
            .Include(p => p.Responsavel)
            .ToListAsync();
            return Ok(idosos);
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleAsync(int id)
        {

            Idoso a = await _context.Idosos
            .Include(p => p.Responsavel)
            .FirstOrDefaultAsync(a => a.Id == id);
            return Ok(a);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(Idoso novoIdoso)
        {
            ResponsavelQr responsavel = await _context.ResponsaveisQr
                .FirstOrDefaultAsync(p => p.Id == novoIdoso.ResponsavelId 
                    && p.Usuario.Id == ObterUsuarioId());

            if(responsavel == null)
                return BadRequest("Seu usuário não contém Responsáveis com o Id do responsável informado.");

            Idoso buscaIdoso= await _context.Idosos
                .FirstOrDefaultAsync(a => a.ResponsavelId == novoIdoso.ResponsavelId);

            if(buscaIdoso != null)
                return BadRequest("O Responsável selecionado já contém uma idodo cadastrado a ele.");
            
            await _context.Idosos.AddAsync(novoIdoso);        
            await _context.SaveChangesAsync();
            
            List<Idoso> idosos = await _context.Idosos.Where(a => a.ResponsavelId == novoIdoso.ResponsavelId).ToListAsync();            
            
            return Ok(idosos);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIdosoAsync(Idoso a)
        {
             _context.Idosos.Update(a);
            await _context.SaveChangesAsync();
            
            return Ok(a);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {   
            Idoso aRemover = _context.Idosos.FirstOrDefault(a => a.Id == id);            
            
            _context.Idosos.Remove(aRemover);            
            _context.SaveChanges();            
           
            List<Idoso> idosos =_context.Idosos.ToList(); 
            
            return Ok(idosos);
        }

    }
}