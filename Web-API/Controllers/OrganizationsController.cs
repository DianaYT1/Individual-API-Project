using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.Models;

namespace Web_API.Controllers
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

        [HttpPost]
        public IActionResult ReceiveData([FromBody] List<OrganizationModel> data)
        {
            if (data == null || data.Count == 0)
            {
                return BadRequest("No data received.");
            }


            return Ok("Data received and processed successfully.");
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var data = organizationService.GetData();

            return Ok(data);
        }

        [HttpGet("statistics/largest")]
        public IActionResult GetLargestCompanies()
        {
            var largestCompanies = organizationService.GetLargestCompanies();
            return Ok(largestCompanies);
        }

        [HttpGet("statistics/total-employees-by-industry")]
        public IActionResult GetTotalEmployeesByIndustry([FromQuery] string industry)
        {
            if (string.IsNullOrEmpty(industry))
            {
                return BadRequest("Industry parameter is required.");
            }

            var totalEmployees = organizationService.GetTotalEmployeesByIndustry(industry);
            return Ok(totalEmployees);
        }

    }
}
