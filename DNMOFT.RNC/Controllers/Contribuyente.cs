using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DNMOFT.RNC.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNMOFT.RNC.Controllers
{
    [Route("api/[controller]")]
    public class Contribuyente : ControllerBase
    {
        private readonly DNMOFT.RNC.Context.AppContext _context;
        public Contribuyente(DNMOFT.RNC.Context.AppContext context)
        {
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public mContribuyente GetByRnc(string rnc)
        {
            var result = _context.mContribuyentes.Where(x => x.RNC == rnc).FirstOrDefault();
            return result;
        }

    }
}
