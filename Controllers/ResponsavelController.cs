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
        public class ResponsavelController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

           public ResponsavelController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObterUsuarioId()
        {            
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost]
        public async Task<IActionResult> AddResponsavelAsync(ResponsavelQr novoResponsavel)
        {

            novoResponsavel.Usuario = await _context.Usuarios.FirstOrDefaultAsync(uBusca => uBusca.Id == ObterUsuarioId());

            //salvamento de dados
            await _context.ResponsaveisQr.AddAsync(novoResponsavel);
            await _context.SaveChangesAsync();

            List<ResponsavelQr> listaResponsaveis = await _context.ResponsaveisQr.ToListAsync();

             return Ok(listaResponsaveis);    
        }

        [HttpGet("GetAll")] 

        public async Task<IActionResult> GetAsync()
        {
        List<ResponsavelQr> listaResponsaveis= new List<ResponsavelQr>();

        listaResponsaveis = await _context.ResponsaveisQr
        .Where(p => p.Usuario.Id == ObterUsuarioId()).ToListAsync();

        return Ok(listaResponsaveis);
        }



        [HttpPut]
        public async Task<IActionResult> UptadeResponsavelAsync(ResponsavelQr p)
        {
            p.Usuario = await _context.Usuarios.FirstOrDefaultAsync(uBusca => uBusca.Id == ObterUsuarioId());
            _context.ResponsaveisQr.Update(p);
            await _context.SaveChangesAsync();

            return Ok(p);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {

        ResponsavelQr pRemover = await _context.ResponsaveisQr.FirstOrDefaultAsync(p => p.Id == id);

        _context.ResponsaveisQr.Remove(pRemover);
        await _context.SaveChangesAsync();

        List<ResponsavelQr> listaResponsaveis = await _context.ResponsaveisQr.ToListAsync();

        return Ok(listaResponsaveis);
        }

    }
}