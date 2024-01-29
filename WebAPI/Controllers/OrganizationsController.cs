using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Services.Contracts;
using Services.Data.DbContext;
using Services.Implementations;
using System.Diagnostics;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService organizationService;
        public OrganizationsController(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
        }
        [HttpGet]
        public IActionResult GetData()
        {

          
            var data = organizationService.GetData();

            return Ok(data);
        }
    }
}
